using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;
using API.DTOs;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly APIContext _context;

        public CategoriesController(APIContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var categories = await _context.Category
                                           .Include(c => c.Products)
                                           .Select(c => new CategoryDto
                                           {
                                               Id = c.Id,
                                               Name = c.Name,
                                               Products = c.Products.Select(p => new ProductDto
                                               {
                                                   Id = p.Id,
                                                   Name = p.Name,
                                                   Price = p.Price,
                                                   CategoryId = p.CategoryId
                                               }).ToList()
                                           }).ToListAsync();

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _context.Category
                                         .Include(c => c.Products)
                                         .Select(c => new CategoryDto
                                         {
                                             Id = c.Id,
                                             Name = c.Name,
                                             Products = c.Products.Select(p => new ProductDto
                                             {
                                                 Id = p.Id,
                                                 Name = p.Name,
                                                 Price = p.Price,
                                                 CategoryId = p.CategoryId
                                             }).ToList()
                                         }).FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> PostCategory(CategoryDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name,
                // Ne pas ajouter les produits si la liste est vide ou nulle
                Products = categoryDto.Products != null && categoryDto.Products.Any()
                           ? categoryDto.Products.Select(p => new Product
                           {
                               Name = p.Name,
                               Price = p.Price,
                               CategoryId = p.CategoryId
                           }).ToList()
                           : new List<Product>()
            };

            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            categoryDto.Id = category.Id;

            return CreatedAtAction("GetCategory", new { id = category.Id }, categoryDto);
        }

    }
}
