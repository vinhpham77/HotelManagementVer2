using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HotelManagement.Models;
using HotelManagement.Services;
using Microsoft.AspNetCore.Authorization;

namespace HotelManagement.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly AccountService _accountService;

    public HomeController(AccountService accountService) =>
        _accountService = accountService;

    public IActionResult Index()
    {
        return View();
    }
    
    [HttpGet("Profile/{username}")]
    public async Task<IActionResult> Profile(string username)
    {
        try
        {
            var accountDto = await _accountService.GetAccountDtoAsync(username);
            return View(accountDto);
        }
        catch
        {
            return View(null);
        }
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}