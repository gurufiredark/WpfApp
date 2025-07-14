using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using WpfApp1.Models;
using WpfApp1.Services;
using WpfApp1.Utils;

namespace WpfApp1.ViewModels
{
    public class PessoaViewModel : INotifyPropertyChanged
    {
        private readonly PersistenceService _persistenceService;
        private static readonly string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
        private readonly string _filePath = Path.Combine(projectRoot, "Data", "pessoas.json");
        private readonly string _pedidosFilePath = Path.Combine(projectRoot, "Data", "pedidos.json");
        private readonly ObservableCollection<Pessoa> _todasAsPessoas;
        private readonly ObservableCollection<Pedido> _todosOsPedidos;

        public ObservableCollection<Pedido> PedidosDaPessoaSelecionada { get; set; }
        public ObservableCollection<Pessoa> PessoasFiltradas { get; set; }

        #region Propriedades do Formulário e Seleção
        private string _nomeForm;
        public string NomeForm
        {
            get => _nomeForm;
            set { _nomeForm = value; OnPropertyChanged(nameof(NomeForm)); }
        }

        private string _cpfForm;
        public string CpfForm
        {
            get => _cpfForm;
            set { _cpfForm = value; OnPropertyChanged(nameof(CpfForm)); }
        }

        private string _enderecoForm;
        public string EnderecoForm
        {
            get => _enderecoForm;
            set { _enderecoForm = value; OnPropertyChanged(nameof(EnderecoForm)); }
        }

        private Pessoa _pessoaSelecionada;
        public Pessoa PessoaSelecionada
        {
            get => _pessoaSelecionada;
            set
            {
                _pessoaSelecionada = value;
                OnPropertyChanged(nameof(PessoaSelecionada));
                CarregarPessoaSelecionada();
                FiltrarPedidosDaPessoa();
            }
        }
        #endregion

        #region Propriedades de Pesquisa
        private string _textoPesquisaNome;
        public string TextoPesquisaNome
        {
            get => _textoPesquisaNome;
            set { _textoPesquisaNome = value; OnPropertyChanged(nameof(TextoPesquisaNome)); }
        }

        private string _textoPesquisaCpf;
        public string TextoPesquisaCpf
        {
            get => _textoPesquisaCpf;
            set { _textoPesquisaCpf = value; OnPropertyChanged(nameof(TextoPesquisaCpf)); }
        }
        #endregion

        #region Propriedades de Filtro de Pedidos
        // Propriedades para os checkboxes
        private bool _mostrarApenasPagos;
        public bool MostrarApenasPagos
        {
            get => _mostrarApenasPagos;
            set { _mostrarApenasPagos = value; OnPropertyChanged(nameof(MostrarApenasPagos)); FiltrarPedidosDaPessoa(); }
        }

        private bool _mostrarApenasEntregues;
        public bool MostrarApenasEntregues
        {
            get => _mostrarApenasEntregues;
            set { _mostrarApenasEntregues = value; OnPropertyChanged(nameof(MostrarApenasEntregues)); FiltrarPedidosDaPessoa(); }
        }

        private bool _mostrarApenasPendentes;
        public bool MostrarApenasPendentes
        {
            get => _mostrarApenasPendentes;
            set { _mostrarApenasPendentes = value; OnPropertyChanged(nameof(MostrarApenasPendentes)); FiltrarPedidosDaPessoa(); }
        }
        #endregion

        #region Comandos
        public ICommand IncluirCommand { get; private set; }
        public ICommand SalvarCommand { get; private set; }
        public ICommand ExcluirCommand { get; private set; }
        public ICommand PesquisarCommand { get; private set; }
        public ICommand MarcarComoPagoCommand { get; private set; }
        public ICommand MarcarComoEnviadoCommand { get; private set; }
        public ICommand MarcarComoRecebidoCommand { get; private set; }
        public ICommand IncluirPedidoCommand { get; private set; }
        #endregion

        public PessoaViewModel()
        {
            _persistenceService = new PersistenceService();

            var pessoasSalvas = _persistenceService.Load<ObservableCollection<Pessoa>>(_filePath);
            _todasAsPessoas = pessoasSalvas ?? new ObservableCollection<Pessoa>();
            PessoasFiltradas = new ObservableCollection<Pessoa>(_todasAsPessoas);

            _todosOsPedidos = _persistenceService.Load<ObservableCollection<Pedido>>(_pedidosFilePath) ?? new ObservableCollection<Pedido>();
            PedidosDaPessoaSelecionada = new ObservableCollection<Pedido>();

            IncluirCommand = new RelayCommand(param => Incluir());
            SalvarCommand = new RelayCommand(param => Salvar());
            ExcluirCommand = new RelayCommand(param => Excluir(), param => PessoaSelecionada != null);
            PesquisarCommand = new RelayCommand(param => Pesquisar());

            MarcarComoPagoCommand = new RelayCommand(MarcarComoPago);
            MarcarComoEnviadoCommand = new RelayCommand(MarcarComoEnviado);
            MarcarComoRecebidoCommand = new RelayCommand(MarcarComoRecebido);

            IncluirPedidoCommand = new RelayCommand(param => IncluirPedido(), param => PessoaSelecionada != null);
        }

