namespace ES_FrontEnd.Models
{
    public class BillModel
    {
        public int? BillId { get; set; }
        public int? BillNumber { get; set; }
        public DateTime? BillDate { get; set; }
        public double Amount { get; set; }
        public double Discount { get; set; }
        public double TotalAmount { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
    }
}
