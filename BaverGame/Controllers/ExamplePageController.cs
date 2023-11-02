using BaverGame.DTOs;
using Core;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BaverGame.Controllers;

public sealed class ExamplePageController : Controller
{
    private readonly IRepository<Game> _gamesRepository;
    private readonly IRepository<Comment> _commentRepository;
    private readonly IRepository<Vote> _voteRepository;
    private readonly UserManager<User> _userManager;

    public ExamplePageController(
        IRepository<Game> gamesRepository,
        IRepository<Comment> commentRepository,
        IRepository<Vote> voteRepository, 
        UserManager<User> userManager)
    {
        _gamesRepository = gamesRepository;
        _commentRepository = commentRepository;
        _voteRepository = voteRepository;
        _userManager = userManager;
    }

    public async Task<ViewResult> Index()
    {
        ExamplePageDto examplePageDto = await CreateDto();
        return View(examplePageDto);
    }

    private async Task<ExamplePageDto> CreateDto()
    {
        var game = (await _gamesRepository.GetAllEntitiesAsync()).First();
        List<Comment> allComments = await _commentRepository.GetAllEntitiesAsync();
        game.Comments = allComments
            .Where(x => x.GameId == game.GameId && x.ParentCommentId is null)
            .ToList();

        var examplePageDto = new ExamplePageDto
        {
            Game = game,
            GameId = game.GameId.ToString(),
        };

        await GetRepliesForCollection(game.Comments, allComments, examplePageDto);
        return examplePageDto;
    }

    private async Task GetRepliesForCollection(IEnumerable<Comment> comments, IReadOnlyList<Comment> allComments, ExamplePageDto dto)
    {
        foreach(var comment in comments)
        {
            dto.CommentsLikesCount.Add(comment.CommentId.ToString(), await GetCommentLikesCount(comment));
            dto.CommentsDislikesCount.Add(comment.CommentId.ToString(), await GetCommentDislikesCount(comment));
            
            ICollection<Comment> replies = GetRepliesFor(allComments, comment);
            comment.Replies = replies;
            if(!replies.Any())
                continue;

            await GetRepliesForCollection(replies, allComments, dto);
        }
    }

    private async Task<int> GetCommentLikesCount(Comment comment) =>
        (await _voteRepository.GetAllEntitiesAsync())
        .Where(x => x.IsLike)
        .Count(x => x.CommentId == comment.CommentId);
    
    private async Task<int> GetCommentDislikesCount(Comment comment) =>
        (await _voteRepository.GetAllEntitiesAsync())
        .Where(x => !x.IsLike)
        .Count(x => x.CommentId == comment.CommentId);

    private static ICollection<Comment> GetRepliesFor(IReadOnlyList<Comment> allComments, Comment comment) =>
        allComments.Where(x => x.ParentCommentId == comment.CommentId).ToList();

    [HttpPost]
    public async Task<IActionResult> AddComment(ExamplePageDto dto)
    {
        var comment = new Comment
        {
            GameId = Guid.Parse(dto.GameId),
            Content = dto.CommentContent,
            CreatedAt = DateTime.Now,
            CommentId = Guid.NewGuid(),
            AuthorName = User.Identity!.Name!,
            ParentCommentId = Guid.Parse(dto.ParentCommentId),
        };

        await _commentRepository.AddNewEntityAsync(comment);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> VoteComment(Guid commentId, bool isLike)
    {
        var vote = new Vote
        {
            CommentId = commentId,
            IsLike = isLike,
            UserId = _userManager.Users.First(user => user.UserName == User.Identity!.Name).Id,
        };

        var votes = await _voteRepository.GetAllEntitiesAsync();
        Vote? previousVote = votes.FirstOrDefault(x => x.UserId == vote.UserId && x.CommentId == vote.CommentId);
        if(previousVote is not null)
        {
            _voteRepository.RemoveExistingEntity(previousVote);
        }
        
        await _voteRepository.AddNewEntityAsync(vote);
        var dto = await CreateDto();
        return RedirectToAction("Index", dto);
    }
}