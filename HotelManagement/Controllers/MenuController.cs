using HotelManagement.Models;
using HotelManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Controllers;

public class MenuController : Controller
{
    private readonly MenuService _menuService;
    private readonly ILogger<MenuController> _logger;

    public MenuController(MenuService menuService, ILogger<MenuController> logger)
    {
        _menuService = menuService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? keyword, string? sort, string? order, int? page, int? size)
    {
        ViewBag.Size = size ?? 10;
        ViewBag.Page = page ?? 1;
        ViewBag.Sort = sort ?? "";
        ViewBag.Order = order ?? "";
        ViewBag.Keyword = keyword ?? "";
        
        try
        {
            var menu = await _menuService.GetAsync(keyword, sort, order, page, size);
            return View(menu);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error retrieving menu");
            return View("Error");
        }
    }
    
    [HttpGet]
    public async Task<JsonResult> GetById(string id)
    {
        var menuItem = await _menuService.GetByIdAsync(id);
        return Json(menuItem);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MenuItem menuItem)
    {
        if (ModelState.IsValid)
        {
            var createdMenuItem = await _menuService.CreateAsync(menuItem);
            return CreatedAtAction(nameof(GetById), new { id = createdMenuItem?.Id }, createdMenuItem);
        }
        
        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(string id, [FromBody] MenuItem menuItem)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _menuService.UpdateAsync(id, menuItem);
                return NoContent();
            }
            catch (HttpRequestException)
            {
                return NotFound();
            }
        }
        
        return BadRequest();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _menuService.DeleteAsync(id);
            return NoContent();
        }
        catch (HttpRequestException)
        {
            return NotFound();
        }
    }
        
    [HttpDelete]
    public async Task<IActionResult> DeleteMany([FromBody] string[] menuItemIds)
    {
        try
        {
            await _menuService.DeleteManyAsync(menuItemIds);
            return NoContent();
        }
        catch (HttpRequestException)
        {
            return NotFound();
        }
    }
}