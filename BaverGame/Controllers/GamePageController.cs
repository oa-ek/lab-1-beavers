using BaverGame.DTOs;
using Core;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BaverGame.Controllers;

public sealed class GamePageController : Controller
{
    private readonly IRepository<Game> _gamesRepository;
    private readonly IRepository<Comment> _commentRepository;
    private readonly IRepository<Vote> _voteRepository;
    private readonly UserManager<User> _userManager;
    private readonly IRepository<Screenshot> _screenshotRepository;
    private readonly IRepository<GameTag> _tagsRepository;
    private readonly IRepository<Price> _priceRepository;
    private readonly IRepository<Publisher> _publisherRepository;
    private readonly IRepository<Developer> _developerRepository;

    // [ActivatorUtilitiesConstructor]
    public GamePageController(
        IRepository<Game> gamesRepository,
        IRepository<Comment> commentRepository,
        IRepository<Vote> voteRepository, 
        UserManager<User> userManager,
        IRepository<Screenshot> screenshotRepository,
        IRepository<GameTag> tagsRepository,
        IRepository<Price> priceRepository, 
        IRepository<Publisher> publisherRepository,
        IRepository<Developer> developerRepository)
    {
        _gamesRepository = gamesRepository;
        _commentRepository = commentRepository;
        _voteRepository = voteRepository;
        _userManager = userManager;
        _screenshotRepository = screenshotRepository;
        _tagsRepository = tagsRepository;
        _priceRepository = priceRepository;
        _publisherRepository = publisherRepository;
        _developerRepository = developerRepository;
    }

    public async Task<ViewResult> Index(Guid id)
    {
        GamePageDto gamePageDto = await CreateDto(id);
        return View(gamePageDto);
    }

    private async Task GetRepliesForCollection(IEnumerable<Comment> comments, IReadOnlyList<Comment> allComments, GamePageDto dto)
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

    [HttpPost]
    public async Task<IActionResult> VoteComment(Guid commentId, bool isLike, Guid id)
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
        return RedirectToAction("Index", new { id = id });
    }

    [HttpPost]
    public async Task<IActionResult> AddComment(GamePageDto dto)
    {
        var comment = new Comment
        {
            GameId = Guid.Parse(dto.GameId),
            Content = dto.CommentContent,
            CreatedAt = DateTime.Now,
            CommentId = Guid.NewGuid(),
            AuthorName = User.Identity!.Name!,
            ParentCommentId = dto.ParentCommentId is null ? null : Guid.Parse(dto.ParentCommentId),
        };

        await _commentRepository.AddNewEntityAsync(comment);
        return RedirectToAction("Index", new { id = comment.GameId });
    }

    private async Task<GamePageDto> CreateDto(Guid id)
    {
        var game = (await _gamesRepository.GetEntityByIdAsync(id));
        List<Comment> allComments = await FillDtoWithComments(game);

        await FillDtoWithScreenshots(game);
        await FillDtoWithTags(game);
        await FillDtoWithPrices(game);

        var examplePageDto = new GamePageDto
        {
            Game = game,
            GameId = game.GameId.ToString(),
        };

        await GetRepliesForCollection(game.Comments, allComments, examplePageDto);
        return examplePageDto;
    }

    private async Task FillDtoWithPrices(Game game)
    {
        var allPrices = await _priceRepository.GetAllEntitiesAsync();
        var prices = allPrices
            .Where(x => x.GameId == game.GameId)
            .ToList();

        game.Prices = prices;
    }

    private async Task FillDtoWithTags(Game game)
    {
        var allTags = await _tagsRepository.GetAllEntitiesAsync();
        var tags = allTags
            .Where(x => x.GameId == game.GameId)
            .Select(gameTag => gameTag.Tag)
            .ToList();

        game.GameTags = tags;
    }
    
    public async Task<RedirectToActionResult> RedirectByTag(string tagName)
    {
        var allTags = await _tagsRepository.GetAllEntitiesAsync();
        
        var tag = allTags.First(gameTag => gameTag.Tag.TagName.ToLower().Equals(tagName.ToLower()));
    
        return RedirectToAction("Index", "GameCatalog", new { tagId = tag.TagId });
    }
    
    public async Task<RedirectToActionResult> RedirectByPublisher(string publisherName)
    {
        var allPublishers = await _publisherRepository.GetAllEntitiesAsync();
    
        var publisher = allPublishers.First(p => p.PublisherName.ToLower().Equals(publisherName.ToLower()));
    
        return RedirectToAction("Index", "GameCatalog", new { publisherId = publisher.PublisherId });
    }
    
    public async Task<RedirectToActionResult> RedirectByDeveloper(string developerName)
    {
        var allDevs = await _developerRepository.GetAllEntitiesAsync();
    
        var developer = allDevs.First(d => d.DeveloperName.ToLower().Equals(developerName.ToLower()));
    
        return RedirectToAction("Index", "GameCatalog", new { developerId = developer.DeveloperId });
    }

    private async Task FillDtoWithScreenshots(Game game)
    {
        var allScreenshots = await _screenshotRepository.GetAllEntitiesAsync();
        var screenshots = allScreenshots.Where(x => x.GameId == game.GameId).ToList();
        game.Screenshots = screenshots;
    }

    private async Task<List<Comment>> FillDtoWithComments(Game game)
    {
        List<Comment> allComments = await _commentRepository.GetAllEntitiesAsync();
        game.Comments = allComments
            .Where(x => x.GameId == game.GameId && x.ParentCommentId is null)
            .ToList();

        return allComments;
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
}