using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TourCompany.AutoMapper;
using TourCompany.Controllers;
using TourCompany.DL.Interfaces;
using TourCompany.Models.MediatR.Customers;
using TourCompany.Models.Models;

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

        private readonly IMapper _mapper;
        private readonly Mock<IMediator> _mediator;

        private readonly Mock<ILogger<CustomerController>> _loggerCustomerControllerMock;

        private readonly Mock<ICustomerRespository> _customerRepositoryMock;

        public CustomerTests()
        {
            var mockMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMappings());
            });
            _mapper = mockMapperConfig.CreateMapper();

            _mediator = new Mock<IMediator>();
            _loggerCustomerControllerMock = new Mock<ILogger<CustomerController>>();
            _customerRepositoryMock = new Mock<ICustomerRespository>();
        }

        //[Fact]
        //public async Task Get_Customer_ById_Ok()
        //{
        //    var customerId = 2001;
        //    _customerRepositoryMock.Setup(x => x.GetCustomerById(customerId))
        //                           .ReturnsAsync(_customers.First(x => x.CustomerId == customerId));

        //    _mediator.Setup(x => x.Send(new Mock<GetCustomerByIdCommand>(customerId), new CancellationToken()));

        //    var controller = new CustomerController(_loggerCustomerControllerMock.Object, _mediator.Object);

        //    var result = await controller.GetCustomer(customerId);

        //    var oKObjectResult = result as OkObjectResult;
        //    Assert.NotNull(oKObjectResult);

        //    var customer = oKObjectResult.Value as Customer;
        //    Assert.NotNull(customer);
        //    Assert.Equal(customerId, customer.CustomerId);
        //}
    }
}
