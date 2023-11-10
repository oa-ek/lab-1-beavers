using System.Collections;
using System.ComponentModel.DataAnnotations;
using Core;

namespace BaverGame.DTOs;

public sealed class GamePageDto
{
    [Key]
    public Game Game { get; set; }

    public string? ParentCommentId { get; set; }
    public string CommentContent { get; set; }
    public string GameId { get; set; }

    public Dictionary<string, int> CommentsLikesCount = new();
    public Dictionary<string, int> CommentsDislikesCount = new();

    public int GetLikesFor(string commentId) =>
        CommentsLikesCount.TryGetValue(commentId, out var result)
            ? result
            : default(int);
    
    public int GetDislikesFor(string commentId) =>
        CommentsDislikesCount.TryGetValue(commentId, out var result)
            ? result
            : default(int);
}