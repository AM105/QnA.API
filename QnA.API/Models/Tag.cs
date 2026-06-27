using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QnA.API.Models
{
    public class Tag
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        [MaxLength(255)]
        public string Description { get; set; }
        
        // Navigation property
        public virtual ICollection<QuestionTag> QuestionTags { get; set; }
    }
}