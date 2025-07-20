
namespace Infrastructure.Models;

public class Genre
{
    public required string Key {  get; set; }
    public string? Name { get; set; }
    public string? MainName { get; set; }
    public required bool IsAvailable { get; set; }
    public required bool IsDisabled { get; set; }
    public required bool IsSkip { get; set; }
    public required int TrackCount { get; set; }
    public required int RatingCount { get; set; }
    public required int Rating { get; set; }
    public RemoteSources? RemoteSources { get; set; }
}