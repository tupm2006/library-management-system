namespace SmartLibraryManagementSystem.Web.Models
{
    public class BookViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Author { get; set; } = default!;
    public int AvailableQuantity { get; set; }
    public string ShelfCode { get; set; } = default!;
}}
