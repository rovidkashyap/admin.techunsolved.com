using System.ComponentModel.DataAnnotations;

namespace admin.techunsolved.com.Models
{
    public class TechTopicModel
    {
        [Display(Name = "Topic ID")]
        public int TechTopicId { get; set; }

        [Display(Name = "Topic Name")]
        public string? TechTopicName { get; set; }

        [Display(Name = "Level")]
        public string? Level { get; set; }

        public int? TechModulesId { get; set; }

        [Display(Name = "Module Name")]
        public string? TechModulesName { get; set; }
    }
}
