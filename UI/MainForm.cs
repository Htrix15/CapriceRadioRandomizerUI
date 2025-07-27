using Infrastructure.Enums;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace UI;

public partial class MainForm : Form
{
    private readonly IPlayerService playerService;
    private readonly IGenreLibraryService genreLibraryService;
    private readonly IRandomGenreService randomGenreService;
    private TrackState trackState = TrackState.Absent;
    private CancellationTokenSource trackPlayingToken;
    private List<Genre> perantGenres = [];
    private Genre currentPerantGenre;
    private Genre currentSubGenre;
    private RandomeMode randomeMode;
    private bool initSkip = true; //Fix start init autoset genre

    public MainForm(IPlayerService playerService,
        IGenreLibraryService genreLibraryService,
        IRandomGenreService randomGenreService)
    {
        this.playerService = playerService;
        this.genreLibraryService = genreLibraryService;
        this.randomGenreService = randomGenreService;
        InitializeComponent();
    }

    private async void MainForm_Load(object sender, EventArgs e)
    {
        await InitComboBoxGenres();
        InitComboBoxRandomMode();
    }

    private async Task InitComboBoxGenres()
    {
        perantGenres = await genreLibraryService.GetGenres();
        comboBoxGenres.DataSource = perantGenres;
        comboBoxGenres.DisplayMember = nameof(Genre.Name);
    }

    private void InitComboBoxRandomMode()
    {
        comboBoxRandomMode.DataSource = Enum.GetValues<RandomeMode>();
    }

    private async Task StartTrack()
    {
        trackState = playerService.PlayTrack(currentSubGenre.RemoteSources!.PlayLink);
        trackPlayingToken = new CancellationTokenSource();
        await LoopUpdateTrackName(trackPlayingToken.Token);
    }

    private async Task StopTrack()
    {
        trackPlayingToken?.Cancel();
        trackState = playerService.StopTrack();
        await Task.Delay(1000);// Fix not play
    }

    private bool ItIsNotPlayState() => trackState == TrackState.Absent || trackState == TrackState.Stopped;

    private async void buttonStartStop_Click(object sender, EventArgs e)
    {
        if (ItIsNotPlayState())
        {
            await StartTrack();
        }
        else
        {
            await StopTrack();
        }
    }

    private void trackBarVolume_Scroll(object sender, EventArgs e)
    {
        var volume = trackBarVolume.Value / 100f;
        playerService.ChangeVolume(volume);
    }

    private async Task LoopUpdateTrackName(CancellationToken token)
    {
        try
        {
            var oldName = labelTrackName.Text;
            await foreach (var trackName in playerService.LoopUpdateTrackName(currentSubGenre.RemoteSources!.TrackInfoBaseLink, token))
            {
                if (!string.Equals(oldName, trackName))
                {
                    await genreLibraryService.IncreaseGenreTrackCount(currentSubGenre.Key);
                    buttonLike.Enabled = true;
                    buttonDislike.Enabled = true;
                    labelTrackName.Text = trackName;
                    oldName = trackName;
                }   
            }
        }
        catch (OperationCanceledException)
        {
            labelTrackName.Text = "";
        }
    }

    private void RenameFormTextByGenres()
    {
        Text = $"Player - {currentPerantGenre.Name} - {currentSubGenre.Name}";
    }

    private async void comboBoxGenres_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (initSkip)
        {
            initSkip = false;
            return;
        }

        currentPerantGenre = (Genre)comboBoxGenres.SelectedItem!;
        currentSubGenre = randomGenreService.GetRandomGenre(currentPerantGenre.SubGenres!);
        RenameFormTextByGenres();

        if (!ItIsNotPlayState())
        {
            await StopTrack();
        }
        await StartTrack();
    }

    private void comboBoxRandomMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        randomeMode = (RandomeMode)comboBoxRandomMode.SelectedItem!;
    }

    private void ForceChangePerantGenre()
    {
        perantGenres = [.. perantGenres.Where(pg => pg.Key != currentPerantGenre.Key)];
        comboBoxGenres.DataSource = perantGenres;
        randomeMode = randomeMode switch
        {
            RandomeMode.SubGenre => RandomeMode.PerantAndSubGenre,
            RandomeMode.SubGenreWithRatingRange => RandomeMode.PerantAndSubGenreWithRatingRange,
            _ => randomeMode
        };
    }

    private async Task Randomize()
    {
        await StopTrack();    

        (currentPerantGenre, currentSubGenre) = randomGenreService.GetRandomGenre(randomeMode, currentPerantGenre, perantGenres);
        comboBoxGenres.SelectedItem = currentPerantGenre;
        RenameFormTextByGenres();

        await StartTrack();
    }

    private async void buttonRandom_Click(object sender, EventArgs e)
    {     
        await genreLibraryService.SkipGenre(currentPerantGenre, currentSubGenre);

        if (currentPerantGenre.IsSkip)
        {
            ForceChangePerantGenre();
        }

        await Randomize(); 
    }

    private async void buttonLike_Click(object sender, EventArgs e)
    {
        buttonLike.Enabled = false;
        await genreLibraryService.ChangeRating(currentSubGenre.Key, 1);
    }

    private async void buttonDislike_Click(object sender, EventArgs e)
    {
        buttonDislike.Enabled = false;
        await genreLibraryService.ChangeRating(currentSubGenre.Key, -1);
    }

    private async Task RemoveCurrentGenre()
    {
        currentPerantGenre.SubGenres = [.. currentPerantGenre.SubGenres!.Where(sg => sg.Key != currentSubGenre.Key)];

        if (currentPerantGenre.SubGenres.Count == 0)
        {
            await genreLibraryService.DisableGenre(currentPerantGenre.Key);
            ForceChangePerantGenre();
        }
        await Randomize();
    }

    private async void buttonDisableGenre_Click(object sender, EventArgs e)
    {
        await genreLibraryService.DisableGenre(currentSubGenre.Key);
        await RemoveCurrentGenre();
    }
}
