using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using TourCompany.DL.Interfaces;
using TourCompany.Models.Models;

namespace TourCompany.DL.Repositories
{
    public class CustomerRepository : ICustomerRespository
    {
        private readonly ILogger<CustomerRepository> _logger;
        private readonly IConfiguration _configuration;
        public CustomerRepository(ILogger<CustomerRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<IEnumerable<Reservation>> GetReservations(int customerId)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    return await conn.QueryFirstOrDefaultAsync<IEnumerable<Reservation>>(@"SELECT * FROM Reservation WITH(NOLOCK) 
                                                                            WHERE CustomerId = @CustomerId",
                                                                            new { CustomerId = customerId });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetReservations)} - {ex.Message}", ex);
                return Enumerable.Empty<Reservation>();
            }
        }

        public async Task<Customer?> GetCustomerById(int customerId)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    return await conn.QueryFirstOrDefaultAsync<Customer>(@"SELECT * FROM Customer WITH(NOLOCK) 
                                                                            WHERE CustomerId = @CustomerId",
                                                                            new { CustomerId = customerId });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetCustomerById)} - {ex.Message}", ex);
                return new Customer();
            }
        }

        public async Task<Customer?> GetCustomerByEmail(string email)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    return await conn.QueryFirstOrDefaultAsync<Customer>(@"SELECT * FROM Customer WITH(NOLOCK) 
                                                                            WHERE Email = @Email",
                                                                            new { Email = email });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetCustomerByEmail)} - {ex.Message}", ex);
                return new Customer();
            }
        }

        public async Task<Customer> CreateAccount(Customer customer)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var query = @"INSERT INTO Customer (CustomerName, Email, Telephone)
                                           VALUES (@CustomerName, @Email, @Telephone)";

                    var result = await conn.QueryAsync<Customer>(query, customer);
                    return customer;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(CreateAccount)} - {ex.Message}", ex);
                return null;
            }
        }

        public async Task<Customer> UpdateAccount(int customerId, Customer customer)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var query = @$"UPDATE Customer SET
                                 CustomerName = @CustomerName, Email = @Email, Telephone = @Telephone
                                 WHERE CustomerId = {customerId}";

                    var result = await conn.ExecuteAsync(query, customer);
                    return customer;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(UpdateAccount)} - {ex.Message}", ex);
                return null;
            }
        }

        public async Task<Customer?> DeleteAccount(int customerId)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    return await conn.QueryFirstOrDefaultAsync<Customer>(@"DELETE FROM Customer
                                                                            WHERE CustomerId = @CustomerId",
                                                                            new { CustomerId = customerId });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(DeleteAccount)} - {ex.Message}", ex);
                return new Customer();
            }
        }
    }
}
