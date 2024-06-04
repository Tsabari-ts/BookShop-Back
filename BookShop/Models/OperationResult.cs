namespace BookShop.Models
{
    public class OperationResult
    {
        public bool IsSuccessful { get; set; } = false;
        public string? Message { get; set; }
    }
}