namespace Infrastructure.Interfaces;

public interface IPlayer
{
    public Task PlayTrack(string playLink);
    public Task StopTrack();
    public Task ChangeVolume(int volume);
}
