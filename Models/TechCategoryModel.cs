using System.ComponentModel.DataAnnotations;

namespace admin.techunsolved.com.Models
{
    public class TechCategoryModel
    {
        [Display(Name = "Category ID")]
        public int TechCategoryId { get; set; }

        [Display(Name = "Category Name")]
        public string? TechCategoryName { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Initial")]
        public string? Initial { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modified On")]
        public DateTime ModifiedDate { get; set; }
    }
}
