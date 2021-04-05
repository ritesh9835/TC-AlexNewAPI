using DataContracts.ViewModels;
using DataContracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.CategoryManager
{
    public interface ICategoryManager
    {
        Task<List<Category>> GetAll();
        Task Create(Category category);
        Task<Category> GetById(Guid categoryId);
        Task Update(Category category);
        Task Delete(Guid categoryId);
        Task Up(Guid categoryId);
        Task Down(Guid categoryId);
        Task<List<CategoryNavigationVM>> GetAllForMenu();
        Task<List<CategoryTypeVM>> FindByName(string name);
        Task<DataContracts.Entities.Category> AddCategory(Category category);
//         Task<List<DataContracts.Entities.Category>> GetTypeById(Guid id);
        Task<DataContracts.Entities.Category> DeleteCategoryType(string id); 
        Task<List<DataContracts.Entities.Category>> GetByName(string name);
        Task<List<DataContracts.Entities.Category>> GetSubCategoryList(Guid categoryId);
        Task<bool> UpdateStatus(Guid categoryId);
    }
}
