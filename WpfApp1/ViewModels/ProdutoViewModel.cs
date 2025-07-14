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
    public class ProdutoViewModel : INotifyPropertyChanged
    {
        private readonly PersistenceService _persistenceService;
        private static readonly string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
        private readonly string _filePath = Path.Combine(projectRoot, "Data", "produtos.json");
        private readonly ObservableCollection<Produto> _todosOsProdutos;
        public ObservableCollection<Produto> ProdutosFiltrados { get; set; }

        #region Propriedades do Formulário e Seleção
        private string _nomeForm;
        public string NomeForm
        {
            get => _nomeForm;
            set { _nomeForm = value; OnPropertyChanged(nameof(NomeForm)); }
        }

        private string _codigoForm;
        public string CodigoForm
        {
            get => _codigoForm;
            set { _codigoForm = value; OnPropertyChanged(nameof(CodigoForm)); }
        }

        private decimal _valorForm;
        public decimal ValorForm
        {
            get => _valorForm;
            set { _valorForm = value; OnPropertyChanged(nameof(ValorForm)); }
        }

        private Produto _produtoSelecionado;
        public Produto ProdutoSelecionado
        {
            get => _produtoSelecionado;
            set
            {
                _produtoSelecionado = value;
                OnPropertyChanged(nameof(ProdutoSelecionado));
                CarregarProdutoSelecionado();
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

        private string _textoPesquisaCodigo;
        public string TextoPesquisaCodigo
        {
            get => _textoPesquisaCodigo;
            set { _textoPesquisaCodigo = value; OnPropertyChanged(nameof(TextoPesquisaCodigo)); }
        }

        // Para pesquisa por faixa de valor
        private decimal? _pesquisaValorInicial;
        public decimal? PesquisaValorInicial
        {
            get => _pesquisaValorInicial;
            set { _pesquisaValorInicial = value; OnPropertyChanged(nameof(PesquisaValorInicial)); }
        }

        private decimal? _pesquisaValorFinal;
        public decimal? PesquisaValorFinal
        {
            get => _pesquisaValorFinal;
            set { _pesquisaValorFinal = value; OnPropertyChanged(nameof(PesquisaValorFinal)); }
        }
        #endregion

        #region Comandos
        public ICommand IncluirCommand { get; private set; }
        public ICommand SalvarCommand { get; private set; }
        public ICommand ExcluirCommand { get; private set; }
        public ICommand PesquisarCommand { get; private set; }
        #endregion

        public ProdutoViewModel()
        {
            _persistenceService = new PersistenceService();
            var produtosSalvos = _persistenceService.Load<ObservableCollection<Produto>>(_filePath);
            _todosOsProdutos = produtosSalvos ?? new ObservableCollection<Produto>();
            ProdutosFiltrados = new ObservableCollection<Produto>(_todosOsProdutos);

            IncluirCommand = new RelayCommand(param => Incluir());
            SalvarCommand = new RelayCommand(param => Salvar());
            ExcluirCommand = new RelayCommand(param => Excluir(), param => ProdutoSelecionado != null);
            PesquisarCommand = new RelayCommand(param => Pesquisar());
        }

        #region Métodos de Lógica
        private void Pesquisar()
        {
            IEnumerable<Produto> resultado = _todosOsProdutos;

            if (!string.IsNullOrWhiteSpace(TextoPesquisaNome))
            {
                resultado = resultado.Where(p => p.Nome != null && p.Nome.ToLower().Contains(TextoPesquisaNome.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(TextoPesquisaCodigo))
            {
                resultado = resultado.Where(p => p.Codigo != null && p.Codigo.ToLower().Contains(TextoPesquisaCodigo.ToLower()));
            }

            if (PesquisaValorInicial.HasValue)
            {
                resultado = resultado.Where(p => p.Valor >= PesquisaValorInicial.Value);
            }

            if (PesquisaValorFinal.HasValue)
            {
                resultado = resultado.Where(p => p.Valor <= PesquisaValorFinal.Value);
            }

            ProdutosFiltrados.Clear();
            foreach (var produto in resultado)
            {
                ProdutosFiltrados.Add(produto);
            }
        }

        private void Incluir()
        {
            ProdutoSelecionado = null;
            LimparFormulario();
        }

        private void Salvar()
        {
            if (string.IsNullOrWhiteSpace(NomeForm) || string.IsNullOrWhiteSpace(CodigoForm)) return;

            if (ProdutoSelecionado == null) // Inclusão
            {
                int novoId = _todosOsProdutos.Any() ? _todosOsProdutos.Max(p => p.Id) + 1 : 1;
                var novoProduto = new Produto
                {
                    Id = novoId,
                    Nome = this.NomeForm,
                    Codigo = this.CodigoForm,
                    Valor = this.ValorForm
                };
                _todosOsProdutos.Add(novoProduto);
            }
            else // Edição
            {
                ProdutoSelecionado.Nome = this.NomeForm;
                ProdutoSelecionado.Codigo = this.CodigoForm;
                ProdutoSelecionado.Valor = this.ValorForm;

                int index = _todosOsProdutos.IndexOf(ProdutoSelecionado);
                if (index != -1) _todosOsProdutos[index] = ProdutoSelecionado;
            }

            _persistenceService.Save(_todosOsProdutos, _filePath);
            Pesquisar();
            LimparFormulario();
        }

        private void Excluir()
        {
            if (ProdutoSelecionado != null)
            {
                _todosOsProdutos.Remove(ProdutoSelecionado);
                _persistenceService.Save(_todosOsProdutos, _filePath);
                Pesquisar();
                LimparFormulario();
            }
        }
        #endregion

        #region Métodos Auxiliares
        private void LimparFormulario()
        {
            NomeForm = string.Empty;
            CodigoForm = string.Empty;
            ValorForm = 0;
        }

        private void CarregarProdutoSelecionado()
        {
            if (ProdutoSelecionado != null)
            {
                NomeForm = ProdutoSelecionado.Nome;
                CodigoForm = ProdutoSelecionado.Codigo;
                ValorForm = ProdutoSelecionado.Valor;
            }
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