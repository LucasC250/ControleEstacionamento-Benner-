# Carro Bem Guardado – Sistema de Controle de Estacionamento

Aplicação web em ASP.NET Core MVC para registrar entradas e saídas de veículos, consultar o histórico e gerenciar tabelas de preços com período de vigência.

## Tecnologias

- C# / ASP.NET Core MVC (.NET 10)
- Entity Framework Core 10
- SQL Server 2022 (Docker)
- xUnit e Moq para testes automatizados


### Estrutura principal

```text
ControleEstacionamento/
├── Controllers/       # Ações HTTP e integração com as views
├── Data/              # DbContext e configuração do modelo
├── DAOs/              # Consultas e persistência com Entity Framework Core
├── Interfaces/        # Contrato da persistência
├── Models/            # Entidades e records de entrada/saída
├── Services/          # Regras de negócio, um serviço por responsabilidade
│   └── Interfaces/    # Contratos dos serviços
└── Views/             # Interface MVC
ControleEstacionamento.Tests/
├── Models/            # Testes de validação dos records
└── Services/          # Testes unitários dos serviços
```


## Executar o SQL Server com Docker

1. Instale o Docker Desktop e inicie-o.
2. Na pasta raiz do projeto, copie `.env.example` para `.env` e altere `MSSQL_SA_PASSWORD` para uma senha forte.
3. Suba o banco:

   ```bash
   docker compose up -d
   ```

4. Confira se o container está em execução:

   ```bash
   docker compose ps
   ```

O banco será criado automaticamente pela aplicação na primeira inicialização, por meio do `EnsureCreated()` do Entity Framework Core. O volume `sqlserver_data` mantém os dados quando o container é reiniciado.

## Configurar a conexão da aplicação

Na pasta do projeto web (`ControleEstacionamento`), configure a connection string usando User Secrets. Use a mesma senha definida no arquivo `.env`:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=EstacionamentoDb;User Id=sa;Password=Troque_Esta_Senha_123!;TrustServerCertificate=True;Encrypt=True"
```

Substitua `Troque_Esta_Senha_123!` pela senha que você colocou no `.env`. A connection string não fica versionada no repositório.

Depois, ainda na pasta `ControleEstacionamento`, execute:

```bash
dotnet restore
dotnet run
```

A aplicação exige a connection string `ConnectionStrings:DefaultConnection`. Ela não inicia silenciosamente usando outro banco se essa configuração estiver ausente.

## Executar os testes

Na raiz da solução:

```bash
dotnet test
```

Os testes cobrem as regras de entrada, saída, cadastro/consulta de tarifas, consultas, validação dos records e cálculo de valores (incluindo os exemplos e limites de tolerância do enunciado). Os serviços são testados com `Moq`, isolando-os do SQL Server.
