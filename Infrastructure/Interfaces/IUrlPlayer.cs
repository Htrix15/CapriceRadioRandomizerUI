using Infrastructure.Enums;

namespace Infrastructure.Interfaces;

public interface IUrlPlayer : IDisposable
{
    public void PlayTrack(string playLink);
    public void StopTrack();
    public TrackState GetTrackState();
    public void ChangeVolume(float volume);
}
