using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Moviewer.Models;
using Moviewer.Services;
using Nito.AsyncEx;

namespace Moviewer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private MovieService _movieService;
        public MainWindowViewModel()
        {
            InitializationNotifier = NotifyTaskCompletion.Create(InitializeAsync());
        }

        public INotifyTaskCompletion InitializationNotifier { get; private set; }

        private async Task InitializeAsync()
        {
            _movieService = new MovieService();
            var data = await _movieService.GetMovies(1);
            await Task.Delay(1000);
            MyItems = new ObservableCollection<Movie>(data);
        }

        private ObservableCollection<Movie> _myItems;
        public ObservableCollection<Movie> MyItems
        {
            get => _myItems;
            set
            {
                if (value != null)
                {
                    _myItems = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}