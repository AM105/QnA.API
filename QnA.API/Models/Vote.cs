using System.ComponentModel.DataAnnotations.Schema;

namespace QnA.API.Models
{
public class Vote
    {
        public int Id { get; set; }
        
        // 1 for upvote, -1 for downvote
        public int Value { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        
        // These foreign keys are nullable because a vote can only be for one entity
        public int? QuestionId { get; set; }
        
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }
        
        public int? AnswerId { get; set; }
        
        [ForeignKey("AnswerId")]
        public virtual Answer Answer { get; set; }
        
        public int? CommentId { get; set; }
        
        [ForeignKey("CommentId")]
        public virtual Comment Comment { get; set; }
    }
}