using Infrastructure.Enums;
using Infrastructure.Interfaces;

namespace UI;

public partial class MainForm : Form
{
    private readonly IPlayerService _playerService;
    private readonly IGenreLibraryService _genreLibraryService;
    private TrackState _trackState = TrackState.Absent;
    private CancellationTokenSource _trackPlayingToken;

    public MainForm(IPlayerService playerService, IGenreLibraryService genreLibraryService)
    {
        _playerService = playerService;
        _genreLibraryService = genreLibraryService;
        InitializeComponent();

    }

    private async void buttonStartStop_Click(object sender, EventArgs e)
    {
        if (_trackState == TrackState.Absent || _trackState == TrackState.Stopped)
        {
            _trackState = _playerService.PlayTrack("http://79.111.14.76:8002/indianfolk");
            _trackPlayingToken = new CancellationTokenSource();
            await LoopUpdateTrackName(_trackPlayingToken.Token);
        }
        else
        {
            _trackPlayingToken?.Cancel();
            _trackState = _playerService.StopTrack();
        }
    }

    private void trackBarVolume_Scroll(object sender, EventArgs e)
    {
        var volume = trackBarVolume.Value / 100f;
        _playerService.ChangeVolume(volume);
    }

    private async Task LoopUpdateTrackName(CancellationToken token)
    {
        try
        {
            await foreach (var trackName in _playerService.LoopUpdateTrackName(token))
            {
                labelTrackName.Text = trackName;
            }
        }
        catch (OperationCanceledException)
        {
            labelTrackName.Text = "";
        }
    }

    private async void MainForm_Load(object sender, EventArgs e)
    {
        await _genreLibraryService.GetGenres();
    }
}
