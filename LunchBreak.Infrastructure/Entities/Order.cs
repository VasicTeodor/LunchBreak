namespace LunchBreak.Infrastructure.Entities
{
    public class Order : BaseEntity
    {
        public string OrderText { get; set; }
        public string Comment { get; set; }
        public decimal Price { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}