using TourCompany.Models.Models;

namespace TourCompany.DL.Interfaces
{
    public interface ICustomerRespository
    {
        public Task<IEnumerable<Reservation>> GetReservations(int customerId);
        public Task<Customer?> GetCustomerById(int customerId);
        public Task<Customer?> GetCustomerByEmail(string email);
        public Task<Customer> CreateAccount(Customer customer);
        public Task<Customer> UpdateAccount(int customerId, Customer customer);
        public Task<Customer?> DeleteAccount(int customerId);
    }
}
