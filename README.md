# Carro Bem Guardado – Sistema de Controle de Estacionamento

Aplicação web desenvolvida em C# com ASP.NET Core MVC para gerenciamento de estacionamentos. O sistema permite registrar entradas e saídas de veículos, consultar movimentações e administrar tabelas de preços com controle de vigência.

## Tecnologias utilizadas

- C# / ASP.NET Core MVC (.NET 10)
- Entity Framework Core 10
- SQL Server 2022
- Docker e Docker Compose
- xUnit e Moq para testes automatizados
- HTML, CSS e JavaScript

## Funcionalidades

- Registro de entrada e saída de veículos.
- Consulta de veículos atualmente estacionados.
- Busca de veículos pela placa.
- Histórico de movimentações.
- Cadastro e gerenciamento de tabelas de preços.
- Controle de vigência das tarifas.
- Cálculo automático do valor da permanência.
- Aplicação das regras de tolerância e cobrança.

## Regras de cobrança

O sistema utiliza a data de entrada do veículo como referência para identificar a tabela de preços vigente.

As regras incluem:

- Até 30 minutos de permanência: cobrança de metade do valor da hora inicial.
- Valor da hora inicial configurável.
- Valor das horas adicionais configurável.
- Tolerância de 10 minutos para as horas adicionais.

Exemplo considerando hora inicial de R$ 2,00 e hora adicional de R$ 1,00:

| Permanência | Valor |
|---|---:|
| 30 minutos | R$ 1,00 |
| 1 hora | R$ 2,00 |
| 1h10 | R$ 2,00 |
| 1h15 | R$ 3,00 |
| 2h05 | R$ 3,00 |
| 2h15 | R$ 4,00 |

## Estrutura do projeto

```text
ControleEstacionamento-Benner/
│
└── ControleEstacionamentoBenner/
    │
    ├── ControleEstacionamento/
    │   ├── Controllers/
    │   ├── DAOs/
    │   ├── Data/
    │   ├── Interfaces/
    │   ├── Models/
    │   ├── Properties/
    │   ├── Services/
    │   ├── Views/
    │   ├── wwwroot/
    │   ├── .gitignore
    │   ├── appsettings.Development.json
    │   ├── appsettings.json
    │   ├── ControleEstacionamento.csproj
    │   └── Program.cs
    │
    ├── ControleEstacionamento.Tests/
    │   ├── Models/
    │   ├── Services/
    │   └── ControleEstacionamento.Tests.csproj
    │
    ├── .env.example
    ├── .gitignore
    ├── ControleEstacionamento.slnx
    ├── docker-compose.yml
    └── README.md
```

### Organização das pastas

**ControleEstacionamento:** contém a aplicação principal.

- **Controllers:** recebem as requisições da aplicação.
- **DAOs:** realizam as operações de consulta e persistência.
- **Data:** contém a configuração do Entity Framework Core.
- **Interfaces:** definem os contratos utilizados no acesso aos dados.
- **Models:** representam as entidades e os dados utilizados pelo sistema.
- **Properties:** contém configurações de inicialização.
- **Services:** concentram as regras de negócio.
- **Views:** contêm as telas da aplicação.
- **wwwroot:** armazena os arquivos estáticos, como CSS, JavaScript e imagens.

**ControleEstacionamento.Tests:** contém os testes automatizados, organizados em Models e Services.

---

# Como executar o projeto

As instruções abaixo consideram um computador Windows utilizando Visual Studio Community e Docker Desktop.

## 1. Pré-requisitos

Antes de começar, instale:

**Visual Studio Community**

https://visualstudio.microsoft.com/pt-br/vs/community/

Durante a instalação, selecione a carga de trabalho:

- ASP.NET e desenvolvimento Web.

**.NET 10 SDK**

https://dotnet.microsoft.com/pt-br/download/dotnet/10.0

**Docker Desktop**

https://www.docker.com/products/docker-desktop/

