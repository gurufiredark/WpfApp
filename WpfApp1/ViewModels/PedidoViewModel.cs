using System;
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
    public class PedidoViewModel : INotifyPropertyChanged
    {
        private readonly PersistenceService _persistenceService;
        private static readonly string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
        private readonly string _pedidosFilePath = Path.Combine(projectRoot, "Data", "pedidos.json");
        private readonly string _pessoasFilePath = Path.Combine(projectRoot, "Data", "pessoas.json");
        private readonly string _produtosFilePath = Path.Combine(projectRoot, "Data", "produtos.json");

        #region Listas de Dados
        public ObservableCollection<Pessoa> TodasAsPessoas { get; set; }
        public ObservableCollection<Produto> TodosOsProdutos { get; set; }
        public ObservableCollection<Pedido> TodosOsPedidos { get; set; }

        // Itens do pedido que está sendo criado
        public ObservableCollection<ItemPedido> ItensDoPedidoAtual { get; set; }
        #endregion

        #region Propriedades do Formulário
        private Pessoa _pessoaSelecionada;
        public Pessoa PessoaSelecionada
        {
            get => _pessoaSelecionada;
            set { _pessoaSelecionada = value; OnPropertyChanged(nameof(PessoaSelecionada)); }
        }

        private Produto _produtoParaAdicionar;
        public Produto ProdutoParaAdicionar
        {
            get => _produtoParaAdicionar;
            set { _produtoParaAdicionar = value; OnPropertyChanged(nameof(ProdutoParaAdicionar)); }
        }

        private int _quantidadeParaAdicionar = 1;
        public int QuantidadeParaAdicionar
        {
            get => _quantidadeParaAdicionar;
            set { _quantidadeParaAdicionar = value; OnPropertyChanged(nameof(QuantidadeParaAdicionar)); }
        }

        private FormaDePagamento _formaDePagamentoSelecionada;
        public FormaDePagamento FormaDePagamentoSelecionada
        {
            get => _formaDePagamentoSelecionada;
            set { _formaDePagamentoSelecionada = value; OnPropertyChanged(nameof(FormaDePagamentoSelecionada)); }
        }

        private decimal _valorTotalPedidoAtual;
        public decimal ValorTotalPedidoAtual
        {
            get => _valorTotalPedidoAtual;
            set { _valorTotalPedidoAtual = value; OnPropertyChanged(nameof(ValorTotalPedidoAtual)); }
        }
        #endregion

        #region Comandos
        public ICommand AdicionarProdutoCommand { get; private set; }
        public ICommand FinalizarPedidoCommand { get; private set; }
        public ICommand RemoverItemCommand { get; private set; }
        #endregion

        public PedidoViewModel()
        {
            _persistenceService = new PersistenceService();

            // Carrega todos os dados necessários
            TodasAsPessoas = _persistenceService.Load<ObservableCollection<Pessoa>>(_pessoasFilePath) ?? new ObservableCollection<Pessoa>();
            TodosOsProdutos = _persistenceService.Load<ObservableCollection<Produto>>(_produtosFilePath) ?? new ObservableCollection<Produto>();
            TodosOsPedidos = _persistenceService.Load<ObservableCollection<Pedido>>(_pedidosFilePath) ?? new ObservableCollection<Pedido>();

            ItensDoPedidoAtual = new ObservableCollection<ItemPedido>();

            // Inicializa os comandos
            AdicionarProdutoCommand = new RelayCommand(param => AdicionarProduto(), param => ProdutoParaAdicionar != null && QuantidadeParaAdicionar > 0);
            FinalizarPedidoCommand = new RelayCommand(param => FinalizarPedido(), param => PessoaSelecionada != null && ItensDoPedidoAtual.Any());
            RemoverItemCommand = new RelayCommand(RemoverItem);

            // Pega mudanças na lista de itens para recalcular o total
            ItensDoPedidoAtual.CollectionChanged += (s, e) => CalcularTotal();
        }

        public PedidoViewModel(Pessoa pessoa) : this()
        {
            if (pessoa != null)
            {
                // Encontra a pessoa na lista carregada e a define como selecionada
                PessoaSelecionada = TodasAsPessoas.FirstOrDefault(p => p.Id == pessoa.Id);
            }
        }

        #region Métodos de Lógica
        private void AdicionarProduto()
        {
            // Verificação se o produto já existe na lista para apenas aumenta a quantidade
            var itemExistente = ItensDoPedidoAtual.FirstOrDefault(i => i.Produto.Id == ProdutoParaAdicionar.Id);

            if (itemExistente != null)
            {
                itemExistente.Quantidade += QuantidadeParaAdicionar;
                OnPropertyChanged(nameof(ItensDoPedidoAtual));
            }
            else
            {
                var novoItem = new ItemPedido
                {
                    Produto = ProdutoParaAdicionar,
                    Quantidade = QuantidadeParaAdicionar
                };
                ItensDoPedidoAtual.Add(novoItem);
            }
        }

        private void RemoverItem(object item)
        {
            if (item is ItemPedido itemPedido)
            {
                ItensDoPedidoAtual.Remove(itemPedido);
            }
        }

        private void FinalizarPedido()
        {
            int novoId = TodosOsPedidos.Any() ? TodosOsPedidos.Max(p => p.Id) + 1 : 1;

            var novoPedido = new Pedido
            {
                Id = novoId,
                Pessoa = this.PessoaSelecionada,
                DataDaVenda = DateTime.Now,
                Status = StatusPedido.Pendente,
                FormaDePagamento = this.FormaDePagamentoSelecionada,
                Produtos = new System.Collections.Generic.List<ItemPedido>(this.ItensDoPedidoAtual)
            };

            TodosOsPedidos.Add(novoPedido);
            _persistenceService.Save(TodosOsPedidos, _pedidosFilePath);

            // Limpa o formulário para um novo pedido
            LimparFormulario();
        }

        private void CalcularTotal()
        {
            ValorTotalPedidoAtual = ItensDoPedidoAtual.Sum(item => item.Produto.Valor * item.Quantidade);
        }
        #endregion

        #region Métodos Auxiliares
        private void LimparFormulario()
        {
            PessoaSelecionada = null;
            ItensDoPedidoAtual.Clear();
            FormaDePagamentoSelecionada = default;
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