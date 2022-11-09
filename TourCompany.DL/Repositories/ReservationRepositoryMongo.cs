using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TourCompany.DL.Interfaces;
using TourCompany.Models.Configurations;
using TourCompany.Models.Models;
using TourCompany.Models.Requests;

namespace TourCompany.DL.Repositories
{
    public class ReservationRepositoryMongo : IReservationRepositoryMongo
    {
        private readonly IOptions<MongoDbConfiguration> _options;
        private readonly IMongoClient _dbClient;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<ReservationMongoRequest> _reservationCollection;

        public ReservationRepositoryMongo(IOptions<MongoDbConfiguration> options)
        {
            _options = options;

            _dbClient = new MongoClient(_options.Value.ConnectionString);

            _database = _dbClient.GetDatabase(_options.Value.DatabaseName);

            _reservationCollection = _database.GetCollection<ReservationMongoRequest>(_options.Value.CollectionName);
        }

        public async Task<ReservationMongoRequest> GetById(int reservationId)
        {
            //var filter = Builders<Reservation>.Filter.Eq("ReservationId", reservationId);
            //var doc = _reservationCollection.Find(filter).FirstOrDefault();

            var doc = await _reservationCollection.Find<ReservationMongoRequest>(x => x.ReservationId == reservationId).FirstOrDefaultAsync();

            if (doc == null) return null;

            return doc;
        }

        public Task<ReservationMongoRequest> SaveReservation(Reservation reservation)
        {
            var document = new ReservationMongoRequest()
            {
                ReservationId = reservation.ReservationId,
                CustomerId = reservation.CustomerId,
                CityId = reservation.CityId,
                ReservationDate = reservation.ReservationDate,
                Days = reservation.Days,
                NumberOfPeople = reservation.NumberOfPeople,
                PromoCode = reservation.PromoCode,
                TotalPrice = reservation.TotalPrice
            };

            _reservationCollection.InsertOne(document);

            return Task.FromResult(document);
        }

        public async Task<ReservationMongoRequest> UpdateReservation(Reservation reservation)
        {
            var filter = Builders<ReservationMongoRequest>.Filter.Eq("ReservationId", reservation.ReservationId);
            var update = Builders<ReservationMongoRequest>.Update.Set("CityId", reservation.CityId)
                                                      .Set("ReservationDate", reservation.ReservationDate)
                                                      .Set("Days", reservation.Days)
                                                      .Set("NumberOfPeople", reservation.NumberOfPeople)
                                                      .Set("PromoCode", reservation.PromoCode)
                                                      .Set("TotalPrice", reservation.TotalPrice);

            //var result = _reservationCollection.UpdateOne(filter, update);

            var result = await _reservationCollection.FindOneAndUpdateAsync(filter, update);

            return result;
        }

        public async Task<ReservationMongoRequest?> DeleteReservationById(int reservationId, int customerId)
        {
            var result = _reservationCollection.FindOneAndDelete(x => x.ReservationId == reservationId && x.CustomerId == customerId);

            return result;
        }
    }
}
