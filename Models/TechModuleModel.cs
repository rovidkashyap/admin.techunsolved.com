using System.ComponentModel.DataAnnotations;

namespace admin.techunsolved.com.Models
{
    public class TechModuleModel
    {
        [Display(Name = "Module ID")]
        public int TechModulesId { get; set; }

        [Display(Name = "Module Name")]
        public string? TechModulesName { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Initial")]
        public string? Initial { get; set; }

        public int? TechSubCategoryId { get; set; }

        [Display(Name = "SubCategory")]
        public string? TechSubCategoryName { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modifed On")]
        public DateTime ModifiedDate { get; set; }

    }
}
