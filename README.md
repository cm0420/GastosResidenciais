# Gastos Residenciais - API

Backend em **.NET 8 / C#** para o desafio de controle de gastos residenciais, organizado em
**camadas** (Models, Dtos, Data, Repositories, Services, Mappers, Controllers) — sem DDD.

## Arquitetura

```
src/GastosResidenciais.Api
├── Controllers/      -> Endpoints HTTP (recebem request, chamam Service, devolvem DTO)
├── Services/          -> Regras de negócio (validações, orquestração)
│   └── Interfaces/
├── Repositories/       -> Acesso a dados via EF Core
│   └── Interfaces/
├── Mappers/            -> Conversão Entity <-> DTO
├── Dtos/                -> Contratos de entrada/saída da API
├── Models/              -> Entidades mapeadas para o banco
├── Enums/                -> TipoTransacao (Despesa/Receita)
├── Data/                  -> AppDbContext (Fluent API)
├── Middlewares/            -> Tratamento global de exceções
└── Exceptions/              -> Exceções de domínio (NotFoundException, BusinessRuleException)
```

Fluxo de dependência: `Controller -> Service (interface) -> Repository (interface) -> AppDbContext`.
Controllers e Services dependem de **interfaces**, não de implementações concretas — a injeção é
feita no `Program.cs`, mantendo baixo acoplamento e facilitando testes unitários (basta mockar as
interfaces).

## Regras de negócio implementadas

- Pessoa: criação, listagem e deleção (deleção remove as transações em cascata, via
  `OnDelete(DeleteBehavior.Cascade)` no `AppDbContext`).
- Transação: criação e listagem. Se a pessoa informada for menor de 18 anos, apenas
  transações do tipo `Despesa` são aceitas (`TransacaoService.CriarAsync`), lançando
  `BusinessRuleException` (HTTP 422) caso viole a regra.
- Relatório de totais: para cada pessoa, soma de receitas, despesas e saldo; ao final, o total
  geral de todas as pessoas.

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Docker + Docker Compose
- JetBrains Rider (ou qualquer IDE .NET)
- (Opcional) DBeaver para inspecionar o Postgres

## Como rodar

### 1. Subir o PostgreSQL via Docker

```bash
docker compose up -d
```

Isso sobe um Postgres 16 em `localhost:5432`, banco `gastos_residenciais`, usuário/senha
`postgres`/`postgres` (ajustável em `docker-compose.yml` e `appsettings.json`).

### 2. Restaurar pacotes e instalar a ferramenta do EF Core (uma vez só)

```bash
cd src/GastosResidenciais.Api
dotnet restore
dotnet tool install --global dotnet-ef
```

### 3. Criar a migration inicial e aplicar no banco

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

> A aplicação também roda `dbContext.Database.Migrate()` automaticamente no startup
> (`Program.cs`), então, se preferir, só rodar `dotnet run` já aplica migrations pendentes —
> mas é necessário que a migration inicial já exista no projeto (passo acima).

### 4. Rodar a API

```bash
dotnet run
```

A API sobe em `http://localhost:5080`, com Swagger disponível em `http://localhost:5080/swagger`.

### Abrindo no Rider

Basta abrir o arquivo `GastosResidenciais.sln` na raiz do repositório.

### Inspecionando o banco no DBeaver

Nova conexão PostgreSQL:
- Host: `localhost`
- Porta: `5432`
- Database: `gastos_residenciais`
- Usuário/senha: `postgres` / `postgres`

## Endpoints principais

| Método | Rota                     | Descrição                                   |
|--------|--------------------------|----------------------------------------------|
| GET    | `/api/pessoas`           | Lista todas as pessoas                        |
| POST   | `/api/pessoas`           | Cadastra uma pessoa                            |
| DELETE | `/api/pessoas/{id}`      | Remove uma pessoa (e suas transações)           |
| GET    | `/api/transacoes`        | Lista todas as transações                        |
| POST   | `/api/transacoes`        | Cadastra uma transação                            |
| GET    | `/api/relatorios/totais` | Totais de receitas/despesas/saldo por pessoa e geral |

## Front-end

O front-end em React + TypeScript está em `frontend/` (README próprio lá com instruções
detalhadas). Resumo rápido:

```bash
cd frontend
cp .env.example .env
npm install
npm run dev
```

Sobe em `http://localhost:5173` e já consome esta API (CORS liberado para
`localhost:3000` e `localhost:5173`).

## Próximos passos sugeridos (não obrigatórios)

- Testes unitários para os Services (xUnit + Moq), já que a injeção via interfaces facilita isso.
- Testes de componente no front-end (Vitest + Testing Library).
