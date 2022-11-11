using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TourCompany.DL.Interfaces;
using TourCompany.Models.Configurations;
using TourCompany.Models.Models;
using TourCompany.Models.Requests;

namespace TourCompany.DL.Repositories
{
    public class CustomerRepositoryMongo : ICustomerRespositoryMongo
    {
        private readonly IOptions<MongoDbConfiguration> _options;
        private readonly IMongoClient _dbClient;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<CustomerMongoRequest> _customerCollection;

        public CustomerRepositoryMongo(IOptions<MongoDbConfiguration> options)
        {
            _options = options;

            _dbClient = new MongoClient(_options.Value.ConnectionString);

            _database = _dbClient.GetDatabase(_options.Value.DatabaseName);

            _customerCollection = _database.GetCollection<CustomerMongoRequest>(_options.Value.CustomerCollectionName);
        }

        public async Task<CustomerMongoRequest> GetCustomerById(int customerId)
        {
            var doc = await _customerCollection.Find<CustomerMongoRequest>(x => x.CustomerId == customerId).FirstOrDefaultAsync();

            if (doc == null) return null;

            return doc;
        }

        public async Task<CustomerMongoRequest> SaveCustomer(Customer customer)
        {
            var document = new CustomerMongoRequest()
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                Email = customer.Email,
                Telephone = customer.Telephone
            };

            await _customerCollection.InsertOneAsync(document);

            return document;
        }

        public async Task<CustomerMongoRequest> UpdateCustomer(Customer customer)
        {
            var filter = Builders<CustomerMongoRequest>.Filter.Eq("CustomerId", customer.CustomerId);
            var update = Builders<CustomerMongoRequest>.Update.Set("CustomerName", customer.CustomerName)
                                                              .Set("Email", customer.Email)
                                                              .Set("Telephone", customer.Telephone);

            var result = await _customerCollection.FindOneAndUpdateAsync(filter, update);

            return result;
        }

        public async Task<CustomerMongoRequest> DeleteCustomer(int customerId)
        {
            var result = _customerCollection.FindOneAndDelete(x => x.CustomerId == customerId);

            return result;
        }
    }
}
