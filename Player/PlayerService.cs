using Infrastructure.Interfaces;

namespace Players;

public class PlayerService : IPlayer
{
    public Task ChangeVolume(int volume)
    {
        throw new NotImplementedException();
    }

    public Task PlayTrack(string playLink)
    {
        throw new NotImplementedException();
    }

    public Task StopTrack()
    {
        throw new NotImplementedException();
    }
}
