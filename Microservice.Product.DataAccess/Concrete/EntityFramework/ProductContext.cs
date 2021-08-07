using Microservice.Product.Entities.Concrete;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Product.DataAccess.Concrete.EntityFramework
{
    public class ProductContext : DbContext
    {
        public static void SetConnectionString(string connectionString)
        {
            if (ConnectionString == null)

            {
                ConnectionString = connectionString;
            }
            else
            {
                throw new Exception();
            }
        }
        // this part will help you to open the connection
        public static SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }

        private static string ConnectionString { get; set; }

        //add the connectionString to options

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        public DbSet<ProductModel> Products { get; set; }
    }
}
