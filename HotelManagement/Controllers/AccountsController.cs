using HotelManagement.Models;
using HotelManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Controllers; 
    
[Authorize(Roles = "admin")]
public class AccountsController : Controller
{
    private readonly AccountService _accountService;
    private readonly ILogger<AccountsController> _logger;

    public AccountsController(AccountService accountService, ILogger<AccountsController> logger)
    {
        _accountService = accountService;
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
            var accounts = await _accountService.GetAsync(keyword, sort, order, page, size);
            return View(accounts);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error retrieving accounts");
            return View("Error");
        }
    }
    
    public async Task<IActionResult> GetUsernames()
    {
        try
        {
            var usernames = await _accountService.GetUsernamesAsync();
            return Json(usernames);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error retrieving usernames");
            return Json(new List<string>());
        }
    }
    
    [HttpGet("Accounts/{username}")]
    public async Task<JsonResult> GetByUsername(string username)
    {
        var account = await _accountService.GetByUsernameAsync(username);
        return Json(account);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Account account)
    {
        if (ModelState.IsValid)
        {
            var createdAccount = await _accountService.CreateAsync(account);
            return CreatedAtAction(nameof(GetByUsername), new { id = createdAccount?.Id }, createdAccount);
        }
        
        return BadRequest();
    }

    [HttpPatch("Accounts/{username}")]
    public async Task<IActionResult> Update(string username, [FromBody] JsonPatchDocument<Account> patchDocument)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _accountService.UpdateAsync(username, patchDocument);
                return NoContent();
            }
            catch (HttpRequestException)
            {
                return NotFound();
            }
        }
        
        return BadRequest();
    }
    
    [HttpPatch("Accounts/ChangeStatus/{username}")]
    public async Task<IActionResult> ChangeStatus(string username, [FromBody] JsonPatchDocument<Account> patchDocument)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _accountService.ChangeStatus(username, patchDocument);
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
            await _accountService.DeleteAsync(id);
            return NoContent();
        }
        catch (HttpRequestException)
        {
            return NotFound();
        }
    }
        
    [HttpDelete]
    public async Task<IActionResult> DeleteMany([FromBody] string[] usernames)
    {
        try
        {
            await _accountService.DeleteManyAsync(usernames);
            return NoContent();
        }
        catch (HttpRequestException)
        {
            return NotFound();
        }
    }
}