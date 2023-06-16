using HotelManagement.Models;
using HotelManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HotelManagement.Controllers;

[Authorize (Roles = "admin")] 
public class RoomsController : Controller
{
    private readonly RoomService _roomService;
    private readonly ILogger<RoomsController> _logger;

    public RoomsController(RoomService roomService, ILogger<RoomsController> logger)
    {
        _roomService = roomService;
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
            var rooms = await _roomService.GetAsync(keyword, sort, order, page, size);
            return View(rooms);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error retrieving rooms");
            return View("Error");
        }
    }
    
    [HttpGet]
    public async Task<JsonResult> GetById(string id)
    {
        var room = await _roomService.GetByIdAsync(id);
        return Json(room);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Room room)
    {
        if (ModelState.IsValid)
        {
            var createdRoom = await _roomService.CreateAsync(room);
            return CreatedAtAction(nameof(GetById), new { id = createdRoom?.Id }, createdRoom);
        }
        
        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(string id, [FromBody] Room room)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _roomService.UpdateAsync(id, room);
                return NoContent();
            }
            catch (HttpRequestException)
            {
                return NotFound();
            }
        }
        
        return BadRequest();
    }
    
    [HttpPatch]
    public async Task<IActionResult> Update(string id, [FromBody] JsonPatchDocument<Room> patchDocument)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _roomService.UpdateAsync(id, patchDocument);
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
            await _roomService.DeleteAsync(id);
            return NoContent();
        }
        catch (HttpRequestException)
        {
            return NotFound();
        }
    }
        
    [HttpDelete]
    public async Task<IActionResult> DeleteMany([FromBody] string[] roomIds)
    {
        try
        {
            await _roomService.DeleteManyAsync(roomIds);
            return NoContent();
        }
        catch (HttpRequestException)
        {
            return NotFound();
        }
    }
}