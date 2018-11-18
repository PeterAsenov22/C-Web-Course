namespace ChushkaWebApp.Web.Controllers
{
    using ChushkaWebApp.Models;
    using ChushkaWebApp.Models.Enums;
    using ViewModels.Products;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using Services.Interfaces;
    using System;

    public class ProductsController : Controller
    {
        private readonly IProductService products;
        private readonly IOrderService orders;
        private UserManager<ChushkaUser> users;

        public ProductsController(IProductService products, IOrderService orders, UserManager<ChushkaUser> users)
        {
            this.products = products;
            this.orders = orders;
            this.users = users;
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            if (!this.products.Exists(id))
            {
                return this.NotFound();
            }

            var product = this.products.FindById(id);

            var productDetailsViewModel = new ProductDetailsViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Type = product.Type
            };

            return this.View(productDetailsViewModel);
        }

        [Authorize]
        public IActionResult Order(int id)
        {
            if (!this.products.Exists(id))
            {
                return this.NotFound();
            }

            var user = this.users.FindByNameAsync(this.User.Identity.Name).Result;
            this.orders.Create(user.Id, id);

            return this.RedirectToAction("Index", "Home");
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

            var productId = this.products.Create(model.Name, model.Price, model.Description, type);

            return this.RedirectToAction("Details", new { id = productId });
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int id)
        {
            if (!this.products.Exists(id))
            {
                return this.NotFound();
            }

            var product = this.products.FindById(id);

            var productUpdateViewModel = new UpdateProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Type = product.Type.ToString()
            };

            return this.View(productUpdateViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(UpdateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            if (!this.products.Exists(model.Id))
            {
                return this.NotFound();
            }

            if (!Enum.TryParse(model.Type, true, out ProductType type))
            {
                return this.View(model);
            }

            this.products.Edit(model.Id, model.Name, model.Price, model.Description, type);

            return this.RedirectToAction("Details", new { id = model.Id });
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            if (!this.products.Exists(id))
            {
                return this.NotFound();
            }

            var product = this.products.FindById(id);

            var productUpdateViewModel = new UpdateProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Type = product.Type.ToString()
            };

            return this.View(productUpdateViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(UpdateProductViewModel model)
        {
            if (!this.products.Exists(model.Id))
            {
                return this.NotFound();
            }

            this.products.Delete(model.Id);

            return this.RedirectToAction("Index", "Home");
        }
    }
}
