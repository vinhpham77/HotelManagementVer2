using System.Net;
using System.Security.Claims;
using HotelManagement.Services;
using HotelManagement.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Controllers;

[AllowAnonymous]
public class AuthController : Controller
{
    private readonly AccountService _accountService;

    public AuthController(AccountService accountService) =>
        _accountService = accountService;

    public IActionResult Index()
    {
        return RedirectToAction("Login");
    }

    public IActionResult Login()
    {
        return View("Login");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel request, string? returnUrl)
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
                        new(ClaimTypes.Role, accountDto.Role),
                        new("FullName", accountDto.Personnel.FullName)
                    };

                    // Tạo ClaimsIdentity với các claims và chế độ xác thực
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties();

                    // Đăng nhập người dùng
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    if (IsValidReturnUrl(returnUrl))
                    {
                        // Giải mã returnUrl trước khi chuyển hướng
                        var decodedReturnUrl = WebUtility.UrlDecode(returnUrl);

                        // Lấy baseUrl từ request
                        var baseUrl = $"{Request.Scheme}://{Request.Host}";

                        // Nối baseUrl và decodedReturnUrl để tạo đường dẫn hoàn chỉnh
                        var fullPath = $"{baseUrl}{decodedReturnUrl}";

                        return Redirect(fullPath);
                    }

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction("Index");
            }
        }

        return View("Login", request);
    }

    public async Task<IActionResult> Logout()
    {
        Console.WriteLine("Logout");
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Auth");
    }
    
    private bool IsValidReturnUrl(string? returnUrl)
    {
        if (string.IsNullOrEmpty(returnUrl) || !Uri.TryCreate(returnUrl, UriKind.RelativeOrAbsolute, out var returnUri))
        {
            return false;
        }

        // Nếu returnUri là tuyệt đối, kiểm tra xem nó có cùng host với ứng dụng không
        if (returnUri.IsAbsoluteUri)
        {
            return returnUri.Host == Request.Host.Host;
        }

        // Trả về true nếu returnUri là tương đối
        return true;
    }
    
    public IActionResult AccessDenied()
    {
        return View();
    }
}