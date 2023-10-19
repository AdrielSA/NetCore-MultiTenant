using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenant.Application.DTOs;
using MultiTenant.Domain.Contracts.IServices;
using MultiTenant.Domain.Entities;

namespace MultiTenant.Api.Controllers
{
    [Route("api/{slugTenant}/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public ProductController(IMapper mapper, IProductService productService)
        {
            _mapper = mapper;
            _productService = productService;
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute]int id)
        {
            var entity = _productService.GetProduct(id);
            var dto = _mapper.Map<ProductDto>(entity);
            return Ok(dto);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var entities = _productService.GetAllProducts();
            var list = _mapper.Map<List<ProductDto>>(entities);
            return Ok(list);
        }

        [HttpPost]
        public IActionResult Post([FromBody]ProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            var entity = _productService.AddProduct(product);
            dto = _mapper.Map<ProductDto>(entity);
            return Ok(dto);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute]int id, [FromBody]ProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            product.Id = id;
            var entity = _productService.UpdateProduct(product);
            dto = _mapper.Map<ProductDto>(entity);
            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]int id)
        {
            _productService.DeleteProduct(id);
            return Ok($"Producto eliminado: {id}");
        }
    }
}
