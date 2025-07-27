namespace Infrastructure.Models;

public class RemoteSources
{
    public required string Key { get; set; }
    public Genre Genre { get; set; }
    public required string PlayLink { get; set; }
    public required string TrackInfoBaseLink { get; set; }
}