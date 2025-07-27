
namespace Infrastructure.Models;

public class Genre
{
    public required string Key {  get; set; }
    public required string Name { get; set; }
    public required bool ItIsParent { get; set; }

    public string? ParentGenreKey { get; set; }
    public Genre? ParentGenre { get; set; }

    public List<Genre>? SubGenres { get; set; }

    public required bool IsAvailable { get; set; }
    public required bool IsDisabled { get; set; }
    public required bool IsSkip { get; set; }
    public required int TrackCount { get; set; }
    public required int RatingCount { get; set; }
    public required int Rating { get; set; }

    public RemoteSources? RemoteSources { get; set; }

    public decimal CalculatedRating => ItIsParent
        ? SubGenres?.Sum(g => g.CalculatedRating) ?? 0
        : (Rating/ (decimal)RatingCount) * 100;
}