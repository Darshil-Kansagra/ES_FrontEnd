namespace ES_FrontEnd.Models
{
    public class ProductModel
    {
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ProductType { get; set; }
        public string ImageUrl { get; set; }
        public int StockQuantity { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class SearchProductModel
    {
        public string Keyword { get; set; }
        public string ProductType { get; set; }
    }
}
