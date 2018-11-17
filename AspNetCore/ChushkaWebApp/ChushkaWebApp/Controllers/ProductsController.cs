namespace ChushkaWebApp.Web.Controllers
{
    using Services.Interfaces;
    using ChushkaWebApp.Models.Enums;
    using ViewModels.Products;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;

    public class ProductsController : Controller
    {
        private readonly IProductService products;
        public ProductsController(IProductService products)
        {
            this.products = products;
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(CreateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            if (!Enum.TryParse(model.Type, true, out ProductType type))
            {
                return this.View(model);
            }

            this.products.Create(model.Name, model.Price, model.Description, type);

            return this.RedirectToAction("Index", "Home");
        }
    }
}
