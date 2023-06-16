using HotelManagement.Models;
using HotelManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Controllers;

[Authorize (Roles = "admin")]
public class RoomTypesController : Controller
{
    private readonly RoomTypeService _roomTypeService;
    private readonly ILogger<RoomTypesController> _logger;

    public RoomTypesController(RoomTypeService roomTypeService, ILogger<RoomTypesController> logger)
    {
        _roomTypeService = roomTypeService;
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
            var roomTypes = await _roomTypeService.GetAsync(keyword, sort, order, page, size);
            return View(roomTypes);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error retrieving room types");
            return View("Error");
        }
    }
    
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var roomTypes = await _roomTypeService.GetAllAsync();
            return Json(roomTypes);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error retrieving room types");
            return Json(new LCount<RoomType>());
        }
    }
    
    [HttpGet]
    public async Task<JsonResult> GetById(string id)
    {
        var roomType = await _roomTypeService.GetByIdAsync(id);
        return Json(roomType);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoomType roomType)
    {
        if (ModelState.IsValid)
        {
            var createdRoomType = await _roomTypeService.CreateAsync(roomType);
            return CreatedAtAction(nameof(GetById), new { id = createdRoomType?.Id }, createdRoomType);
        }
        
        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(string id, [FromBody] RoomType roomType)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _roomTypeService.UpdateAsync(id, roomType);
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
            await _roomTypeService.DeleteAsync(id);
            return NoContent();
        }
        catch (HttpRequestException)
        {
            return NotFound();
        }
    }
        
    [HttpDelete]
    public async Task<IActionResult> DeleteMany([FromBody] string[] roomTypeIds)
    {
        try
        {
            await _roomTypeService.DeleteManyAsync(roomTypeIds);
            return NoContent();
        }
        catch (HttpRequestException)
        {
            return NotFound();
        }
    }
}