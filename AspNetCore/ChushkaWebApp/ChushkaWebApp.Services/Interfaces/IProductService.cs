namespace ChushkaWebApp.Services.Interfaces
{
    using ChushkaWebApp.Models.Enums;
    using Models;
    using System.Collections.Generic;

    public interface IProductService
    {
        int Create(string name, decimal price, string description, ProductType type);

        void Edit(int id, string name, decimal price, string description, ProductType type);

        void Delete(int id);

        bool Exists(int id);

        ProductModel FindById(int id);

        IEnumerable<ProductModel> All();
    }
}
