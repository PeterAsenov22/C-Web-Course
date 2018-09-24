namespace CarDealer.Web.Controllers
{
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using Services.Models;
    using Models.CustomersViewModels;

    [Route("customers")]
    public class CustomersController : Controller
    {
        private readonly ICustomerService customers;

        public CustomersController(ICustomerService customers)
        {
            this.customers = customers;
        }

        [Route("all/{order?}")]
        public IActionResult All(string order)
        {
            var orderDirection = order != null && order.ToLower() == "descending"
                ? OrderDirection.Descending
                : OrderDirection.Ascending;

            var allCustomers = this.customers.OrderedCustomers(orderDirection);

            var allCustomersViewModel = new AllCustomersViewModel
            {
                Customers = allCustomers,
                OrderDirection = orderDirection
            };

            return View(allCustomersViewModel);
        }

        [Route("{id}")]
        public IActionResult TotalSales(int id)
        {
            return this.ViewOrNotFound(this.customers.TotalSalesById(id));
        }

        [Route(nameof(Create))]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route(nameof(Create))]
        public IActionResult Create(CustomerFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.customers.Create(
                model.Name,
                model.BirthDate,
                model.IsYoungDriver);

            return RedirectToAction(nameof(All));
        }

        [Route(nameof(Edit) + "/{id}")]
        public IActionResult Edit(int id)
        {
            var customer = this.customers.ById(id);
            if (customer is null)
            {
                return NotFound();
            }

            var customerFormViewModel = new CustomerFormViewModel
            {
                Name = customer.Name,
                BirthDate = customer.BirthDate,
                IsYoungDriver = customer.IsYoungDriver
            };

            return View(customerFormViewModel);
        }

        [HttpPost]
        [Route(nameof(Edit) + "/{id}")]
        public IActionResult Edit(int id, CustomerFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var customerExists = this.customers.Exists(id);
            if (!customerExists)
            {
                return NotFound();
            }

            this.customers.Edit(
                id,
                model.Name,
                model.BirthDate,
                model.IsYoungDriver);

            return RedirectToAction(nameof(All));
        }
    }
}
