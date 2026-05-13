using ApiEcommerce.Models;
using ApiEcommerce.Models.DTOs;
using ApiEcommerce.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

    [HttpGet("SearchProductByCategory/{categoryName:alpha}", Name = "GetProductByCategory")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetProductsByCategory(string categoryName)
    {
        var category = _categoryRepository.GetCategory(categoryName);

        if (category == null)
            return NotFound($"La categoría con nombre {categoryName} no existe");

        var products = _productRepository.GetProductsForCategory(category.Id);

        var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);

        return Ok(productsDto);
    }

    [HttpGet("SearchProductByTerm/{searchTerm}", Name = "GetProductByTerm")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetProductsByTerm(string searchTerm)
    {
        var products = _productRepository.SearchProducts(searchTerm);

        if (products.Count == 0)
            return NotFound($"No se encontraron productos que coincidan con el término {searchTerm}");

        var mappedProducts = _mapper.Map<IEnumerable<ProductDto>>(products);

        return Ok(mappedProducts);
    }

    [HttpPatch("buyProduct/{name}/{quantity:int}", Name = "BuyProduct")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult BuyProduct(string name, int quantity)
    {
        if (string.IsNullOrEmpty(name) || quantity <= 0)
            return BadRequest("El nombre del producto no puede estar vacío y la cantidad debe ser mayor a cero");

        var foundProduct = _productRepository.ProductExists(name);

        if (!foundProduct)
            return NotFound($"El producto con nombre {name} no existe");

        if (!_productRepository.BuyProduct(name, quantity))
        {
            ModelState.AddModelError("CustomError", $"No se pudo comprar el producto {name} con la cantidad {quantity}");
            return BadRequest(ModelState);
        }
        var units = quantity == 1 ? "unidad" : "unidades";



        return Ok($"Has comprado {quantity} {units} del producto {name} exitosamente");
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

        if (_categoryRepository.CategoryExists(createProductDto.CategoryId) == false)
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

    [HttpPut("{productId:int}", Name = "UpdateProduct")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult UpdateProduct(int productId, [FromBody] UpdateProductDto updateProductDto)
    {
        if (updateProductDto == null)
        {
            ModelState.AddModelError("CustomError", "El producto a actualizar no existe");
            return BadRequest(ModelState);
        }

        if (!_categoryRepository.CategoryExists(updateProductDto.CategoryId))
        {
            ModelState.AddModelError("CustomError", $"La categoría con Id {updateProductDto.CategoryId} no existe");
            return BadRequest(ModelState);
        }

        var product = _mapper.Map<Product>(updateProductDto);
        product.Id = productId;

        if (!_productRepository.UpdateProduct(product))
        {
            ModelState.AddModelError("CustomError", $"Algo salió mal al actualizar el producto {product.Name}");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }


    [HttpDelete("{productId:int}", Name = "DeleteProduct")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteProduct([FromRoute] int productId)
    {
        
        if(productId <= 0)
        {
            ModelState.AddModelError("CustomError", "El Id del producto debe ser mayor a cero");
            return BadRequest(ModelState);
        }

        var product = _productRepository.GetProduct(productId);
        
        if (product == null)
        {
            ModelState.AddModelError("CustomError", $"El producto con Id {productId} no existe");
            return NotFound(ModelState);
        }

        if(!_productRepository.DeleteProduct(product))
        {
            ModelState.AddModelError("CustomError", $"Algo salió mal al eliminar el producto {product.Name}");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
}


