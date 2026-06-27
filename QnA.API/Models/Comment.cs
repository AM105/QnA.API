using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace QnA.API.Models
{
    public class Comment
    {
        public int Id { get; set; }
        
        [Required]
        public string Body { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        
        [ForeignKey("Answer")]
        public int AnswerId { get; set; }
        public virtual Answer Answer { get; set; }
        
        // Self-referencing relationship for nested comments
        public int? ParentId { get; set; }
        
        [ForeignKey("ParentId")]
        public virtual Comment Parent { get; set; }
        
        public virtual ICollection<Vote> Votes { get; set; }
    }
}