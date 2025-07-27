using Infrastructure.Enums;

namespace Infrastructure.Interfaces;

public interface IPlayerService
{
    public IAsyncEnumerable<string> LoopUpdateTrackName(string trackNameLink, CancellationToken _trackPlayingToken);
    public TrackState PlayTrack(string playLink);
    public TrackState StopTrack();
    public void ChangeVolume(float volume);
}
