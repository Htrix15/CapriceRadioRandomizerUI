
namespace Infrastructure.Models;

public class Genre
{
    public required string Key {  get; set; }
    public string? Name { get; set; }
    public string? MainName { get; set; }
    public required bool IsAvailable { get; set; }
    public bool? IsDisabled { get; set; }
    public bool? IsSkip { get; set; }
    public int TrackCount { get; set; }
    public int RatingCount { get; set; }
    public int Rating { get; set; }
    public RemoteSources? RemoteSources { get; set; }
}