namespace ES_FrontEnd.Areas.Admin.Models
{
    public class CustomerModel
    {
        public int? CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
    public class UserDropDown
    {
        public int Userid { get; set; }
        public string UserName { get; set; }
    }
}
