namespace ES_FrontEnd.Areas.Admin.Models
{
    public class SpecificationModel
    {
        public int? SpecificationId { get; set; }
        public string ModelNumber { get; set; }
        public string Brand { get; set; }
        public int? Sizes { get; set; }
        public string? Resolution { get; set; }
        public string? HDTechnology { get; set; }
        public int? RefreshRate { get; set; }
        public string? SoundTechnology { get; set; }
        public double? Capacity { get; set; }
        public string? Refrigerant { get; set; }
        public int? Voltage { get; set; }
        public string? Color { get; set; }
        public int? Warranty { get; set; }
        public int? StarRating { get; set; }
        public string BrochureUrl { get; set; }
        public int ProductId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
