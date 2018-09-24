namespace CarDealer.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Models.PartsViewModels;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PartsController : Controller
    {
        private const int PageSize = 25;

        private readonly IPartService parts;
        private readonly ISupplierService suppliers;

        public PartsController(IPartService parts, ISupplierService suppliers)
        {
            this.parts = parts;
            this.suppliers = suppliers;
        }

        public IActionResult All(int page = 1)
        {
            var partPageListingViewModel = new PartPageListingViewModel
            {
                Parts = this.parts.AllListings(page, PageSize),
                TotalPages = (int)Math.Ceiling(this.parts.Count() / (double)PageSize),
                CurrentPage = page
            };

            return View(partPageListingViewModel);
        }

        public IActionResult Create()
        {
            var partFromViewModel = new PartFormViewModel
            {
                Suppliers = this.GetSupplierListItems()
            };

            return View(partFromViewModel);
        }

        [HttpPost]
        public IActionResult Create(PartFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Suppliers = this.GetSupplierListItems();
                return View(model);
            }

            this.parts.Create(
                model.Name,
                model.Price,
                model.Quantity,
                model.SupplierId);

            return RedirectToAction(nameof(All));
        }

        public IActionResult Delete(int id)
        {
            return View(id);
        }

        public IActionResult Destroy(int id)
        {
            this.parts.Delete(id);

            return RedirectToAction(nameof(All));
        }

        public IActionResult Edit(int id)
        {
            var part = this.parts.ById(id);

            if (part is null)
            {
                return NotFound();
            }

            var partFormViewModel = new PartFormViewModel
            {
                Name = part.Name,
                Price = part.Price,
                Quantity = part.Quantity,
                IsEdit = true
            };

            return this.View(partFormViewModel);
        }

        [HttpPost]
        public IActionResult Edit(int id, PartFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.IsEdit = true;
                return View(model);
            }

            this.parts.Edit(
                id,
                model.Price,
                model.Quantity);

            return RedirectToAction(nameof(All));
        }

        private IEnumerable<SelectListItem> GetSupplierListItems()
        {
            return this.suppliers
                .All()
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                });
        }
    }
}
