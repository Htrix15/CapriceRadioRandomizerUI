using Infrastructure.Enums;
using Infrastructure.Interfaces;

namespace Players.Tests;

internal class PlayerTests
{
    private string _playLink;

    [SetUp]
    public void Setup()
    {
        _playLink = "http://79.111.14.76:8002/indianfolk";
    }

    [Test]
    public void GetTrackState_NotStartedPlay_StoppedState()
    {
        using IUrlPlayer player = new UrlPlayer();
        var state = player.GetTrackState();
        Assert.That(state, Is.EqualTo(TrackState.Absent));
    }

    [Test]
    public void GetTrackState_StartedPlay_PlayingState()
    {
        using IUrlPlayer player = new UrlPlayer();
        player.PlayTrack(_playLink);
        var state = player.GetTrackState();
        Assert.That(state, Is.EqualTo(TrackState.Playing));
    }
}
