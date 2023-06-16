using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace HotelManagement.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập.")]
    public string Username { get; set; } = null!;
    
    [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}