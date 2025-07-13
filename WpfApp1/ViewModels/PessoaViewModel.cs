using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp1.Models;
using WpfApp1.Utils;

namespace WpfApp1.ViewModels
{
    public class PessoaViewModel : INotifyPropertyChanged
    {
        #region Propriedades

        //Para avisar a View sobre mudanças de dados
        public ObservableCollection<Pessoa> Pessoas { get; set; }

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

        // Guardar o item selecionado na DataGrid
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

        #region Comandos (Ações dos Botões)

        public ICommand IncluirCommand { get; private set; }
        public ICommand SalvarCommand { get; private set; }
        public ICommand ExcluirCommand { get; private set; }

        #endregion

        public PessoaViewModel()
        {
            Pessoas = new ObservableCollection<Pessoa>();

            // Instanciando os comandos
            IncluirCommand = new RelayCommand(param => Incluir());
            SalvarCommand = new RelayCommand(param => Salvar());
            ExcluirCommand = new RelayCommand(param => Excluir(), param => PessoaSelecionada != null); 

            // --- PONTO IMPORTANTE ---
            // No futuro, aqui você chamará o seu "Service" para carregar os dados do arquivo JSON/XML.
            // Por enquanto, vamos adicionar dados de teste para ver a tela funcionando.
            CarregarDadosDeTeste();
        }

        #region Métodos (Lógica dos Comandos)

        private void Incluir()
        {
            // Limpa o formulário e deseleciona qualquer item na grid
            PessoaSelecionada = null; 
            LimparFormulario();
        }

        private void Salvar()
        {
            // Validação de CPF
            if (string.IsNullOrWhiteSpace(CpfForm))
            {
                return;
            }

            // Se ninguém está selecionado, é uma Inclusão
            if (PessoaSelecionada == null) 
            {
                int novoId = Pessoas.Any() ? Pessoas.Max(p => p.Id) + 1 : 1;

                var novaPessoa = new Pessoa
                {
                    Id = novoId,
                    Nome = this.NomeForm,
                    CPF = this.CpfForm,
                    Endereco = this.EnderecoForm
                };
                Pessoas.Add(novaPessoa);
            }
            // Se alguém está selecionado, é uma Edição
            else
            {
                PessoaSelecionada.Nome = this.NomeForm;
                PessoaSelecionada.CPF = this.CpfForm;
                PessoaSelecionada.Endereco = this.EnderecoForm;

                Pessoas[Pessoas.IndexOf(PessoaSelecionada)] = PessoaSelecionada;
            }

            // Futuramente, aqui você chamaria o seu "Service" para salvar a lista completa no arquivo.

            LimparFormulario();
        }

        private void Excluir()
        {
            if (PessoaSelecionada != null)
            {
                Pessoas.Remove(PessoaSelecionada);
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

        private void CarregarDadosDeTeste()
        {
            Pessoas.Add(new Pessoa { Id = 1, Nome = "João da Silva", CPF = "111.222.333-44", Endereco = "Rua A, 123" });
            Pessoas.Add(new Pessoa { Id = 2, Nome = "Maria Oliveira", CPF = "555.666.777-88", Endereco = "Av. B, 456" });
        }

        #endregion

        #region Implementação do INotifyPropertyChanged

        // Para notificar a View
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
