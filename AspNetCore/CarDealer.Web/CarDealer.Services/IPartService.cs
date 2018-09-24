namespace CarDealer.Services
{
    using System.Collections.Generic;
    using Models.Parts;

    public interface IPartService
    {
        IEnumerable<PartListingModel> AllListings(int page, int pageSize);

        IEnumerable<PartBasicModel> All();
        
        PartEditModel ById(int id);

        int Count();

        void Create(string name, decimal price, int quantity, int supplierId);

        void Edit(int id, decimal price, int quantity);

        void Delete(int id);
    }
}
