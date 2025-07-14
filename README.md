
# Sistema de Gestão de Pessoas, Produtos e Pedidos

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)](https://github.com/gurufiredark/WpfApp)
[![.NET Framework](https://img.shields.io/badge/.NET-Framework%204.6-blue)](https://dotnet.microsoft.com/)
[![WPF](https://img.shields.io/badge/WPF-Windows%20Presentation%20Foundation-lightgrey)](https://learn.microsoft.com/dotnet/desktop/wpf/)
[![License](https://img.shields.io/github/license/gurufiredark/WpfApp)](https://github.com/gurufiredark/WpfApp/blob/main/LICENSE)
[![Made with C#](https://img.shields.io/badge/Made%20with-C%23-239120?logo=c-sharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/)
[![Last Commit](https://img.shields.io/github/last-commit/gurufiredark/WpfApp)](https://github.com/gurufiredark/WpfApp)

---

## 1. Objetivo do Projeto  
Desenvolver uma aplicação desktop utilizando **C# com WPF**, voltada para o **cadastro e gerenciamento de pessoas, produtos e pedidos**, com interface intuitiva, arquitetura desacoplada e persistência local em arquivos JSON.

---

## 2. Tecnologias Utilizadas

- **Linguagem:** C#  
- **Framework:** .NET Framework 4.6  
- **Interface Gráfica:** WPF (Windows Presentation Foundation)  
- **Arquitetura:** MVVM (Model-View-ViewModel)  
- **Manipulação de Dados:** LINQ (Language-Integrated Query)  
- **Persistência de Dados:** Arquivos JSON  

### Dependências Externas  
- **[Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/):** Biblioteca para serialização e desserialização de objetos em JSON.

---

## 3. Funcionalidades Principais

### Tela de Pessoas  
- Cadastro, edição, exclusão e listagem de pessoas  
- Pesquisa por nome e CPF  
- Validação de campos obrigatórios e verificação de duplicidade de CPF  
- Máscara de entrada para CPF  
- Exibição de pedidos vinculados ao cliente selecionado  
- Filtros de pedidos por status (Pago, Enviado, Recebido)  
- Alteração de status de pedidos  
- Criação de novos pedidos para a pessoa selecionada  

### Tela de Produtos  
- Cadastro, edição, exclusão e listagem de produtos  
- Pesquisa por nome, código e faixa de valores  

### Tela de Novo Pedido  
- Seleção de cliente entre os cadastrados  
- Adição de múltiplos produtos com definição de quantidade  
- Cálculo automático do valor total do pedido  
- Escolha da forma de pagamento  
- Salvamento do pedido com status **"Pendente"** e data atual  

---

## 4. Como Executar o Projeto

### Pré-requisitos
- **Visual Studio 2017 ou superior** (com o workload *".NET Desktop Development"*)  
- **.NET Framework 4.6** (geralmente já incluído com o Visual Studio)

### Passos

1. **Clone o repositório:**

   ```bash
   git clone https://github.com/gurufiredark/WpfApp
   ```

2. **Abra a solução:**

   - Navegue até a pasta do projeto e abra `WpfApp1.sln` com o Visual Studio.

3. **Restaure as dependências:**

   - O Visual Studio deve restaurar automaticamente o pacote `Newtonsoft.Json`.  
   - Caso contrário, clique com o botão direito sobre a solução e selecione **"Restaurar Pacotes NuGet"**.

4. **Execute a aplicação:**

   - Pressione `F5` ou clique em **"Iniciar"** para compilar e executar a aplicação.

---

## 5. Estrutura do Projeto

A organização segue o padrão **MVVM**, promovendo modularidade e separação de responsabilidades:

```
├── Models        # Classes de domínio (Pessoa, Produto, Pedido)
├── Views         # Interfaces gráficas XAML (PessoaView.xaml, ProdutoView.xaml, etc.)
├── ViewModels    # Lógica de apresentação (PessoaViewModel.cs, etc.)
├── Services      # Serviços auxiliares (ex: PersistenceService.cs)
├── Utils         # Utilitários gerais (ex: RelayCommand.cs)
├── Data          # Arquivos JSON persistentes (pessoas.json, produtos.json, pedidos.json)
```

---

## Licença

Distribuído sob a licença MIT. Veja o arquivo [LICENSE](https://github.com/gurufiredark/WpfApp/blob/main/LICENSE) para mais informações.
