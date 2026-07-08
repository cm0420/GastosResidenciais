/**
 * Tipos espelhando os DTOs expostos pela API (GastosResidenciais.Api.Dtos).
 * O enum TipoTransacao é serializado como string pelo backend (HasConversion<string>),
 * por isso é representado aqui como union type de strings.
 */

export type TipoTransacao = 'Despesa' | 'Receita'

export interface Pessoa {
  id: string
  nome: string
  idade: number
}

export interface PessoaCreatePayload {
  nome: string
  idade: number
}

export interface Transacao {
  id: string
  descricao: string
  valor: number
  tipo: TipoTransacao
  pessoaId: string
  pessoaNome?: string | null
}

export interface TransacaoCreatePayload {
  descricao: string
  valor: number
  tipo: TipoTransacao
  pessoaId: string
}

export interface TotaisPessoa {
  pessoaId: string
  nome: string
  totalReceitas: number
  totalDespesas: number
  saldo: number
}

export interface TotaisGerais {
  pessoas: TotaisPessoa[]
  totalReceitasGeral: number
  totalDespesasGeral: number
  saldoGeral: number
}

/** Formato de erro devolvido pelo ExceptionHandlingMiddleware da API. */
export interface ApiErrorPayload {
  status: number
  erro: string
}
