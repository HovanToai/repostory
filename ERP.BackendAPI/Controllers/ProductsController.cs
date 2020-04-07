using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Common.Catalog.Products;
using ERP.ViewModels.Catalog.ProductImages;
using ERP.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IPublicProductService _publicProductService;
        private readonly IManageProductService _manageProductService;
        public ProductsController(IPublicProductService publicProductService, IManageProductService ManageProductService)
        {
            _publicProductService = publicProductService;
            _manageProductService = ManageProductService;

        }

        // http://localhost:port/product
        //[HttpGet("{languageId}")]
        //public async Task<IActionResult> Get(string languageId)
        //{
        //    var products = await _publicProductService.GetAll(languageId);
        //    return Ok(products);
        //}

        // http://localhost:port/product?pageIndex=1&pagesize=10&CategoryId=1
        [HttpGet("{languageId}")]
        public async Task<IActionResult> GetAllPaging(string languageId, [FromQuery] GetPublicProductPagingRequest request)
        {
            var products = await _publicProductService.GetAllByCategoryId(languageId,   request);
            return Ok(products);
        }


        // http://localhost:port/product/1
        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var products = await _manageProductService.GetById(productId, languageId);
            if (products == null)
                return BadRequest("cannot find product");

            return Ok(products);
        }


        // http://localhost:port/product/create
        //Create
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productId = await _manageProductService.Create(request );
            if (productId == 0)
            
                return BadRequest();
            var product = await _manageProductService.GetById(productId, request.LanguageId);

            //return Created(nameof(GetById),product);
            return CreatedAtAction(nameof(GetById),new { id = productId}, product);
        }

        // Udate
        [HttpPut]
        public async Task<IActionResult> Update([FromForm]ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affectedResult = await _manageProductService.Update(request);
            if (affectedResult == 0)

                return BadRequest();

            return Ok();
        }
        //Delete
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {

            var affectedResult = await _manageProductService.Delete(productId);
            if (affectedResult == 0)

                return BadRequest();

            return Ok();
        }

        //Update Price
        [HttpPut("{productId}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            var isScuccessful = await _manageProductService.UpdatePrice(productId, newPrice);
            if (isScuccessful)

                return Ok();

            return BadRequest();
        }

        //Image
        [HttpPost("{productId}/image")]
        public async Task<IActionResult> CreateImage(int productId, [FromForm]ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var imageId = await _manageProductService.AddImage(productId,request);
            if (imageId == 0)

                return BadRequest();
            var image = await _manageProductService.GetImageById(imageId);

            //return Created(nameof(GetById),product);
            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
        }

        [HttpPut("{productId}/image{imageId}")]
        public async Task<IActionResult> UpdateImage(int imageId, [FromForm]ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _manageProductService.UpdateImage(imageId, request);
            if (result == 0)

                return BadRequest();
            //return Created(nameof(GetById),product);
            return Ok();
        }

        [HttpGet("{productId}/image/{imageId}")]
        public async Task<IActionResult> GetImageById(int productId, int imageId)
        {
            var products = await _manageProductService.GetImageById(imageId);
            if (products == null)
                return BadRequest("cannot find product");

            return Ok(products);
        }

        [HttpDelete("{productId}/image{imageId}")]
        public async Task<IActionResult> RemoveImage(int imageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _manageProductService.RemoveImage(imageId);
            if (result == 0)

                return BadRequest();
            //return Created(nameof(GetById),product);
            return Ok();
        }
    }
}