using HotelManagement.Models;
using HotelManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Controllers;

public class CustomersController : Controller
{
    private readonly CustomerService _customerService;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(CustomerService customerService, ILogger<CustomersController> logger)
    {
        _customerService = customerService;
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
            var customers = await _customerService.GetAsync(keyword, sort, order, page, size);
            return View(customers);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error retrieving customers");
            return View("Error");
        }
    }
    
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var customers = await _customerService.GetAllAsync();
            return Json(customers);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error retrieving customers");
            return Json(new LCount<Customer>());
        }
    }
    
    [HttpGet]
    public async Task<JsonResult> GetById(string id)
    {
        var customer = await _customerService.GetByIdAsync(id);
        return Json(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Customer customer)
    {
        if (ModelState.IsValid)
        {
            var createdCustomer = await _customerService.CreateAsync(customer);
            return CreatedAtAction(nameof(GetById), new { id = createdCustomer?.Id }, createdCustomer);
        }
        
        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(string id, [FromBody] Customer customer)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _customerService.UpdateAsync(id, customer);
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
            await _customerService.DeleteAsync(id);
            return NoContent();
        }
        catch (HttpRequestException)
        {
            return NotFound();
        }
    }
        
    [HttpDelete]
    public async Task<IActionResult> DeleteMany([FromBody] string[] customerIds)
    {
        try
        {
            await _customerService.DeleteManyAsync(customerIds);
            return NoContent();
        }
        catch (HttpRequestException)
        {
            return NotFound();
        }
    }
}