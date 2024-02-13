# DesafioTunts.Rocks2024
Criar uma aplicação em uma linguagem de programação de sua preferência. A aplicação deve ser capaz de ler  uma planilha do google sheets, buscar as informações necessárias, calcular e escrever o  resultado na planilha.

## Sobre o projeto

Feito em ASP.NET MVC, um framework de desenvolvimento para construir aplicativos da web usando a plataforma .NET. No ASP.NET MVC, os aplicativos são construídos seguindo o padrão arquitetural MVC, que separa a aplicação em três componentes principais: Model, View e Controller.

### Model

O Model representa a estrutura de dados e a lógica de negócios da aplicação. Ele geralmente consiste em classes que representam objetos de domínio ou dados que serão manipulados pela aplicação. O Model é responsável pela interação com o banco de dados e pela aplicação de regras de negócio.

### View

A View é responsável pela apresentação dos dados ao usuário. Ela consiste em páginas HTML, que podem conter marcação HTML, CSS, JavaScript e código Razor (uma sintaxe que combina código C# com HTML). As Views são usadas para exibir os dados ao usuário de forma visualmente atraente e interativa.
### Controller

O Controller é responsável por gerenciar o fluxo de solicitações e respostas entre o Model e a View. Ele recebe solicitações HTTP dos usuários, chama os métodos apropriados no Model para processar essas solicitações e, em seguida, seleciona a View correta para exibir os resultados ao usuário. O Controller atua como o intermediário entre o Model e a View, coordenando a lógica de negócios e a apresentação de dados.

#### HomeController
Primeira a ser executada contendo a página inicial.
##### Index
Redireciona para a View Home.
##### Info
Redireciona para a View Info.

#### StudentGradesController
Responsável pela API do GoogleSheets.
##### Index
Redireciona para a View Home.
##### CalculateGrades
Redireciona para a action CalculateGrades.
##### FinalPassing
Chama a função responsável por calcular as notas dos alunos e redireciona para a página que exibe as tabelas.
##### SaveAvg 
Recebe a lista de médias chama a função responsavel por calcular a nota final e depois reássa a informação para a página que exibe as notas dos alunos

## Para executar um projeto ASP.NET MVC, siga estes passos:
Copie o URL do Repositório: clique no botão "Code". Isso abrirá um menu suspenso onde você pode copiar o URL do repositório. 
Clone o Repositório: Use o comando git clone seguido pelo URL do repositório que você copiou anteriormente.
Abra o projeto: Abra a solução do projeto no Visual Studio. 
Execute o Projeto: Pressione F5 ou clique em "Iniciar" na barra de ferramentas do Visual Studio para compilar e iniciar o projeto. O Visual Studio iniciará o servidor web embutido e abrirá o navegador padrão com a página inicial do seu projeto.


```sobre a execução do projeto
Caso o projeto não rode, vá na barra de ferramentas e selecione o IIS Express.
