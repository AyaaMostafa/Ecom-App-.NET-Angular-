using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Sharing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{

    public class ProductController : BaseController
    {
        public ProductController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> Get([FromQuery]  ProductParams productParams)
        {
            try
            {


                var product = await _work.ProductRepository
                    .GetAllAsync(productParams);
                var totalCount = await _work.ProductRepository.CountAsync();
                return Ok(new Pagination<ProductDTO>(productParams.PageNumber, productParams.PageSize, totalCount, product));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-by-id/{id}" )]
        public async Task<IActionResult> GetById(int id )
        {
            try
            {
                var product = await _work.ProductRepository.GetById(id,
                    x => x.Category, x => x.Photos);
                var result = _mapper.Map<ProductDTO>(product);
                if (product == null)
                    return BadRequest(new ResponseAPI(400));
                return Ok(result);
            }
            catch (Exception ex)
            {

               return BadRequest(ex.Message) ;
            }
        }
        [HttpPost("Add-Product")]
        public async Task<IActionResult> add(AddPrroductDTO productDTO)
        {
            try
            {
                await _work.ProductRepository.AddAsync(productDTO);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseAPI(400,ex.Message));
            }
        }


        [HttpPut("Update-Product")]
        public async Task<IActionResult> Update(UpdateProductDTO updateProductDTO)
        {
            try
            {
                await _work.ProductRepository.UpdateAsync(updateProductDTO);
                return Ok( new ResponseAPI(200));
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }

        [HttpDelete("Delete-Product/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await _work.ProductRepository.GetById(id, x => x.Photos, x => x.Category);
                await _work.ProductRepository.DeleteAsync(product);  
                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseAPI(400, ex.Message));
            }

        }

    }
}
