using System.ComponentModel;
using System.Windows.Input;
using WpfApp1.Models;
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
        public ICommand ShowPedidosViewCommand { get; private set; }

        public static MainViewModel Instance { get; private set; }

        public MainViewModel()
        {
            Instance = this;
            ShowPessoasViewCommand = new RelayCommand(param => ShowPessoasView());
            ShowProdutosViewCommand = new RelayCommand(param => ShowProdutosView());
            ShowPedidosViewCommand = new RelayCommand(param => ShowPedidosView());

            // Tela inicial é a de Pessoas
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
        private void ShowPedidosView()
        {
            CurrentView = new PedidoView();
        }

        public void ShowPedidoViewParaPessoa(Pessoa pessoa)
        {
            var pedidoViewModel = new PedidoViewModel(pessoa);

            var pedidoView = new PedidoView { DataContext = pedidoViewModel };

            CurrentView = pedidoView;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}