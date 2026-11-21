# Carro Bem Guardado – Sistema de Controle de Estacionamento

Sistema web desenvolvido em C# com ASP.NET Core MVC para controle de estacionamento. Permite registrar a entrada e saída de veículos, consultar o histórico e gerenciar tabelas de preços com períodos de vigência.

## Tecnologias utilizadas

- C# / ASP.NET Core MVC (.NET 10)
- Entity Framework Core 10
- SQL Server 2022
- Docker e Docker Compose
- xUnit
- Moq

## Estrutura do projeto

```
ControleEstacionamento/
├── Controllers/       # Controllers e ações da aplicação
├── Data/              # Configuração do banco de dados
├── DAOs/              # Consultas e persistência de dados
├── Interfaces/        # Interfaces de persistência
├── Models/            # Entidades e modelos
├── Services/          # Regras de negócio
│   └── Interfaces/    # Interfaces dos serviços
└── Views/             # Telas da aplicação

ControleEstacionamento.Tests/
├── Models/            # Testes dos modelos
└── Services/          # Testes dos serviços
```

## Requisitos

Para executar o projeto, é necessário ter instalado:

- [Git](https://git-scm.com/downloads)
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

## 1. Baixar o projeto

Clone o repositório:

```
git clone URL_DO_REPOSITORIO
```

Entre na pasta raiz do projeto, onde estão o arquivo `docker-compose.yml` e o arquivo `.env.example`.

Caso tenha baixado pelo GitHub em formato ZIP, basta extrair os arquivos e abrir o terminal nessa pasta.

## 2. Configurar o banco de dados

Abra o Docker Desktop e aguarde até que esteja iniciado.

Na pasta raiz do projeto, copie o arquivo `.env.example`:

```
Copy-Item .env.example .env
```

Abra o arquivo `.env` e configure a senha do SQL Server:

```
MSSQL_SA_PASSWORD=SuaSenhaForteAqui123!
```

Essa senha será utilizada também na configuração da aplicação.

Agora inicie o banco de dados:

```
docker compose up -d
```

Para verificar se o container está em execução:

```
docker compose ps
```

O SQL Server utiliza a porta `1433` e o volume `sqlserver_data` para armazenar os dados.

Aguarde o SQL Server finalizar a inicialização antes de continuar.

Se necessário, consulte os logs:

```
docker compose logs -f sqlserver
```

## 3. Configurar a conexão com o banco

Entre na pasta do projeto principal:

```
cd ControleEstacionamento
```

Inicialize o User Secrets:

```
dotnet user-secrets init
```

Configure a conexão com o SQL Server:

```
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=EstacionamentoDb;User Id=sa;Password=SuaSenhaForteAqui123!;TrustServerCertificate=True;Encrypt=True"
```

Substitua `SuaSenhaForteAqui123!` pela mesma senha definida no arquivo `.env`.

A connection string fica armazenada localmente por meio do User Secrets, sem precisar ser adicionada ao repositório.

## 4. Executar a aplicação

Ainda dentro da pasta `ControleEstacionamento`, execute:

```
dotnet restore
```

Depois:

```
dotnet run
```

Após iniciar, o terminal mostrará o endereço local da aplicação. Basta abrir esse endereço no navegador.

Na primeira execução, o Entity Framework Core utiliza `EnsureCreated()` para criar automaticamente o banco de dados e suas tabelas, caso ainda não existam.

Não é necessário criar o banco manualmente.

## 5. Executar os testes

Volte para a pasta raiz da solução:

```
cd ..
```

Execute os testes automatizados:

```
dotnet test
```

Os testes foram desenvolvidos com xUnit e Moq e verificam as validações dos modelos e as regras de negócio dos serviços, incluindo entradas, saídas, tarifas e cálculos de valores.

Os testes unitários utilizam dependências simuladas, não sendo necessário conectar ao SQL Server para executá-los.

## 6. Encerrar a aplicação

Para parar a aplicação, pressione `Ctrl + C` no terminal em que ela está sendo executada.

Para parar o container do SQL Server:

```
docker compose down
```

Os dados continuam salvos no volume Docker e estarão disponíveis quando o container for iniciado novamente.

Caso queira remover também os dados armazenados e começar com um banco vazio:

```
docker compose down -v
```

**Observações:**

- O Docker Desktop precisa estar aberto para utilizar o banco de dados.
- A porta `1433` precisa estar disponível.
- A senha do `.env` e da connection string deve ser a mesma.
- Não envie o arquivo `.env` com suas credenciais para o GitHub.
- Os dados são mantidos no volume `sqlserver_data` entre as inicializações do container.
