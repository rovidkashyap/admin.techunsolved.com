using System.ComponentModel.DataAnnotations;
using System.Runtime.Versioning;

namespace admin.techunsolved.com.Models
{
    public class TechArticleDataModel
    {
        [Display(Name = "Article Data ID")]
        public int TechArticlesDataId { get; set; }

        [Display(Name = "Program Heading")]
        public string? program_heading { get; set; }

        [Display(Name = "Program Code")]
        public string? program_code { get; set; }

        [Display(Name = "Output Heading")]
        public string? output_heading { get; set; }

        [Display(Name = "Output Code")]
        public string? output_code { get; set; }

        [Display(Name = "Code Heading")]
        public string? other_heading { get; set; }

        [Display(Name = "Third Code")]
        public string? other_code { get; set; }

        public int? TechArticlesId { get; set; }

        [Display(Name = "Article Name")]
        public string? TechArticlesNames { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modifed On")]
        public DateTime ModifiedDate { get; set; }
    }
}
