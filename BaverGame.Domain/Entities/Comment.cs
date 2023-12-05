using System.ComponentModel.DataAnnotations;

namespace BaverGame.Domain.Entities;

public class Comment
{
    [Key]
    public Guid CommentId { get; set; } = Guid.NewGuid();
    public Guid GameId { get; set; }
    public Guid? ParentCommentId { get; set; } // Nullable for root comments
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Vote> Votes { get; set; }
    public ICollection<Comment> Replies { get; set; } // Child comments

    public string AuthorName { get; set; }
}