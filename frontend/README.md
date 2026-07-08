# Livro de Casa — Frontend

Interface em **React + TypeScript** (Vite) para o desafio de controle de gastos residenciais.

## Identidade visual

A UI segue uma estética de **livro-razão contábil** ("Livro de Casa"): papel esverdeado com
grade de pontos discreta, tipografia serifada (Fraunces) nos títulos, números sempre em
monoespaçada (IBM Plex Mono) alinhados à direita como um razão de verdade, e um "selo" de saldo
geral no topo com o clássico traço duplo de fechamento de conta. Receitas em verde-pinho,
despesas em terracota-ferrugem.

## Estrutura

```
src/
├── api/client.ts          -> Cliente HTTP central (fetch), com tratamento do erro padronizado da API
├── types/index.ts          -> Tipos espelhando os DTOs do backend .NET
├── components/
│   ├── SaldoSelo.tsx         -> Elemento de assinatura visual (saldo geral)
│   ├── ErrorBanner.tsx        -> Faixa de erro/aviso
│   ├── PessoasView.tsx         -> Aba de cadastro/listagem/remoção de pessoas
│   ├── TransacoesView.tsx       -> Aba de lançamento/listagem de transações
│   └── TotaisView.tsx            -> Aba de totais por pessoa + fechamento geral
├── utils/formatters.ts     -> Formatação de moeda (pt-BR)
├── App.tsx                 -> Orquestra estado, abas e chamadas à API
└── main.tsx                 -> Bootstrap do React
```

`App.tsx` mantém o estado de pessoas/transações/totais e repassa dados + callbacks para as views —
sem gerenciador de estado externo, já que o escopo do desafio não justifica essa complexidade.

## Regras de negócio refletidas na UI

- Ao selecionar, no formulário de transação, uma pessoa menor de 18 anos, o campo "Tipo" é travado
  em "Despesa" (a API também valida isso — a UI só evita o erro antes de enviar).
- Erros de negócio devolvidos pela API (ex.: tentar cadastrar receita para menor de idade) aparecem
  na faixa de aviso no topo, com a mensagem original do backend.
- Remover uma pessoa pede confirmação, avisando que as transações dela também serão apagadas
  (reflete o `ON DELETE CASCADE` do backend).

## Como rodar

### 1. Configurar a URL da API

```bash
cp .env.example .env
```

Por padrão aponta para `http://localhost:5080/api` (onde a API .NET sobe localmente).

### 2. Instalar dependências

```bash
npm install
```

### 3. Rodar em desenvolvimento

```bash
npm run dev
```

Abre em `http://localhost:5173`. Certifique-se de que o backend (`dotnet run`) e o Postgres
(`docker compose up -d`, na raiz do repositório) já estejam rodando — o CORS da API já libera
essa origem.

### 4. Build de produção

```bash
npm run build
npm run preview
```

## Scripts

| Script            | Descrição                                  |
|-------------------|----------------------------------------------|
| `npm run dev`     | Sobe o servidor de desenvolvimento (Vite)      |
| `npm run build`   | Type-check (`tsc -b`) + build de produção       |
| `npm run preview` | Serve o build de produção localmente             |
| `npm run lint`    | Roda apenas o type-check, sem build               |
