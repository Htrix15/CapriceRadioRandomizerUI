using Infrastructure.Interfaces;
using Infrastructure.Models;
using RadioServices.Services;

namespace UI;

public partial class GenresViewerForm : Form
{
    private IGenreLibraryService genreLibraryService;
    private List<Genre> genres;

    private int indexItIsPerantColumn;
    private int indexParentGenreKeyColumn;
    private int indexParentGenreNameColumn;

    //public GenresViewerForm()
    //{
    //    InitializeComponent();
    //}

    public GenresViewerForm(IGenreLibraryService genreLibraryService, List<Genre> genres)
    {
        this.genreLibraryService = genreLibraryService;
        this.genres = genres;
        InitializeComponent();
        InitDataGridGenres();
    }

    private void InitDataGridGenres()
    {
        Helpers.DisableAllControls(this);
        dataGridViewGenres.AutoGenerateColumns = false;

        dataGridViewGenres.Columns.AddRange([
                new DataGridViewTextBoxColumn
                {
                    HeaderText = "Key",
                    Name = nameof(Genre.Key),
                    DataPropertyName = nameof(Genre.Key),
                    ReadOnly = true,
                    Visible = false,
                },
                new DataGridViewTextBoxColumn
                {
                    HeaderText = "Genre Name",
                    Name = nameof(Genre.Name),
                    DataPropertyName = nameof(Genre.Name),
                },
                new DataGridViewCheckBoxColumn
                {
                    HeaderText = "It is Parent Genre",
                    Name = nameof(Genre.ItIsParent),
                    DataPropertyName = nameof(Genre.ItIsParent),
                    ValueType = typeof(bool),
                    ReadOnly = true,
                    Visible = false,
                },
                new DataGridViewTextBoxColumn
                {
                    HeaderText = "ParentGenreKey",
                    Name = nameof(Genre.ParentGenreKey),
                    DataPropertyName = nameof(Genre.ParentGenreKey),
                    ReadOnly = true,
                    Visible = false,
                },
                new DataGridViewTextBoxColumn
                {
                    HeaderText = "Parent Genre Name",
                    Name = "Parent Genre Name",
                    ReadOnly = true,
                },
                new DataGridViewCheckBoxColumn
                {
                    HeaderText = "Exists on the site",
                    Name = nameof(Genre.IsAvailable),
                    DataPropertyName = nameof(Genre.IsAvailable),
                    ValueType = typeof(bool),
                    ReadOnly = true,
                },
                new DataGridViewCheckBoxColumn
                {
                    HeaderText = "Disabled for playback",
                    Name = nameof(Genre.IsDisabled),
                    DataPropertyName = nameof(Genre.IsDisabled),
                    ValueType = typeof(bool),
                },
                new DataGridViewCheckBoxColumn
                {
                    HeaderText = "Already played (exclude for Random)",
                    Name = nameof(Genre.IsSkip),
                    DataPropertyName = nameof(Genre.IsSkip),
                    ValueType = typeof(bool),
                },
                new DataGridViewCheckBoxColumn
                {
                    HeaderText = "Played before closing",
                    Name = nameof(Genre.IsLastChoice),
                    DataPropertyName = nameof(Genre.IsLastChoice),
                    ValueType = typeof(bool),
                    ReadOnly = true,
                },
                new DataGridViewTextBoxColumn
                {
                    HeaderText = "Number of tracks listened to",
                    Name = nameof(Genre.TrackCount),
                    DataPropertyName = nameof(Genre.TrackCount),
                    ReadOnly = true,
                    ValueType = typeof(int),
                },
                new DataGridViewTextBoxColumn
                {
                    HeaderText = "Number of ratings",
                    Name = nameof(Genre.RatingCount),
                    DataPropertyName = nameof(Genre.RatingCount),
                    ReadOnly = true,
                    ValueType = typeof(int),
                },
                new DataGridViewTextBoxColumn
                {
                    HeaderText = "Rating",
                    Name = nameof(Genre.Rating),
                    DataPropertyName = nameof(Genre.Rating),
                    ValueType = typeof(int),
                },
            ]);

        indexParentGenreKeyColumn = dataGridViewGenres.Columns[nameof(Genre.ParentGenreKey)]!.Index;
        indexItIsPerantColumn = dataGridViewGenres.Columns[nameof(Genre.ItIsParent)]!.Index;
        indexParentGenreNameColumn = dataGridViewGenres.Columns["Parent Genre Name"]!.Index;

        dataGridViewGenres.DataSource = genres;
        Helpers.EnableAllControls(this);
    }

    private void RenamePeranteGenres()
    {
        Helpers.DisableAllControls(this);
        for (int i = 0; i < dataGridViewGenres.RowCount; i++)
        {
            if ((bool)dataGridViewGenres[indexItIsPerantColumn, i].Value! == true) continue;

            var parentGenreKey = (string)dataGridViewGenres[indexParentGenreKeyColumn, i].Value!;
            dataGridViewGenres[indexParentGenreNameColumn, i].Value = genres.First(g => g.Key == parentGenreKey).Name;
        }
        Helpers.EnableAllControls(this);
    }

    private void dataGridViewGenres_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
        if (dataGridViewGenres.Columns[e.ColumnIndex].Name == nameof(Genre.Name))
        {
            RenamePeranteGenres();
        }
    }

    private void dataGridViewGenres_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
        RenamePeranteGenres();
    }

    private void CancelCellWithInvalidateName(DataGridViewCellValidatingEventArgs e)
    {
        if (dataGridViewGenres.Columns[e.ColumnIndex].Name != nameof(Genre.Name))
            return;

        var newName = e.FormattedValue?.ToString()?.Trim();

        if (string.IsNullOrEmpty(newName))
        {
            e.Cancel = true;
        }
    }

    private void CancelCellWithInvalidateRating(DataGridViewCellValidatingEventArgs e)
    {
        if (dataGridViewGenres.Columns[e.ColumnIndex].Name != nameof(Genre.Rating))
            return;

        var newRatingString = e.FormattedValue?.ToString()?.Trim();

        if (string.IsNullOrEmpty(newRatingString))
        {
            e.Cancel = true;
        }

        if (!int.TryParse(newRatingString, out var newRating))
        {
            e.Cancel = true;
        } 
    }

    private void dataGridViewGenres_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
        CancelCellWithInvalidateName(e);
        CancelCellWithInvalidateRating(e);
    }

    private async void buttonRescanGenres_Click(object sender, EventArgs e)
    {
    

    }

    private async void buttonSaveChanges_Click(object sender, EventArgs e)
    {
        Helpers.DisableAllControls(this);
        await genreLibraryService.UpdateGenres(genres, new Infrastructure.Options.UpdateGenreOptions()
        {
            UpdateName = true,
            UpdateIsDisabled = true,
            UpdateIsSkip = true,
            UpdateRating = true,
        });
        Helpers.EnableAllControls(this);
    }
}
