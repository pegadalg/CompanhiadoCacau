# CompanhiadoCacau

Este projeto é um sistema desenvolvido para gerenciar as vendas e cadastros da Companhia do Cacau. Há funcionalidades como cadastro de clientes e registro de pedidos. O projeto utiliza .NET com suporte a testes automatizados.

Estrutura do Projeto
CompanhiadoCacau: Contém o código principal do sistema.
CompanhiadoCacau.Test: Contém os testes automatizados para validar o funcionamento do sistema.

Funcionalidades do sistema:
Cadastro de Clientes: Possui um modelo Cliente com propriedades Nome, CPF, Data de Nascimento, Email, Telefone, Endereço e uma lista de Pedidos.

Validações: Implementa validações personalizadas para o CPF, Email, Data de Nascimento (não pode ser uma data futura) e Telefone.

Gestão de Pedidos: Cada cliente pode ter uma lista de pedidos associados, permitindo o gerenciamento completo das operações de compra.

Pré-requisitos
.NET 8
Git para controle de versão

Como Executar
Clone o repositório:

bash
Copiar código
git clone <URL_DO_REPOSITORIO>
Navegue até a pasta do projeto:

bash
Copiar código
cd CompanhiaDoCacau
Restaure os pacotes:

bash
Copiar código
dotnet restore
Execute o projeto:

bash
Copiar código
dotnet run --project CompanhiaDoCacau

Como associar o Banco de Dados:
- Vá no menu "exibir", no canto superior esquerdo da tela, 
- Selecione a opção "Pesquisador de Objetos do SQL Server"
- Expanda o servidor local
- Expanda o Banco de dados
- Clique com o botão direito do mouse na pasta do seu banco de dados e selecione a opção "Adicionar Novo Banco de Dados"
- Crie o Banco de dados com o nome: Cacau
- Clique com o botão direito do mouse no banco de dados que acabou de criar "CompanhiadoCacau" e selecione a opção "propriedades"
- Perceba que abrirá um novo menu em sua tela, procure por "Cadeia de Conexão" e copie.
- Vá até o arquivo "appsettings.json" e substitua o código contido após  "CompanhiadoCacauConnection":
- Agora é só abrir o menu "Ferramentas", selecione a opção "Gerenciador de pacotes do NuGet" e abra o "Console de gerenciador de projetos"
- Perceba que abrirá uma nova tela de console na parte inferior da tela, suba o seu banco de dados utilizando o comando "Update-Database"


Como Executar os Testes
Para executar os testes automatizados:

bash
Copiar código
dotnet test CompanhiaDoCacau.Test
Estrutura de Arquivos
.gitignore e .gitattributes: Configurações para ignorar arquivos desnecessários e padronizar atributos.
CompanhiaDoCacau.sln: Arquivo de solução do Visual Studio para abrir o projeto.
README.md: Este arquivo de documentação.
Contribuição
Faça um fork do projeto.

Crie uma nova branch com sua feature:

bash
Copiar código
git checkout -b feature/nova-feature
Faça commit das suas alterações:

bash
Copiar código
git commit -m "Adiciona nova feature"
Envie para a branch principal:

bash
Copiar código
git push origin feature/nova-feature
Abra um Pull Request.
