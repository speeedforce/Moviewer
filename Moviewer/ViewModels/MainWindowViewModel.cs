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

        private INotifyTaskCompletion _initializationNotifier;
        public INotifyTaskCompletion InitializationNotifier
        {
            get => _initializationNotifier;
            private set
            {
                _initializationNotifier = value;
                OnPropertyChanged();
            }
        }

        private async Task InitializeAsync()
        {
            Page = 1;
            _movieService = new MovieService();
            await LoadData(Page);
        }

        public async Task LoadData(int page)
        {
            var data = await _movieService.GetMovies(page);
            await Task.Delay(1000);
            MyItems = new ObservableCollection<Movie>(data);
        }

        public void NextPage()
        {
            if (_page+1 > 10)
                Page = 1;  
            else
                Page = _page + 1;
            
            InitializationNotifier = NotifyTaskCompletion.Create(LoadData(Page));
        }

        public void PrevPage()
        {
            if (1 > _page - 1)
                Page = 10;
            else
                Page = _page - 1;

            InitializationNotifier = NotifyTaskCompletion.Create(LoadData(Page));
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

        private int _page;
        public int Page
        {
            get => _page;
            set
            {
                _page = value;
                OnPropertyChanged();
            }
        }
    }
}
