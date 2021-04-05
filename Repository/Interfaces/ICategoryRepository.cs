using DataContracts.Entities;
using DataContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Database.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task CreateAsync(Category category);
        Task<Category> GetByIdAsync(Guid categoryId);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Guid categoryId);
        Task UpAsync(Guid categoryId);
        Task DownAsync(Guid categoryId);
        Task<List<CategoryNavigationVM>> GeAllForMenuAsync();
        Task<List<CategoryType>> FindByNameAsync(string name);
        Task<Category> AddAsync(Category category);
//         Task<List<Category>> GetByIdAsync(Guid id);
        Task<Category> DeleteCategoryType(string id);
        Task<List<Category>> GetByNameAsync(string name);
        Task<bool> UpdateStatus(bool status,Guid categoryId);
    }
}
