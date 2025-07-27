using Infrastructure.Enums;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using System.Runtime.CompilerServices;

namespace RadioServices;

public class PlayerService(IRemoteRepository remoteRepository,
    IUrlPlayer player) : IPlayerService
{
    private const int UpdateTrakeNameLoopTimeSec = 3000;

    public void ChangeVolume(float volume)
    {
        player.ChangeVolume(volume);
    }

    public async IAsyncEnumerable<string> LoopUpdateTrackName([EnumeratorCancellation] CancellationToken _trackPlayingToken)
    {
   
        while (!_trackPlayingToken.IsCancellationRequested)
        {
            var trackInfo = await remoteRepository.GetTrackInfo("http://79.111.14.76:8000/status.xsl?mount=/indianfolk");//Tested url
            yield return trackInfo.Name;
            await Task.Delay(UpdateTrakeNameLoopTimeSec, _trackPlayingToken);
        }
    }

    public TrackState PlayTrack(string playLink)
    {
        player.PlayTrack("http://79.111.14.76:8002/indianfolk");//Tested url
        return player.GetTrackState();
    }

    public TrackState StopTrack()
    {
        player.StopTrack();
        return player.GetTrackState();
    }
}
