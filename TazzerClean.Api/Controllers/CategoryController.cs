using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataContracts.Common;
using DataContracts.Entities;
using DataContracts.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceContracts.CategoryManager;
using TazzerClean.Util;

namespace TazzerClean.Api.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryManager _categoryManager;
        public CategoryController(ILogger<CategoryController> logger, ICategoryManager categoryManager)
        {
            _logger = logger;
            _categoryManager = categoryManager;
        }

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Category>>> GetAsync(CancellationToken cancellationToken)
        {
            try
            {
                var categories = await _categoryManager.GetAll();
                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in CategoryController:GetAsync");
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<List<Category>>> CreateAsync(Category category)
        {
            try
            {
                await _categoryManager.Create(category);

                var categories = await _categoryManager.GetAll();
                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in CategoryController:CreateAsync");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("{categoryId}")]
        [AllowAnonymous]
        public async Task<ActionResult<Category>> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken)
        {
            try
            {
                var category = await _categoryManager.GetById(categoryId);

                if (category == null)
                {
                    return NoContent();
                }
                else
                {
                    return category;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in CategoryController:GetByIdAsync");
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("{categoryId}")]
        [Authorize]
        public async Task UpdateAsync(Category category)
        {
            try
            {
                await _categoryManager.Update(category);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in CategoryController:UpdateAsync");
                BadRequest(ex);
            }
        }

        [HttpDelete]
        [Route("{categoryId}")]
        [Authorize]
        public async Task DeleteAsync(Guid categoryId)
        {
            try
            {
                await _categoryManager.Delete(categoryId);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in CategoryController:DeleteAsync");
                BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("{categoryId}/up")]
        [Authorize]
        public async Task UpAsync(Guid categoryId)
        {
            try
            {
                await _categoryManager.Up(categoryId);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in CategoryController:DeleteAsync");
                BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("{categoryId}/down")]
        [Authorize]
        public async Task DownAsync(Guid categoryId)
        {
            try
            {
                await _categoryManager.Down(categoryId);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in CategoryController:DeleteAsync");
                BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("all")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Category>>> GetAll()
        {
            try
            {
                var categories = await _categoryManager.GetAll();
                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in CategoryController:GetAll");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("getAllCategoriesForMenu")]
        public async Task<ActionResult<List<CategoryNavigationVM>>> GetAllForMenu()
        {
            try
            {
                var categories = await _categoryManager.GetAllForMenu();
                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in CategoryController:GetAll");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("find")]
        public async Task<ActionResult<List<CategoryTypeVM>>> FindByName(string name)
        {
            try
            {
                var result = await _categoryManager.FindByName(name);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in CategoryController:FindByName");
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("add")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Category>> AddCategory([FromBody] Category request)
        {
            try
            {
                var categoryType = new Category
                {

                };

                var result = await _categoryManager.AddCategory(categoryType);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in CategoryController:AddCategory");
                return BadRequest(ex);
            }
        }

        //[HttpGet]
        //[Route("getById")]
        //public async Task<ActionResult<List<Category>>> GetById(string id)
        //{
        //    try
        //    {
        //        var guidId = Guid.Parse(id);
        //        var result = await _categoryManager.GetTypeById(guidId);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("An error has been thrown in CategoryController:GetTypeById");
        //        return BadRequest(ex);
        //    }
        //}

        [HttpPost]
        [Route("deleteCategoryType")]
        [AllowAnonymous]
        public async Task<ActionResult<Category>> DeleteCategoryType(string id)
        {
            try
            {
                var result = await _categoryManager.DeleteCategoryType(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in CategoryController:DeleteCategoryType");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("getByName")]
        public async Task<ActionResult<List<Category>>> GetByName(string name)
        {
            try
            {
                var result = await _categoryManager.GetByName(name);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in CategoryController:GetTypeById");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("getSubCategoryList")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Category>>> SubCategoryList(Guid categoryId)
        {
            try
            {
                var result = await _categoryManager.GetSubCategoryList(categoryId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in CategoryController:GetTypeById");
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("updateStatus")]
        [Authorize]
        public async Task<IActionResult> UpdateStatus(Guid categoryId)
        {
            var res = await _categoryManager.UpdateStatus(categoryId);
            return Ok( new ResponseViewModel<bool>
            {
                Data = res,
                Message = res ? TazzerCleanConstants.SavedSuccess : TazzerCleanConstants.GeneralError
            });
        }
    }
}
