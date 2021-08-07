using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MassTransit;
using MediatR;
using Microservice.Customer.CQRS.Commands;
using Microservice.Customer.CQRS.Queries;
using Microservice.Customer.Entities.Concrete;
using Microservice.Customer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tesodev.Core.CrossCuttingConcerns.FluentValidation;
using Tesodev.Core.SharedModels;

namespace Microservice.Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IBus _busService;
        private readonly IValidator<CustomerModel> _validator;

        public CustomerController(IMediator mediator, IBus busService, IValidator<CustomerModel> validator)
        {
            _mediator = mediator;
            _busService = busService;
            _validator = validator;
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _mediator.Send(new GetAllCustomersQuery());

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Customer/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var customer = await _mediator.Send(new GetCustomerByIdQuery(id));

                if(customer == null)
                {
                    return Ok(null);
                }

                // In order to get related Address information, response from Address microservice is awaiting
                var response = await GetResponse<GetAddressRequestModel, GetAddressResponseModel>("rabbitmq://localhost/getAddressQueue", new GetAddressRequestModel
                {
                    Id = customer.AddressId
                });               

                var address = new Address();
                if(response.Message != null)
                {
                    address.Id = response.Message.Id;
                    address.AddressLine = response.Message.AddressLine;
                    address.City = response.Message.City;
                    address.CityCode = response.Message.CityCode;
                    address.Country = response.Message.Country;
                }

                var entity = new CustomerDetail
                {
                    Id = customer.Id,
                    Address = address,
                    CreatedAt = customer.CreatedAt,
                    Email = customer.Email,
                    Name = customer.Name,
                    UpdatedAt = customer.UpdatedAt
                };

                return Ok(entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Customer
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomerDetail customerDetail)
        {
            try
            {
                // In order to add new Customer, a new Address should be created
                var response = await GetResponse<CreateAddressRequestModel, CreateAddressResponseModel>("rabbitmq://localhost/createAddressQueue", new CreateAddressRequestModel
                {
                    AddressLine = customerDetail.Address.AddressLine,
                    City = customerDetail.Address.City,
                    CityCode = customerDetail.Address.CityCode,
                    Country = customerDetail.Address.Country
                });

                // If new Address is successfully added, Customer can be created by using AddressId responded by the Address microservice
                if(response.Message.Message == "Success")
                {
                    var model = new CreateCustomerCommand(
                        customerDetail.Name,
                        customerDetail.Email, 
                        response.Message.Id,
                        customerDetail.CreatedAt,
                        customerDetail.UpdatedAt
                    );

                    var res = await _mediator.Send(model);
                    return Ok(res);
                }
                else
                {
                    return BadRequest(response.Message.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Customer/
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CustomerModel customer)
        {
            try
            {
                // In order to update Customer, the Address object must be exist
                var response = await GetResponse<GetAddressRequestModel, GetAddressResponseModel>("rabbitmq://localhost/getAddressQueue", new GetAddressRequestModel
                {
                    Id = customer.AddressId
                });

                // If Address object is exist, then Customer object will be updated
                if (response.Message.Id != 0)
                {
                    var model = new UpdateCustomerCommand(
                        customer.Name,
                        customer.Email,
                        customer.AddressId,
                        customer.CreatedAt,
                        customer.UpdatedAt
                    );

                    var res = await _mediator.Send(model);
                    return Ok(res);
                }
                else
                {
                    return BadRequest($"Address with id:{customer.AddressId} does not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // In order to delete Customer, it must be exist
                var customer = await _mediator.Send(new GetCustomerByIdQuery(id));
                if (customer == null)
                {
                    return BadRequest($"Customer with id:{id} does not found");
                }

                // If Customer is deleted, related Address must also be deleted. Becuse of the one-to-one relation
                var response = await GetResponse<DeleteAddressRequestModel, DeleteAddressResponseModel>("rabbitmq://localhost/deleteAddressQueue", new DeleteAddressRequestModel
                {
                    Id = customer.AddressId
                });

                // If Address is successfully deleted, Customer can be deleted
                if (response.Message.Message == "Success")
                {
                    var res = await _mediator.Send(new DeleteCustomerCommand(id));
                    return Ok(res);
                }
                else
                {
                    return BadRequest(response.Message.Message);
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [Route("Validate/{id}")]
        [HttpGet]
        public async Task<IActionResult> Validate(int id)
        {
            // In order to validate Customer, it must be exist
            var customer = await _mediator.Send(new GetCustomerByIdQuery(id));
            if (customer == null)
            {
                return BadRequest($"Customer with id:{id} does not found");
            }

            try
            {
                ValidatorTool.FluentValidate(_validator, customer);
                return Ok(true);
            }
            catch
            {
                return Ok(false);
            }
        }

        public IRequestClient<T> CreateEndpoint<T>(string url) where T : class
        {
            Uri uri = new Uri(url);
            var endPoint = _busService.CreateRequestClient<T>(uri);

            return endPoint;
        }

        public async Task<Response<TResponse>> GetResponse<TRequest, TResponse>(string uri, TRequest request) where TRequest : class where TResponse : class
        {
            var endPoint = CreateEndpoint<TRequest>(uri);
            var response = await endPoint.GetResponse<TResponse>(request);

            return response;
        }
    }
}
