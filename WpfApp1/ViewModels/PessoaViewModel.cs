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
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "pessoas.json");
        private readonly ObservableCollection<Pessoa> _todasAsPessoas;
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

        #region Comandos
        public ICommand IncluirCommand { get; private set; }
        public ICommand SalvarCommand { get; private set; }
        public ICommand ExcluirCommand { get; private set; }
        public ICommand PesquisarCommand { get; private set; }
        #endregion

        public PessoaViewModel()
        {
            _persistenceService = new PersistenceService();

            // Carrega os dados do arquivo
            var pessoasSalvas = _persistenceService.Load<ObservableCollection<Pessoa>>(_filePath);
            _todasAsPessoas = pessoasSalvas ?? new ObservableCollection<Pessoa>();

            PessoasFiltradas = new ObservableCollection<Pessoa>(_todasAsPessoas);

            IncluirCommand = new RelayCommand(param => Incluir());
            SalvarCommand = new RelayCommand(param => Salvar());
            ExcluirCommand = new RelayCommand(param => Excluir(), param => PessoaSelecionada != null);
            PesquisarCommand = new RelayCommand(param => Pesquisar());
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
                _todasAsPessoas.Add(novaPessoa); // Adiciona na lista principal
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
                _todasAsPessoas.Remove(PessoaSelecionada); // Remove da lista principal
                _persistenceService.Save(_todasAsPessoas, _filePath);

                Pesquisar(); // Atualiza a exibição
                LimparFormulario();
            }
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