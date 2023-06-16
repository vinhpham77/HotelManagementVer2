using System.Security.Claims;
using HotelManagement.Models;
using HotelManagement.Services;
using HotelManagement.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Controllers;

public class LoginController : Controller
{
    private readonly AccountService _accountService;

    public LoginController(AccountService accountService) =>
        _accountService = accountService;

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Auth(LoginViewModel request)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var accountDto = await _accountService.LoginAsync(request);

                if (accountDto is not null)
                {
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, request.Username),
                        new(ClaimTypes.Role, accountDto.Role)
                    };

                    // Tạo ClaimsIdentity với các claims và chế độ xác thực
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties();

                    // Đăng nhập người dùng
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    // Chuyển hướng người dùng đến trang chủ sau khi đăng nhập thành công
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction("Index");
            }
        }
        
        return View("Index", request);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Login");
    }

    private Task<AccountDto?> GetAccountDto(LoginViewModel request)
    {
        return _accountService.LoginAsync(request);
    }
}