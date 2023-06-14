namespace HotelManagement.Models;

public class TableOptions
{
    public string Column { get; set; } = null!;
    public string Sort { get; set; } = null!;
    public string Order { get; set; } = null!;
    public string ActionName { get; set; } = null!;
    public string ControllerName { get; set; } = null!;
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string Keyword { get; set; } = "";
}