Após a instalação, abra o Docker Desktop e aguarde sua inicialização.

## 2. Baixar o projeto

Acesse o repositório:

https://github.com/LucasC250/ControleEstacionamento-Benner-

Clique em **Code → Download ZIP** e extraia os arquivos.

Entre na pasta `ControleEstacionamentoBenner`.

Essa pasta contém os arquivos necessários para configurar e executar o projeto.

## 3. Configurar as variáveis de ambiente

Localize o arquivo `.env.example`.

Faça uma cópia e renomeie para `.env`.

Abra o arquivo `.env` e configure uma senha forte para o SQL Server:

```env
MSSQL_SA_PASSWORD=SuaSenhaForte123!
```

A senha configurada será utilizada também na conexão da aplicação.

**Importante:** não publique o arquivo `.env` no GitHub. Apenas o `.env.example` deve permanecer no repositório.

## 4. Iniciar o banco de dados

Abra o Docker Desktop e aguarde até que esteja em execução.

Abra um terminal na pasta `ControleEstacionamentoBenner`, onde está localizado o arquivo `docker-compose.yml`.

Execute:

```bash
docker compose up -d
```

Para verificar se o container está em execução:

```bash
docker compose ps
```

Aguarde a inicialização completa do SQL Server antes de continuar.

## 5. Configurar a conexão com o banco de dados

Abra a solução pelo Visual Studio Community utilizando o arquivo:

`ControleEstacionamento.slnx`

No Gerenciador de Soluções:

1. Localize o projeto `ControleEstacionamento`.
2. Clique com o botão direito sobre ele.
3. Selecione **Gerenciar Segredos do Usuário**.
4. Abra o arquivo `secrets.json`.

Configure a conexão:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=EstacionamentoDb;User Id=sa;Password=SuaSenhaForte123!;TrustServerCertificate=True;Encrypt=True"
  }
}
```

Substitua `SuaSenhaForte123!` pela mesma senha configurada no arquivo `.env`.

Salve o arquivo.

Essa configuração fica armazenada localmente e não é enviada ao GitHub.

## 6. Executar a aplicação

No Visual Studio Community:

1. Clique com o botão direito no projeto `ControleEstacionamento`.
2. Selecione **Definir como Projeto de Inicialização**.
3. Na barra superior, selecione o perfil `https`.
4. Pressione `F5` ou clique no botão verde de iniciar.

O navegador será aberto automaticamente com a aplicação.

Na primeira execução, a aplicação cria a estrutura inicial do banco de dados.

## 7. Executar os testes automatizados

O projeto possui testes automatizados utilizando xUnit e Moq.

Para executar pelo Visual Studio Community:

1. Acesse o menu **Teste**.
2. Clique em **Executar Todos os Testes**.

Também é possível executar pelo terminal.

Na pasta `ControleEstacionamentoBenner`, onde está localizado o arquivo `ControleEstacionamento.slnx`, execute:

```bash
dotnet restore
dotnet build
dotnet test
```

Os comandos restauram as dependências, compilam a solução e executam os testes automatizados.

## 8. Encerrar o ambiente

Para parar o container do SQL Server sem apagar os dados:

```bash
docker compose down
```

Para iniciar novamente:

```bash
docker compose up -d
```

Os dados são armazenados em um volume Docker persistente.

---

## Observações

- O SQL Server precisa estar em execução para utilizar a aplicação.
- A conexão utiliza a porta 1433.
- O arquivo `.env` deve ser criado localmente a partir do `.env.example`.
- Os User Secrets armazenam as configurações locais de conexão.
- O banco de dados é inicializado pela aplicação na primeira execução.
- Não compartilhe senhas ou credenciais no repositório.

## Autor

Desenvolvido por Lucas Cardoso de Souza.

Projeto desenvolvido como exercício prático de desenvolvimento de software utilizando C#, ASP.NET Core MVC e SQL Server.
