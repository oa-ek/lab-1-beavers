using System.ComponentModel.DataAnnotations;

namespace BaverGame.Domain.Entities;

public class Vote
{
    [Key]
    public Guid VoteId { get; set; } = Guid.NewGuid();
    public Guid CommentId { get; set; }
    public bool IsLike { get; set; } // true for like, false for dislike
    public Guid UserId { get; set; }
    public Comment Comment { get; set; }
}