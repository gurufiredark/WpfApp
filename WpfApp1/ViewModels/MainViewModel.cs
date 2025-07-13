using System.ComponentModel;
using System.Windows.Input;
using WpfApp1.Utils;
using WpfApp1.Views;

namespace WpfApp1.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public ICommand ShowPessoasViewCommand { get; private set; }
        public ICommand ShowProdutosViewCommand { get; private set; }

        public MainViewModel()
        {
            ShowPessoasViewCommand = new RelayCommand(param => ShowPessoasView());
            ShowProdutosViewCommand = new RelayCommand(param => ShowProdutosView());

            // Define a tela inicial que será mostrada
            CurrentView = new PessoaView();
        }

        private void ShowPessoasView()
        {
            CurrentView = new PessoaView();
        }

        private void ShowProdutosView()
        {
            CurrentView = new ProdutoView();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}