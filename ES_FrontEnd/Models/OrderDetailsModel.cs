namespace ES_FrontEnd.Models
{
    public class OrderDetailsModel
    {
        public int? OrderDetailId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public double Amount { get; set; }
        public double TotalAmount { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
    }
}
