namespace LunchBreak.Shared.Models
{
    public class OrderDto
    {
        public string Id { get; set; }
        public string OrderText { get; set; }
        public string Comment { get; set; }
        public decimal Price { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}