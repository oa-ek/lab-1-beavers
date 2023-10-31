using System.Collections;
using System.ComponentModel.DataAnnotations;
using Core;

namespace BaverGame.DTOs;

public sealed class ExamplePageDto
{
    [Key]
    public Game Game { get; set; }

    public string ParentCommentId { get; set; }
    public string CommentContent { get; set; }
    public string GameId { get; set; }

    public Dictionary<string, int> CommentsLikesCount = new Dictionary<string, int>();
    public Dictionary<string, int> CommentsDislikesCount = new Dictionary<string, int>();

    public int GetLikesFor(string commentId) =>
        CommentsLikesCount.TryGetValue(commentId, out var result)
            ? result
            : default(int);
    
    public int GetDislikesFor(string commentId) =>
        CommentsDislikesCount.TryGetValue(commentId, out var result)
            ? result
            : default(int);
}