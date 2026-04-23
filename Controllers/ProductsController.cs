using ApiEcommerce.Models;
using ApiEcommerce.Models.DTOs;
using ApiEcommerce.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiEcommerce.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public ProductsController(
        IProductRepository productRepository,
        IMapper mapper,
        ICategoryRepository categoryRepository
    )
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _categoryRepository = categoryRepository;
    }
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult GetProducts()
    {
        var products = _productRepository.GetProducts();
        var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);
        return Ok(productsDto);
    }

    [HttpGet("{productId:int}", Name = "GetProduct")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetProduct(int productId)
    {
        var product = _productRepository.GetProduct(productId);

        if (product == null)
            return NotFound($"El producto con Id {productId} no existe");

        var productDto = _mapper.Map<ProductDto>(product);

        return Ok(productDto);

    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult CreateProduct([FromBody] CreateProductDto createProductDto)
    {
        if (createProductDto == null)
        {
            return BadRequest(ModelState);
        }

        if (_productRepository.ProductExists(createProductDto.Name))
        {
            ModelState.AddModelError("CustomError", $"El producto {createProductDto.Name} ya existe");
            return Conflict(ModelState);
        }

        if(_categoryRepository.CategoryExists(createProductDto.CategoryId) == false)
        {
            ModelState.AddModelError("CustomError", $"La categoría con Id {createProductDto.CategoryId} no existe");
            return BadRequest(ModelState);
        }

        var product = _mapper.Map<Product>(createProductDto);

        if (!_productRepository.CreateProduct(product))
        {
            ModelState.AddModelError("CustomError", $"Algo salió mal al guardar el producto {product.Name}");
            return StatusCode(500, ModelState);
        }
        var createdProductDto = _productRepository.GetProduct(product.Id);
        var productDto = _mapper.Map<ProductDto>(createdProductDto);

        return CreatedAtRoute("GetProduct", new { productId = product.Id }, productDto);
    }

}


