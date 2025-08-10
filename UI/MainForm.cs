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
    private List<Genre> parentGenres = [];
    private Genre currentParentGenre;
    private Genre currentSubGenre;
    private RandomeMode randomeMode;
    private bool isComboBoxLoaded = false;
    private bool formLoaded = false;

    private GenresViewerFormFactory genresViewerFormFactory;
    private GenresViewerForm genresViewerForm;
    
    public MainForm(IPlayerService playerService,
        IGenreLibraryService genreLibraryService,
        IRandomGenreService randomGenreService,
        GenresViewerFormFactory genresViewerFormFactory)
    {
        this.playerService = playerService;
        this.genreLibraryService = genreLibraryService;
        this.randomGenreService = randomGenreService;
        this.genresViewerFormFactory = genresViewerFormFactory;
        InitializeComponent();
    }

    private async void MainForm_Load(object sender, EventArgs e)
    {
        labelProgress.Text = "Loading...";
        Helpers.DisableAllControls(this);
        InitComboBoxRandomMode();
        await InitComboBoxGenres();
        Helpers.EnableAllControls(this);
        labelProgress.Text = "";
        formLoaded = true;
    }

    private async Task InitGenresViewerForm()
    {
        genresViewerForm = await genresViewerFormFactory.Create();
    }

    private bool InitByGenreLastChoice()
    {
        var parentWithLsat = parentGenres.FirstOrDefault(pg => pg.SubGenres!.Any(g => g.IsLastChoice));
        if (parentWithLsat != null)
        {
            currentParentGenre = parentWithLsat;
            currentSubGenre = parentWithLsat.SubGenres!.First(g => g.IsLastChoice);
            comboBoxGenres.SelectedItem = currentParentGenre;
            return true;
        }
        return false;
    }

    private async Task InitComboBoxGenres()
    {
        parentGenres = await genreLibraryService.GetOrCreateGenres();
        comboBoxGenres.DataSource = parentGenres;
        comboBoxGenres.DisplayMember = nameof(Genre.Name);

        var initedChoice = InitByGenreLastChoice();
        if (!initedChoice)
        {
            ForceChangeParentGenre();
            await Randomize();
        }
        isComboBoxLoaded = true;
        RenameFormTextByGenres();
    }

    private void InitComboBoxRandomMode()
    {
        var dandomeModeDictionary = new Dictionary<RandomeMode, string>
        {
            { RandomeMode.SubGenre, "Random Subgenre" },
            { RandomeMode.ParentAndSubGenre, "Random Parent genre and Subgenre" },
            { RandomeMode.SubGenreWithRatingRange, "Random Subgenre based on rating" },
            { RandomeMode.ParentAndSubGenreWithRatingRange, "Random Parent genre and Subgenre based on rating" }
        };

        comboBoxRandomMode.DataSource = new BindingSource(dandomeModeDictionary, null);
        comboBoxRandomMode.DisplayMember = "Value";
        comboBoxRandomMode.ValueMember = "Key";

    }

    private async Task StartTrack()
    {
        if (!formLoaded) return;
        trackState = playerService.PlayTrack(currentSubGenre.RemoteSources!.PlayLink);
        trackPlayingToken = new CancellationTokenSource();
        await LoopUpdateTrackName(trackPlayingToken.Token);
    }

    private async Task StopTrack()
    {
        if (!formLoaded) return;
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
          //  labelTrackName.Text = "";
        }
    }

    private void RenameFormTextByGenres()
    {
        Text = $"Player - {currentParentGenre.Name} - {currentSubGenre.Name}";
    }

    private async void comboBoxGenres_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!isComboBoxLoaded) return;

        currentParentGenre = (Genre)comboBoxGenres.SelectedItem!;
        currentSubGenre = randomGenreService.GetRandomGenre(currentParentGenre.SubGenres!);
        RenameFormTextByGenres();

        if (!ItIsNotPlayState())
        {
            await StopTrack();
        }
        await StartTrack();
    }

    private void comboBoxRandomMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        randomeMode = ((KeyValuePair<RandomeMode, string>)comboBoxRandomMode.SelectedItem!).Key;
    }

    private void ForceChangeParentGenre()
    {
        parentGenres = [.. parentGenres.Where(pg => pg.Key != currentParentGenre?.Key)];
        comboBoxGenres.DataSource = parentGenres;
        randomeMode = randomeMode switch
        {
            RandomeMode.SubGenre => RandomeMode.ParentAndSubGenre,
            RandomeMode.SubGenreWithRatingRange => RandomeMode.ParentAndSubGenreWithRatingRange,
            _ => randomeMode
        };
    }

    private async Task Randomize()
    {
        await StopTrack();
 
        (currentParentGenre, currentSubGenre) = randomGenreService.GetRandomGenre(randomeMode, currentParentGenre, parentGenres);
        comboBoxGenres.SelectedItem = currentParentGenre;
        RenameFormTextByGenres();

        await StartTrack();
    }

    private async void buttonRandom_Click(object sender, EventArgs e)
    {
        await genreLibraryService.SkipGenre(currentParentGenre, currentSubGenre);

        if (currentParentGenre.IsSkip)
        {
            ForceChangeParentGenre();
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
        currentParentGenre.SubGenres = [.. currentParentGenre.SubGenres!.Where(sg => sg.Key != currentSubGenre.Key)];

        if (currentParentGenre.SubGenres.Count == 0)
        {
            await genreLibraryService.ReActiveteSubGenres(currentParentGenre.Key);
            currentParentGenre.SubGenres.ForEach(s => s.IsDisabled = false);
        }
        await Randomize();
    }

    private async void buttonDisableGenre_Click(object sender, EventArgs e)
    {
        await genreLibraryService.DisableGenre(currentSubGenre.Key);
        await RemoveCurrentGenre();
    }

    private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        await genreLibraryService.ItIsLastChoice(currentSubGenre.Key);
    }

    private async void buttonEditGenres_Click(object sender, EventArgs e)
    {
        await InitGenresViewerForm();
        buttonEditGenres.Enabled = false;
        genresViewerForm.DataReturned += (data) => {
            buttonEditGenres.Enabled = true;
            if (!data.hasChanges) return;

            parentGenres = genreLibraryService.PackGenresIntoParentGenres(data.genres);
            currentParentGenre = parentGenres.FirstOrDefault(g => g.Key == currentParentGenre.Key);
            currentSubGenre = currentParentGenre.SubGenres.FirstOrDefault(g => g.Key == currentSubGenre.Key);

            isComboBoxLoaded = false;
            comboBoxGenres.DataSource = parentGenres;
            isComboBoxLoaded = true;
            RenameFormTextByGenres();
        };

        genresViewerForm.Show();
       
    }
}