        #region Métodos de Lógica
        private void Pesquisar()
        {
            IEnumerable<Pessoa> resultado = _todasAsPessoas;

            if (!string.IsNullOrWhiteSpace(TextoPesquisaNome))
            {
                resultado = resultado.Where(p => p.Nome != null && p.Nome.ToLower().Contains(TextoPesquisaNome.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(TextoPesquisaCpf))
            {
                resultado = resultado.Where(p => p.CPF != null && p.CPF.Replace(".", "").Replace("-", "").Contains(TextoPesquisaCpf.Trim()));
            }

            PessoasFiltradas.Clear();
            foreach (var pessoa in resultado)
            {
                PessoasFiltradas.Add(pessoa);
            }
        }

        private void Incluir()
        {
            PessoaSelecionada = null;
            LimparFormulario();
        }

        private void Salvar()
        {
            if (string.IsNullOrWhiteSpace(CpfForm) || string.IsNullOrWhiteSpace(NomeForm))
            {
                return;
            }

            if (PessoaSelecionada == null) // Inclusão
            {
                int novoId = _todasAsPessoas.Any() ? _todasAsPessoas.Max(p => p.Id) + 1 : 1;
                var novaPessoa = new Pessoa
                {
                    Id = novoId,
                    Nome = this.NomeForm,
                    CPF = this.CpfForm,
                    Endereco = this.EnderecoForm
                };
                _todasAsPessoas.Add(novaPessoa);
            }
            else // Edição
            {
                PessoaSelecionada.Nome = this.NomeForm;
                PessoaSelecionada.CPF = this.CpfForm;
                PessoaSelecionada.Endereco = this.EnderecoForm;

                int index = _todasAsPessoas.IndexOf(PessoaSelecionada);
                if (index != -1) _todasAsPessoas[index] = PessoaSelecionada;
            }

            _persistenceService.Save(_todasAsPessoas, _filePath);

            Pesquisar();
            LimparFormulario();
        }

        private void Excluir()
        {
            if (PessoaSelecionada != null)
            {
                _todasAsPessoas.Remove(PessoaSelecionada);
                _persistenceService.Save(_todasAsPessoas, _filePath);

                Pesquisar();
                LimparFormulario();
            }
        }

        private void FiltrarPedidosDaPessoa()
        {
            PedidosDaPessoaSelecionada.Clear();
            if (PessoaSelecionada != null)
            {
                // Todos os pedidos da pessoa
                IEnumerable<Pedido> pedidosFiltrados = _todosOsPedidos.Where(p => p.Pessoa.Id == PessoaSelecionada.Id);

                // Aplica os filtros com base nos checkboxes
                if (MostrarApenasPagos)
                {
                    pedidosFiltrados = pedidosFiltrados.Where(p => p.Status == StatusPedido.Pago);
                }
                if (MostrarApenasEntregues)
                {
                    // Um pedido entregue pode ter o status "Enviado" ou "Recebido"
                    pedidosFiltrados = pedidosFiltrados.Where(p => p.Status == StatusPedido.Enviado || p.Status == StatusPedido.Recebido);
                }
                if (MostrarApenasPendentes)
                {
                    pedidosFiltrados = pedidosFiltrados.Where(p => p.Status == StatusPedido.Pendente);
                }

                // Para adiciona o resultado filtrado na lista
                foreach (var pedido in pedidosFiltrados)
                {
                    PedidosDaPessoaSelecionada.Add(pedido);
                }
            }
        }

        private void MarcarComoPago(object pedido)
        {
            if (pedido is Pedido p)
            {
                p.Status = StatusPedido.Pago;
                SalvarPedidos();
            }
        }

        private void MarcarComoEnviado(object pedido)
        {
            if (pedido is Pedido p)
            {
                p.Status = StatusPedido.Enviado;
                SalvarPedidos();
            }
        }

        private void MarcarComoRecebido(object pedido)
        {
            if (pedido is Pedido p)
            {
                p.Status = StatusPedido.Recebido;
                SalvarPedidos();
            }
        }

        private void SalvarPedidos()
        {
            _persistenceService.Save(_todosOsPedidos, _pedidosFilePath);
            FiltrarPedidosDaPessoa();
        }
        #endregion

        #region Métodos Auxiliares
        private void LimparFormulario()
        {
            NomeForm = string.Empty;
            CpfForm = string.Empty;
            EnderecoForm = string.Empty;
        }

        private void CarregarPessoaSelecionada()
        {
            if (PessoaSelecionada != null)
            {
                NomeForm = PessoaSelecionada.Nome;
                CpfForm = PessoaSelecionada.CPF;
                EnderecoForm = PessoaSelecionada.Endereco;
            }
        }
        private void IncluirPedido()
        {
            // Pede para a MainViewModel mostrar a tela de pedido
            MainViewModel.Instance?.ShowPedidoViewParaPessoa(PessoaSelecionada);
        }
        #endregion

        #region Implementação do INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}