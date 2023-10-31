using System.ComponentModel.DataAnnotations;

namespace BaverGame.DTOs;

public sealed class CommentDto
{
    [Key]
    public string GameId { get; set; }
    public string? ParentCommentId { get; set; } // Nullable for root comments
    public string Content { get; set; }
    public string AuthorName { get; set; }
}