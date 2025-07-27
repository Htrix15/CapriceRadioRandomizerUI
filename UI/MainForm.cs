using Infrastructure.Enums;
using Infrastructure.Interfaces;

namespace UI;

public partial class MainForm : Form
{
    private readonly IUrlPlayer _player;
    private readonly IGenreRepository _genreRepository;
    private readonly IRemoteRepository _remoteRepository;
    private TrackState _trackState = TrackState.Absent;
    private CancellationTokenSource _trackPlayingToken;
    private const int UpdateTrakeNameLoopTimeSec = 3000;

    public MainForm(IUrlPlayer player,
        IGenreRepository genreRepository,
        IRemoteRepository remoteRepository)
    {
        _player = player;
        _genreRepository = genreRepository;
        _remoteRepository = remoteRepository;
        InitializeComponent();
    }

    private async void buttonStartStop_Click(object sender, EventArgs e)
    {
        if (_trackState == TrackState.Absent || _trackState == TrackState.Stopped)
        {
            _player.PlayTrack("http://79.111.14.76:8002/indianfolk");//Tested url
            _trackState = _player.GetTrackState();
            _trackPlayingToken = new CancellationTokenSource();
            await LoopUpdateTrackName(_trackPlayingToken.Token);
        }
        else
        {
            _trackPlayingToken?.Cancel();
            _player.StopTrack();
            _trackState = TrackState.Stopped;
        }
    }

    private void trackBarVolume_Scroll(object sender, EventArgs e)
    {
        var volume = trackBarVolume.Value / 100f;
        _player.ChangeVolume(volume);
    }

    private async Task LoopUpdateTrackName(CancellationToken _trackPlayingToken)
    {
        try
        {
            while (!_trackPlayingToken.IsCancellationRequested)
            {
                var trackInfo = await _remoteRepository.GetTrackInfo("http://79.111.14.76:8000/status.xsl?mount=/indianfolk");//Tested url
                labelTrackName.Text = trackInfo.Name;
                await Task.Delay(UpdateTrakeNameLoopTimeSec, _trackPlayingToken);
            }
        }
        catch (OperationCanceledException) {
            labelTrackName.Text = "";
        }
    }
}
