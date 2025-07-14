Sistema de Gestão de Pessoas e Pedidos
1. Objetivo do Projeto
Desenvolver uma aplicação desktop em C# utilizando WPF para o cadastro e manipulação de dados de pessoas, produtos e pedidos.

2. Tecnologias e Dependências
Linguagem: C#

Framework: .NET Framework 4.6 

Interface Gráfica: WPF (Windows Presentation Foundation) 

Arquitetura: MVVM (Model-View-ViewModel) 

Manipulação de Dados: LINQ (Language-Integrated Query) 

Persistência de Dados: Arquivos JSON 

Dependências Externas:

Newtonsoft.Json: Biblioteca utilizada para serializar e desserializar os dados para o formato JSON.

3. Funcionalidades Implementadas
Tela de Pessoas
Cadastro, edição, exclusão e listagem de pessoas. 

Pesquisa de pessoas por Nome e CPF. 

Validação de campos obrigatórios (Nome, CPF) e verificação de CPF duplicado. 

Máscara de formatação para o campo de CPF.

Visualização da lista de pedidos associados ao cliente selecionado. 

Filtros para a lista de pedidos (pagos, entregues, pendentes). 

Ações para alterar o status de um pedido (Pago, Enviado, Recebido). 


Botão para iniciar um novo pedido vinculado ao cliente selecionado. 

Tela de Produtos
Cadastro, edição, exclusão e listagem de produtos. 

Pesquisa de produtos por Nome, Código e Faixa de Valor. 

Tela de Novo Pedido
Seleção de um cliente a partir da lista de pessoas cadastradas. 

Adição de múltiplos produtos ao pedido, com definição de quantidade. 

Cálculo automático do valor total do pedido. 

Seleção da forma de pagamento. 

Ao finalizar, o pedido é salvo com status "Pendente" e data atual. 


4. Como Executar o Projeto
Pré-requisitos
Visual Studio 2017 ou superior (com o workload ".NET Desktop Development" instalado).

.NET Framework 4.6 (geralmente já incluído no Visual Studio).

Passos para Execução
Clone o Repositório:

Bash

git clone https://github.com/gurufiredark/WpfApp
Abra a Solução:

Navegue até a pasta do projeto e abra o arquivo da solução (WpfApp1.sln) com o Visual Studio.

Restaure as Dependências:

O Visual Studio deve restaurar automaticamente o pacote Newtonsoft.Json do NuGet. Caso isso não ocorra, clique com o botão direito na solução no "Gerenciador de Soluções" e escolha "Restaurar Pacotes NuGet".

Execute o Projeto:

Pressione F5 ou clique no botão "Iniciar" na barra de ferramentas do Visual Studio para compilar e rodar a aplicação.

A aplicação iniciará na tela principal, com a navegação para as seções de Pessoas, Produtos e Novo Pedido.

5. Estrutura do Projeto
O projeto segue a estrutura de pastas sugerida, baseada no padrão MVVM: 


/Models: Contém as classes de domínio (Pessoa.cs, Produto.cs, Pedido.cs). 


/Views: Contém as telas da aplicação em formato XAML (PessoaView.xaml, ProdutoView.xaml, etc.). 


/ViewModels: Contém a lógica de apresentação e o estado das telas (PessoaViewModel.cs, etc.). 


/Services: Contém os serviços utilizados pela aplicação, como o PersistenceService.cs. 

/Utils: Contém classes de utilidade, como o RelayCommand.cs.


/Data: Contém os arquivos de dados pessoas.json, produtos.json e pedidos.json. 