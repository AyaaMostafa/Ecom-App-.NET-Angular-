using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{

    public class CategoriesController : BaseController
    {
        public CategoriesController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var category = await _work.CategoryRepository.GetAllAsync();
                if (category == null)
                    return BadRequest();
                return Ok(category);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await _work.CategoryRepository.GetByIdAsync(id);
                if (category == null)
                    return BadRequest();
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("add-category")]
        public async Task<IActionResult> Add(CategoryDTO categoryDTO)
        {
            try
            {
                var category =  _mapper.Map<Category>(categoryDTO); //Mapping
                await _work.CategoryRepository.AddAsync(category);
                return Ok(new { message = " Item has been added " });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPut("update-category")]
        public async Task<IActionResult> update(updateCategoryDTO categoryDTO)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryDTO); //Mapping
                await _work.CategoryRepository.UpdateAsync(category);
                return Ok(new { message = " Item has been updated " });

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("delete-category/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _work.CategoryRepository.DeleteAsync(id);
                return Ok(new { message = " Item has been deleted " });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}