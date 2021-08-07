using MassTransit;
using MediatR;
using Microservice.Order.CQRS.Commands;
using Microservice.Order.CQRS.Queries;
using Microservice.Order.Entities.Concrete;
using Microservice.Order.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tesodev.Core.Entities;
using Tesodev.Core.SharedModels;
using Tesodev.Core.SharedModels.Customer.Request;
using Tesodev.Core.SharedModels.Customer.Response;
using Tesodev.Core.SharedModels.Product;
using Tesodev.Core.SharedModels.Product.Request;
using Tesodev.Core.SharedModels.Product.Response;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Microservice.Order.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IBus _busService;

        public OrderController(IMediator mediator, IBus busService)
        {
            _mediator = mediator;
            _busService = busService;
        }

        // GET: api/<OrderController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _mediator.Send(new GelAllOrdersQuery());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var order = await _mediator.Send(new GetOrderByIdQuery(id));

                if (order == null)
                {
                    return Ok(null);
                }

                // In order to get related Address information, response from Address microservice is awaiting
                var addressEndPoint = CreateEndpoint<GetAddressRequestModel>("rabbitmq://localhost/getAddressQueue");
                var addressResponse = await addressEndPoint.GetResponse<GetAddressResponseModel>(new GetAddressRequestModel
                {
                    Id = order.AddressId
                });

                var address = new Address();
                if (addressResponse.Message != null)
                {
                    address.Id = addressResponse.Message.Id;
                    address.AddressLine = addressResponse.Message.AddressLine;
                    address.City = addressResponse.Message.City;
                    address.CityCode = addressResponse.Message.CityCode;
                    address.Country = addressResponse.Message.Country;
                }

                // In order to get related Product information, response from Product microservice is awaiting
                var productEndPoint = CreateEndpoint<GetProductRequestModel>("rabbitmq://localhost/getProductQueue");
                var productResponse = await productEndPoint.GetResponse<GetProductResponseModel>(new GetProductRequestModel
                {
                    Id = order.ProductId
                });

                var product = new Product();
                if (productResponse.Message != null)
                {
                    product.Id = productResponse.Message.Id;
                    product.ImageUrl = productResponse.Message.ImageUrl;
                    product.Name = productResponse.Message.Name;
                }

                var entity = new OrderDetail
                {
                    Address = address,
                    CreatedAt = order.CreatedAt,
                    CustomerId = order.CustomerId,
                    Id = order.Id,
                    Price = order.Price,
                    Product = product,
                    Quantity = order.Quantity,
                    Status = order.Status,
                    UpdatedAt = order.UpdatedAt
                };

                return Ok(entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("GetListByCustomerId/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetListByCustomerId(int id)
        {
            try
            {
                var res = await _mediator.Send(new GetOrdersByCustomerIdQuery(id));
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // POST api/<OrderController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderDetail orderDetail)
        {
            try
            {
                // In order to add new Order, customer with customerId should be exist. If not it will throw exception
                await ExecuteRequest<GetCustomerRequestModel, GetCustomerResponseModel>("rabbitmq://localhost/getCustomerQueue", "Customer", orderDetail.CustomerId, new GetCustomerRequestModel
                {
                    Id = orderDetail.CustomerId
                });

                // In order to add new Order, a new Product should be created. If not it will throw exception
                var newProductId = await ExecuteRequest<CreateProductRequestModel, CreateProductResponseModel>("rabbitmq://localhost/createProductQueue", new CreateProductRequestModel
                {
                    ImageUrl = orderDetail.Product.ImageUrl,
                    Name = orderDetail.Product.Name
                });

                // In order to add new Order, a new Adress should be created. If not it will throw exception
                var newAddressId = await ExecuteRequest<CreateAddressRequestModel, CreateAddressResponseModel>("rabbitmq://localhost/createAddressQueue", new CreateAddressRequestModel
                {
                    AddressLine = orderDetail.Address.AddressLine,
                    City = orderDetail.Address.City,
                    CityCode = orderDetail.Address.CityCode,
                    Country = orderDetail.Address.Country              
                });
          
                var model = new CreateOrderCommand()
                {
                    AddressId = newAddressId,
                    CreatedAt = orderDetail.CreatedAt,
                    CustomerId = orderDetail.CustomerId,
                    Price = orderDetail.Price,
                    ProductId = newProductId,
                    Quantity = orderDetail.Quantity,
                    Status = orderDetail.Status,
                    UpdatedAt = orderDetail.UpdatedAt
                };

                var res = await _mediator.Send(model);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("UpdateStatus/{id}")]
        [HttpPatch]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            try
            {
                var model = new UpdateOrderStatusCommand
                {
                    Id = id,
                    Status = status
                };

                var res = await _mediator.Send(model);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<OrderController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] OrderDetail orderDetail)
        {
            try
            {
                // In order to update Order, the Customer object must be exist. If not it will throw exception
                await ExecuteRequest<GetCustomerRequestModel, GetCustomerResponseModel>("rabbitmq://localhost/getCustomerQueue", "Customer", orderDetail.CustomerId, new GetCustomerRequestModel
                {
                    Id = orderDetail.CustomerId
                });

                // In order to update Order, the Address object may be updated.
                await ExecuteRequest<UpdateAddressRequestModel, UpdateAddressResponseModel>("rabbitmq://localhost/updateAddressQueue", new UpdateAddressRequestModel
                {
                    AddressLine = orderDetail.Address.AddressLine,
                    City = orderDetail.Address.City,
                    CityCode = orderDetail.Address.CityCode,
                    Country = orderDetail.Address.Country,
                    Id = orderDetail.Address.Id
                });

                // In order to update Order, the Product object may be updated.
                await ExecuteRequest<UpdateProductRequestModel, UpdateProductResponseModel>("rabbitmq://localhost/updateProductQueue",  new UpdateProductRequestModel
                {
                    ImageUrl = orderDetail.Product.ImageUrl,
                    Name = orderDetail.Product.Name,
                    Id = orderDetail.Product.Id
                });

                var model = new UpdateOrderCommand
                {
                    AddressId = orderDetail.Address.Id,
                    CreatedAt = orderDetail.CreatedAt,
                    CustomerId = orderDetail.CustomerId,
                    Id = orderDetail.Id,
                    Price = orderDetail.Price,
                    ProductId = orderDetail.Product.Id,
                    Quantity = orderDetail.Quantity,
                    Status = orderDetail.Status,
                    UpdatedAt = orderDetail.UpdatedAt
                };

                var res = await _mediator.Send(model);

                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // In order to delete Order, it must be exist
                var order = await _mediator.Send(new GetOrderByIdQuery(id));
                if (order == null)
                {
                    return BadRequest($"Order with id:{id} does not found");
                }

                // If Order is deleted, related Address must also be deleted. Becuse of the one-to-one relation
                await ExecuteRequest<DeleteAddressRequestModel, DeleteAddressResponseModel>("rabbitmq://localhost/deleteAddressQueue", new DeleteAddressRequestModel
                {
                    Id = order.AddressId
                });

                // If Order is deleted, related Product must also be deleted. Becuse of the one-to-one relation
                await ExecuteRequest<DeleteProductRequestModel, DeleteProductResponseModel>("rabbitmq://localhost/deleteProductQueue", new DeleteProductRequestModel
                {
                    Id = order.ProductId
                });

                var res = await _mediator.Send(new DeleteOrderCommand(id));
                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        public async Task<int> ExecuteRequest<TRequest, TResponse>(string uri, string entityType, int entityId, TRequest request) where TRequest : class where TResponse : class, IResponsable
        {
            var response = await GetResponse<TRequest, TResponse>(uri, request);

            if (response.Message.Id == 0)
            {
                throw new Exception($"{entityType} with id:{entityId} does not exist");
            }

            return response.Message.Id;
        }

        public async Task<int> ExecuteRequest<TRequest, TResponse>(string uri,  TRequest request) where TRequest : class where TResponse : class, IResponsable
        {
            var response = await GetResponse<TRequest, TResponse>(uri, request);

            if(response.Message.Message != "Success")
            {
                throw new Exception(response.Message.Message);
            }

            return response.Message.Id;
        }

        private IRequestClient<T> CreateEndpoint<T>(string url) where T : class
        {
            Uri uri = new Uri(url);
            var endPoint = _busService.CreateRequestClient<T>(uri);

            return endPoint;
        }

        private async Task<Response<TResponse>> GetResponse<TRequest, TResponse>(string uri, TRequest request) where TRequest : class where TResponse : class
        {
            var endPoint = CreateEndpoint<TRequest>(uri);
            var response = await endPoint.GetResponse<TResponse>(request);

            return response;
        }
    }
}
