using System.Collections.Generic;

namespace CarDealer.Web.Controllers
{
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Models.SalesViewModels;
    using Services;
    using System.Linq;

    [Route("sales")]
    public class SalesController : Controller
    {
        private readonly ICarService cars;
        private readonly ICustomerService customers;
        private readonly ISaleService sales;

        public SalesController(ICarService cars, ICustomerService customers, ISaleService sales)
        {
            this.cars = cars;
            this.customers = customers;
            this.sales = sales;
        }

        [Route("")]
        public IActionResult All()
        {
            var salesViewModel = new SalesViewModel
            {
                Type = "All Sales",
                Sales = this.sales.All()
            };

            return View(salesViewModel);
        }

        [Route("{id}")]
        public IActionResult Details(int id)
        {
            return this.ViewOrNotFound(this.sales.Details(id));
        }

        [Route("discounted/{percent?}")]
        public IActionResult Discounted(double percent)
        {
            SalesViewModel salesViewModel;
            var discounted = this.sales.Discounted();

            if (percent > 0)
            {
                salesViewModel = new SalesViewModel
                {
                    Type = $"Sales With {percent} Percent Discount",
                    Sales = discounted.Where(s => (s.Discount*100 + (s.IsYoungDriver ? 5 : 0)).Equals(percent))
                };
            }
            else
            {
                salesViewModel = new SalesViewModel
                {
                    Type = "Discounted Sales",
                    Sales = discounted
                };
            }

            return View("All", salesViewModel);
        }

        [Route("add")]
        public IActionResult Add()
        {
            var saleFormViewModel = new SaleFormViewModel
            {
                Customers = this.GetCustomersSelectListItems(),
                Cars = this.GetCarsSelectListItems()
            };

            return View(saleFormViewModel);
        }

        [Route("add")]
        [HttpPost]
        public IActionResult Add(SaleFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Customers = this.GetCustomersSelectListItems();
                model.Cars = this.GetCarsSelectListItems();

                return View(model);
            }

            TempData["customerId"] = model.CustomerId;
            TempData["carId"] = model.CarId;
            TempData["discount"] = model.Discount;

            return RedirectToAction(nameof(Confirm));
        }

        [Route("add/confirm")]
        public IActionResult Confirm()
        {
            ViewData["customerId"] = TempData["customerId"];
            ViewData["carId"] = TempData["carId"];
            ViewData["discount"] = TempData["discount"];

            int customerId;
            int carId;
            double discount;

            bool customerParseSuccess = int.TryParse(TempData["customerId"].ToString(), out customerId);
            bool carParseSuccess = int.TryParse(TempData["carId"].ToString(), out carId);
            bool discountParseSuccess = double.TryParse(TempData["discount"].ToString(), out discount);

            if (!customerParseSuccess || !carParseSuccess || !discountParseSuccess)
            {
                return RedirectToAction(nameof(Add));
            }

            var customerByIdModel = this.customers.ById(customerId);
            var carByIdModel = this.cars.ById(carId);

            if (customerByIdModel is null || carByIdModel is null)
            {
                return RedirectToAction(nameof(Add));
            }

            var confirmSaleViewModel = new ConfirmSaleViewModel
            {
                CustomerId = customerId,
                CarId = carId,
                Discount = discount,
                IsYoungDriver = customerByIdModel.IsYoungDriver,
                CustomerName = customerByIdModel.Name,
                CarFullName = carByIdModel.FullName,
                CarPrice = carByIdModel.Price
            };

            return View(confirmSaleViewModel);
        }

        [HttpPost]
        [Route("add/confirm")]
        public IActionResult Confirm(ConfirmSaleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Add));
            }

            if (!this.cars.Exists(model.CarId) || !this.customers.Exists(model.CustomerId))
            {
                return RedirectToAction(nameof(Add));
            }

            this.sales.Create(model.CustomerId, model.CarId, model.Discount * 0.01);

            return RedirectToAction(nameof(All));
        }

        private IEnumerable<SelectListItem> GetCustomersSelectListItems()
        {
            return this.customers
                .All()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                });
        }

        private IEnumerable<SelectListItem> GetCarsSelectListItems()
        {
            return this.cars
                .AllWithFullName()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                });
        }
    }
}
