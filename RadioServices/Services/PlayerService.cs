using Infrastructure.Enums;
using Infrastructure.Interfaces;
using System.Runtime.CompilerServices;

namespace RadioServices.Services;

public class PlayerService(IRemoteRepository remoteRepository,
    IUrlPlayer player) : IPlayerService
{
    private const int UpdateTrakeNameLoopTimeSec = 3000;

    public void ChangeVolume(float volume)
    {
        player.ChangeVolume(volume);
    }

    public async IAsyncEnumerable<string> LoopUpdateTrackName(string playLink, [EnumeratorCancellation] CancellationToken _trackPlayingToken)
    {
   
        while (!_trackPlayingToken.IsCancellationRequested)
        {
            var trackInfo = await remoteRepository.GetTrackInfo(playLink);
            yield return trackInfo.Name;
            await Task.Delay(UpdateTrakeNameLoopTimeSec, _trackPlayingToken);
        }
    }

    public TrackState PlayTrack(string playLink)
    {
        player.PlayTrack(playLink);
        return player.GetTrackState();
    }

    public TrackState StopTrack()
    {
        player.StopTrack();
        return player.GetTrackState();
    }
}
