using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TourCompany.AutoMapper;
using TourCompany.BL.CommandHandlers.CustomersHandlers;
using TourCompany.BL.Kafka;
using TourCompany.DL.Interfaces;
using TourCompany.Models.MediatR.Customers;
using TourCompany.Models.Models;
using TourCompany.Models.Requests;

namespace TourCompany.UnitTests
{
    public class CustomerTests
    {
        private List<Customer> _customers = new List<Customer>()
        {
            new Customer()
            {
                CustomerId = 2000,
                CustomerName = "Johny D",
                Email = "johny.d@mail.com",
                Telephone = "0884567891"
            },
            new Customer()
            {
                CustomerId = 2001,
                CustomerName = "Jessy D",
                Email = "jessy.d@mail.com",
                Telephone = "0884567788"
            }
        };
        private List<Reservation> _reservations = new List<Reservation>()
        {
            new Reservation()
            {
                CustomerId = 2001
            },
            new Reservation()
            {
                CustomerId = 2001
            }
        };

        private readonly IMapper _mapper;

        private readonly Mock<ILogger<GetCustomerByIdCommandHandler>> _loggerGetCustomerByIdMock;
        private readonly Mock<ILogger<GetCustomerReservationsCommandHandler>> _loggerGetCustomerReservationsMock;
        private readonly Mock<ILogger<CreateAccountCommandHandler>> _loggerCreateAccountMock;
        private readonly Mock<ILogger<UpdateCustomerCommandHandler>> _loggerUpdateAccountMock;
        private readonly Mock<ILogger<DeleteCustomerCommandHandler>> _loggerDeleteAccountMock;

        private readonly Mock<IProducerService<int, Customer>> _producer;

        private readonly Mock<ICustomerRespository> _customerRepositoryMock;

