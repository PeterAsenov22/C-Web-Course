namespace ChushkaWebApp.Services.Interfaces
{
    using Models.Enums;

    public interface IProductService
    {
        void Create(string name, decimal price, string description, ProductType type);
    }
}
