using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using TourCompany.DL.Interfaces;
using TourCompany.Models.Models;

namespace TourCompany.DL.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ILogger<ReservationRepository> _logger;
        private readonly IConfiguration _configuration;
        public ReservationRepository(ILogger<ReservationRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<Reservation?> GetById(int reservationId, int customerId)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    return await conn.QueryFirstOrDefaultAsync<Reservation>(@"SELECT * FROM Reservation WITH(NOLOCK) 
                                                                            WHERE ReservationId = @ReservationId 
                                                                            AND CustomerId = @CustomerId", 
                                                                            new { ReservationId = reservationId, CustomerId = customerId});
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetById)} - {ex.Message}", ex);
                return new Reservation();
            }
        }

        public async Task<Reservation> CreateReservation(Reservation reservation)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var query = @"INSERT INTO Reservation (CustomerId, ReservationDate, CityId, Days, NumberOfPeople, PromoCode, TotalPrice)
                                OUTPUT INSERTED.ReservationId, INSERTED.CustomerId, INSERTED.ReservationDate, INSERTED.CityId, INSERTED.Days, INSERTED.NumberOfPeople, INSERTED.PromoCode   
                                VALUES (@CustomerId, @ReservationDate, @CityId, @Days, @NumberOfPeople, @PromoCode, @TotalPrice)";

                    var result = await conn.QueryFirstOrDefaultAsync<Reservation>(query, reservation);

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(CreateReservation)} - {ex.Message}", ex);
                return null;
            }
        }

        public async Task<Reservation> UpdateReservation(int reservationId, Reservation reservation)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var query = @$"UPDATE Reservation SET
                                 CustomerId = @CustomerId, ReservationDate = @ReservationDate, CityId = @CityId, 
                                 Days = @Days, NumberOfPeople = @NumberOfPeople, PromoCode = @PromoCode, TotalPrice = @TotalPrice
                                 OUTPUT INSERTED .*
                                 WHERE ReservationId = {reservationId}";

                    var result = await conn.QueryFirstOrDefaultAsync<Reservation>(query, reservation);

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(UpdateReservation)} - {ex.Message}", ex);
                return null;
            }
        }

        public async Task<Reservation?> DeleteReservationById(int reservationId, int customerId)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    return await conn.QueryFirstOrDefaultAsync<Reservation>(@"DELETE FROM Reservation
                                                                            WHERE ReservationId = @ReservationId 
                                                                            AND CustomerId = @CustomerId",
                                                                            new { ReservationId = reservationId, CustomerId = customerId });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(DeleteReservationById)} - {ex.Message}", ex);
                return new Reservation();
            }
        }
    }
}