        public CustomerTests()
        {
            var mockMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMappings());
            });
            _mapper = mockMapperConfig.CreateMapper();

            _loggerGetCustomerByIdMock = new Mock<ILogger<GetCustomerByIdCommandHandler>>(); 
            _loggerGetCustomerReservationsMock = new Mock<ILogger<GetCustomerReservationsCommandHandler>>();
            _loggerCreateAccountMock = new Mock<ILogger<CreateAccountCommandHandler>>();
            _loggerUpdateAccountMock = new Mock<ILogger<UpdateCustomerCommandHandler>>();
            _loggerDeleteAccountMock = new Mock<ILogger<DeleteCustomerCommandHandler>>();

            _producer = new Mock<IProducerService<int, Customer>>();
            _customerRepositoryMock = new Mock<ICustomerRespository>();
        }

        [Fact]
        public async Task Get_Customer_ById_Ok()
        {
            var customerId = 2001;
            _customerRepositoryMock.Setup(x => x.GetCustomerById(customerId))
                                   .ReturnsAsync(_customers.First(x => x.CustomerId == customerId));

            var command = new GetCustomerByIdCommand(customerId);
            var handler = new GetCustomerByIdCommandHandler(_loggerGetCustomerByIdMock.Object, _customerRepositoryMock.Object);

            var result = await handler.Handle(command, new CancellationToken());

            Assert.NotNull(result);
            Assert.Equal(customerId, result.CustomerId);
        }

        [Fact]
        public async Task Get_Customer_ById_NotOk()
        {
            var customerId = 2003;
            _customerRepositoryMock.Setup(x => x.GetCustomerById(customerId))
                                   .ReturnsAsync(_customers.FirstOrDefault(x => x.CustomerId == customerId));

            var command = new GetCustomerByIdCommand(customerId);
            var handler = new GetCustomerByIdCommandHandler(_loggerGetCustomerByIdMock.Object, _customerRepositoryMock.Object);

            var result = await handler.Handle(command, new CancellationToken());

            Assert.Equal(null, result);
        }

        [Fact]
        public async Task Get_Customer_Reservations_Ok()
        {
            var customerId = 2001;
            _customerRepositoryMock.Setup(x => x.GetCustomerById(customerId))
                                   .ReturnsAsync(_customers.First(x => x.CustomerId == customerId));
            _customerRepositoryMock.Setup(x => x.GetReservations(customerId))
                                   .ReturnsAsync(_reservations.FindAll(x => x.CustomerId == customerId));

            var command = new GetCustomerReservationsCommand(customerId);
            var handler = new GetCustomerReservationsCommandHandler(_loggerGetCustomerReservationsMock.Object, _customerRepositoryMock.Object);

            var result = await handler.Handle(command, new CancellationToken());

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task Get_Customer_Reservations_NotOk()
        {
            var customerId = 2003;
            _customerRepositoryMock.Setup(x => x.GetCustomerById(customerId))
                                   .ReturnsAsync(_customers.FirstOrDefault(x => x.CustomerId == customerId));

            var command = new GetCustomerReservationsCommand(customerId);
            var handler = new GetCustomerReservationsCommandHandler(_loggerGetCustomerReservationsMock.Object, _customerRepositoryMock.Object);

            var result = await handler.Handle(command, new CancellationToken());

            Assert.Equal(null, result);
        }

        [Fact]
        public async Task Create_Account_Ok()
        {
            var customerRequest = new CustomerRequest()
            {
                CustomerName = "Mickail",
                Email = "m.m@mail.com",
                Telephone = "0887788799"
            };
            var expectedId = 2002;
            _customerRepositoryMock.Setup(x => x.CreateAccount(It.IsAny<Customer>()))
                                   .Callback(() =>
                                   {
                                       _customers.Add(new Customer()
                                       {
                                           CustomerId = expectedId,
                                           CustomerName = customerRequest.CustomerName,
                                           Email = customerRequest.Email,
                                           Telephone = customerRequest.Telephone
                                       });
                                   })!.ReturnsAsync(() => _customers.FirstOrDefault(x => x.CustomerId == expectedId));

            var command = new CreateAccountCommand(customerRequest);
            var handler = new CreateAccountCommandHandler(_loggerCreateAccountMock.Object, _customerRepositoryMock.Object, _mapper, _producer.Object);

            var result = await handler.Handle(command, new CancellationToken());

            Assert.NotNull(result);

            Assert.Equal("You Successfully Created Your account!", result.Message);
            Assert.Equal(expectedId, result.Customer.CustomerId);
        }

        [Fact]
        public async Task Create_Account_NotOk()
        {
            var customerRequest = new CustomerRequest()
            {
                CustomerName = "Jessy",
                Email = "jessy.d@mail.com",
                Telephone = "0887788799"
            };
            var expectedId = 2002;
            _customerRepositoryMock.Setup(x => x.GetCustomerByEmail(customerRequest.Email))
                                   .ReturnsAsync(() => _customers.FirstOrDefault(x => x.Email == customerRequest.Email));

            var command = new CreateAccountCommand(customerRequest);
            var handler = new CreateAccountCommandHandler(_loggerCreateAccountMock.Object, _customerRepositoryMock.Object, _mapper, _producer.Object);

            var result = await handler.Handle(command, new CancellationToken());

            Assert.Equal(null, result.Customer);
            Assert.Equal("This account already exists.", result.Message);
        }

        [Fact]
        public async Task Update_Account_OK()
        {
            var customerRequest = new CustomerRequest()
            {
                CustomerName = "Jessy D",
                Email = "jes.dd@mail.com",
                Telephone = "0884567788"
            };
            var updateId = 2001;
            _customerRepositoryMock.Setup(x => x.GetCustomerById(updateId))
                                   .ReturnsAsync(() => _customers.FirstOrDefault(x => x.CustomerId == updateId));
            _customerRepositoryMock.Setup(x => x.UpdateAccount(updateId, It.IsAny<Customer>()))
                                   .Callback(() =>
                                   {
                                       _customers.Add(new Customer()
                                       {
                                           CustomerId = updateId,
                                           CustomerName = customerRequest.CustomerName,
                                           Email = customerRequest.Email,
                                           Telephone = customerRequest.Telephone
                                       });
                                   })!.ReturnsAsync(() => _customers.Last(x => x.CustomerId == updateId));

            var command = new UpdateAccountCommand(updateId, customerRequest);
            var handler = new UpdateCustomerCommandHandler(_loggerUpdateAccountMock.Object, _customerRepositoryMock.Object, _mapper);

            var result = await handler.Handle(command, new CancellationToken());

            Assert.NotNull(result);

            Assert.Equal("You Successfully Updated Your Account!", result.Message);
            Assert.Equal(updateId, result.Customer.CustomerId);
            Assert.Equal(customerRequest.Email, result.Customer.Email);
        }

        [Fact]
        public async Task Update_Account_NotOK()
        {
            var customerRequest = new CustomerRequest()
            {
                CustomerName = "Jessy D",
                Email = "jesd@mail.com",
                Telephone = "0884567788"
            };
            var updateId = 2002;
            _customerRepositoryMock.Setup(x => x.GetCustomerById(updateId))
                                   .ReturnsAsync(() => _customers.FirstOrDefault(x => x.CustomerId == updateId));

            var command = new UpdateAccountCommand(updateId, customerRequest);
            var handler = new UpdateCustomerCommandHandler(_loggerUpdateAccountMock.Object, _customerRepositoryMock.Object, _mapper);

            var result = await handler.Handle(command, new CancellationToken());

            Assert.Equal(null, result.Customer);

            Assert.Equal("The account doesn't exist.", result.Message);
        }

        [Fact]
        public async Task Delete_Account_Ok()
        {
            var customerId = 2001;
            _customerRepositoryMock.Setup(x => x.GetCustomerById(customerId))!
                               .ReturnsAsync(_customers.FirstOrDefault(x => x.CustomerId == customerId));
            _customerRepositoryMock.Setup(x => x.DeleteAccount(customerId))
                               .Callback(() =>
                               {
                                   _customers.RemoveAll(x => x.CustomerId == customerId);
                               })!.ReturnsAsync(_customers.FirstOrDefault(x => x.CustomerId == customerId));

            var command = new DeleteAccountCommand(customerId);
            var handler = new DeleteCustomerCommandHandler(_loggerDeleteAccountMock.Object, _customerRepositoryMock.Object);

            var result = await handler.Handle(command, new CancellationToken());

            Assert.Equal("You Successfully Deleted Your Account.", result.Message);
        } 

        [Fact]
        public async Task Delete_Account_NotOk()
        {
            var customerId = 2002;
            _customerRepositoryMock.Setup(x => x.DeleteAccount(customerId))
                               .ReturnsAsync(_customers.FirstOrDefault(x => x.CustomerId == customerId));

            var command = new DeleteAccountCommand(customerId);
            var handler = new DeleteCustomerCommandHandler(_loggerDeleteAccountMock.Object, _customerRepositoryMock.Object);

            var result = await handler.Handle(command, new CancellationToken());

            Assert.Equal("This account doesn't exist.", result.Message);
        }
    }
}
