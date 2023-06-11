using HotelManagement.Models;
using HotelManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace HotelManagement.Controllers;

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
        try
        {
            var roomTypes = await _roomTypeService.GetAsync(keyword, sort, order, page, size);
            return View(roomTypes?.Items);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error retrieving room types");
            return View("Error");
        }
    }
    
    [HttpGet]
    public async Task<JsonResult> GetById(string id)
    {
        var roomType = await _roomTypeService.GetByIdAsync(id);
        return Json(roomType);
    }

    [HttpPost]
    public async Task<JsonResult> Create([FromBody] RoomType roomType)
    {
        if (ModelState.IsValid)
        {
            var createdRoomType = await _roomTypeService.CreateAsync(roomType);
            return Json(new { success = createdRoomType != null, roomType = createdRoomType });
        }
        return Json(new { success = false });
    }

    [HttpPut]
    public async Task<JsonResult> Update(string id, [FromBody] RoomType roomType)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _roomTypeService.UpdateAsync(id, roomType);
                return Json(new { success = true });
            }
            catch (HttpRequestException)
            {
                return Json(new { success = false });
            }
        }
        return Json(new { success = false });
    }

    [HttpDelete]
    public async Task<JsonResult> Delete(string id)
    {
        try
        {
            await _roomTypeService.DeleteAsync(id);
            return Json(new { success = true });
        }
        catch (HttpRequestException)
        {
            return Json(new { success = false });
        }
    }
        
    [HttpDelete]
    public async Task<JsonResult> DeleteMany([FromBody] string[] roomTypeIds)
    {
        try
        {
            await _roomTypeService.DeleteManyAsync(roomTypeIds);
            return Json(new { success = true });
        }
        catch (HttpRequestException)
        {
            return Json(new { success = false });
        }
    }
}