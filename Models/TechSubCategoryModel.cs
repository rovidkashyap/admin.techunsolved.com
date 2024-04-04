using System.ComponentModel.DataAnnotations;

namespace admin.techunsolved.com.Models
{
    public class TechSubCategoryModel
    {
        [Display(Name = "SubCategory ID")]
        public int TechSubcategoryId { get; set; }

        [Display(Name = "SubCategory Name")]
        public string? TechSubcategoryName { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Initial")]
        public string? Initial { get; set; }
        public int? CategoryId { get; set; }

        [Display(Name = "Category")]
        public string? CategoryName { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modified On")]
        public DateTime ModifiedDate { get; set; }
    }
}
