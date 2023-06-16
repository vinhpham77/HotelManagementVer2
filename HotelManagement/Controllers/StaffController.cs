using HotelManagement.Models;
using HotelManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Controllers;

[Authorize (Roles = "admin")]
public class StaffController : Controller
{
    private readonly StaffService _staffService;
    private readonly ILogger<StaffController> _logger;

    public StaffController(StaffService staffService, ILogger<StaffController> logger)
    {
        _staffService = staffService;
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
            var staff = await _staffService.GetAsync(keyword, sort, order, page, size);
            return View(staff);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error retrieving staff");
            return View("Error");
        }
    }
    
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var staff = await _staffService.GetAllAsync();
            return Json(staff);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error retrieving staff");
            return Json(new LCount<Personnel>());
        }
    }
    
    [HttpGet]
    public async Task<JsonResult> GetById(string id)
    {
        var personnel = await _staffService.GetByIdAsync(id);
        return Json(personnel);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Personnel personnelIn)
    {
        if (ModelState.IsValid)
        {
            var personnel = await _staffService.CreateAsync(personnelIn);
            return CreatedAtAction(nameof(GetById), new { id = personnel?.Id }, personnel);
        }
        
        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(string id, [FromBody] Personnel personnel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _staffService.UpdateAsync(id, personnel);
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
            await _staffService.DeleteAsync(id);
            return NoContent();
        }
        catch (HttpRequestException)
        {
            return NotFound();
        }
    }
        
    [HttpDelete]
    public async Task<IActionResult> DeleteMany([FromBody] string[] personnelIds)
    {
        try
        {
            await _staffService.DeleteManyAsync(personnelIds);
            return NoContent();
        }
        catch (HttpRequestException)
        {
            return NotFound();
        }
    }
}