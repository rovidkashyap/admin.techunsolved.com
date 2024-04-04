using System.ComponentModel.DataAnnotations;

namespace admin.techunsolved.com.Models
{
    public class TechArticleModel
    {
        [Display(Name = "Article ID")]
        public int TechArticleId { get; set; }

        [Display(Name = "Heading")]
        public string? main_Heading { get; set; }

        [Display(Name = "Sub Heading")]
        public string? main_subHeading { get; set; }

        [Display(Name = "Introduction Heading")]
        public string? intro_Heading { get; set; }

        [Display(Name = "Introduction")]
        public string? intro_Data { get; set; }

        [Display(Name = "Content Heading")]
        public string? content_Heading { get; set; }

        [Display(Name = "Content")]
        public string? content_Data { get; set; }

        [Display(Name = "Example")]
        public string? example_content { get; set; }

        [Display(Name = "Output")]
        public string? output_content { get; set; }

        [Display(Name = "Conclusion")]
        public string? conclusion_data { get; set; }

        [Display(Name = "Other Content")]
        public string? other_Content { get; set; }

        [Display(Name = "Topic Name")]
        public int? TechTopicId { get; set; }
        public string? TechTopicName { get; set; }

        public int? TechModuleId { get; set; }
        public string? TechModuleName { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modified On")]
        public DateTime ModifiedDate { get; set; }
    }
}
