Sistema de Gest�o de Pessoas e Pedidos
1. Objetivo do Projeto
Desenvolver uma aplica��o desktop em C# utilizando WPF para o cadastro e manipula��o de dados de pessoas, produtos e pedidos.

2. Tecnologias e Depend�ncias
Linguagem: C#

Framework: .NET Framework 4.6 

Interface Gr�fica: WPF (Windows Presentation Foundation) 

Arquitetura: MVVM (Model-View-ViewModel) 

Manipula��o de Dados: LINQ (Language-Integrated Query) 

Persist�ncia de Dados: Arquivos JSON 

Depend�ncias Externas:

Newtonsoft.Json: Biblioteca utilizada para serializar e desserializar os dados para o formato JSON.

3. Funcionalidades Implementadas
Tela de Pessoas
Cadastro, edi��o, exclus�o e listagem de pessoas. 

Pesquisa de pessoas por Nome e CPF. 

Valida��o de campos obrigat�rios (Nome, CPF) e verifica��o de CPF duplicado. 

M�scara de formata��o para o campo de CPF.

Visualiza��o da lista de pedidos associados ao cliente selecionado. 

Filtros para a lista de pedidos (pagos, entregues, pendentes). 

A��es para alterar o status de um pedido (Pago, Enviado, Recebido). 


Bot�o para iniciar um novo pedido vinculado ao cliente selecionado. 

Tela de Produtos
Cadastro, edi��o, exclus�o e listagem de produtos. 

Pesquisa de produtos por Nome, C�digo e Faixa de Valor. 

Tela de Novo Pedido
Sele��o de um cliente a partir da lista de pessoas cadastradas. 

Adi��o de m�ltiplos produtos ao pedido, com defini��o de quantidade. 

C�lculo autom�tico do valor total do pedido. 

Sele��o da forma de pagamento. 

Ao finalizar, o pedido � salvo com status "Pendente" e data atual. 


4. Como Executar o Projeto
Pr�-requisitos
Visual Studio 2017 ou superior (com o workload ".NET Desktop Development" instalado).

.NET Framework 4.6 (geralmente j� inclu�do no Visual Studio).

Passos para Execu��o
Clone o Reposit�rio:

Bash

git clone https://github.com/gurufiredark/WpfApp
Abra a Solu��o:

Navegue at� a pasta do projeto e abra o arquivo da solu��o (WpfApp1.sln) com o Visual Studio.

Restaure as Depend�ncias:

O Visual Studio deve restaurar automaticamente o pacote Newtonsoft.Json do NuGet. Caso isso n�o ocorra, clique com o bot�o direito na solu��o no "Gerenciador de Solu��es" e escolha "Restaurar Pacotes NuGet".

Execute o Projeto:

Pressione F5 ou clique no bot�o "Iniciar" na barra de ferramentas do Visual Studio para compilar e rodar a aplica��o.

A aplica��o iniciar� na tela principal, com a navega��o para as se��es de Pessoas, Produtos e Novo Pedido.

5. Estrutura do Projeto
O projeto segue a estrutura de pastas sugerida, baseada no padr�o MVVM: 


/Models: Cont�m as classes de dom�nio (Pessoa.cs, Produto.cs, Pedido.cs). 


/Views: Cont�m as telas da aplica��o em formato XAML (PessoaView.xaml, ProdutoView.xaml, etc.). 


/ViewModels: Cont�m a l�gica de apresenta��o e o estado das telas (PessoaViewModel.cs, etc.). 


/Services: Cont�m os servi�os utilizados pela aplica��o, como o PersistenceService.cs. 

/Utils: Cont�m classes de utilidade, como o RelayCommand.cs.


/Data: Cont�m os arquivos de dados pessoas.json, produtos.json e pedidos.json. 