using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core
{
    public class Like
    {
        [Key]
        public Guid LikeId { get; set; } = Guid.NewGuid();
        public Guid CommentId { get; set; }
        public bool IsLike { get; set; } // true for like, false for dislike
        public Guid UserId { get; set; }
        public Comment Comment { get; set; }
    }
}