using Infrastructure.Interfaces;

namespace UI
{
    public partial class MainForm : Form
    {
        private readonly IPlayer _player;
        private readonly IGenreRepository _genreRepository;
        private readonly IRemoteRepository _remoteRepository;

        public MainForm(IPlayer player, IGenreRepository genreRepository, IRemoteRepository remoteRepository)
        {
            _player = player;
            _genreRepository = genreRepository;
            _remoteRepository = remoteRepository;

            InitializeComponent();
        }
    }
}
