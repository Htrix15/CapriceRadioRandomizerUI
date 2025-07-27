using Infrastructure.Enums;
using Infrastructure.Interfaces;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Players;

public class UrlPlayer : IUrlPlayer
{
    private WaveOutEvent? _player;
    private MediaFoundationReader? _stream;
    private SampleChannel? _channel;
    private float _baseVolume = 0.5f;

    public void ChangeVolume(float volume)
    {
        if (_channel != null) { 
            _channel.Volume = volume;
        }
        else
        {
            _baseVolume = volume;
        }
    }

    public void PlayTrack(string playLink)
    {
        _player = new WaveOutEvent();
        _player.PlaybackStopped += (sender, args) =>
        {
            Dispose();
        };

        _stream = new MediaFoundationReader(playLink);
        _channel = new SampleChannel(_stream)
        {
            Volume = _baseVolume
        };

        _player!.Init(_channel);
        _player.Play();
    }

    public void StopTrack()
    {
        _player?.Stop();
    }

    public TrackState GetTrackState()
    {
        if (_player == null) return TrackState.Absent;
        return MapState(_player.PlaybackState);
    }

    private static TrackState MapState(PlaybackState state) => state switch
    {
        PlaybackState.Stopped => TrackState.Stopped,
        PlaybackState.Playing => TrackState.Playing,
        _ => throw new NotImplementedException(),
    };

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_player != null)
            {
                _player.Dispose();
                _player = null;
            }

            if (_stream != null)
            {
                _stream.Dispose();
                _stream = null;
            }

            if (_channel != null)
            {
                _channel = null;
            }
        }
    }
}
