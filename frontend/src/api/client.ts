import type {
  ApiErrorPayload,
  Pessoa,
  PessoaCreatePayload,
  Transacao,
  TransacaoCreatePayload,
  TotaisGerais
} from '../types'

const BASE_URL = import.meta.env.VITE_API_URL ?? 'http://localhost:5080/api'

/** Erro de aplicação com a mensagem já extraída do payload de erro da API. */
export class ApiError extends Error {
  status: number

  constructor(status: number, message: string) {
    super(message)
    this.status = status
    this.name = 'ApiError'
  }
}

async function request<T>(path: string, init?: RequestInit): Promise<T> {
  const response = await fetch(`${BASE_URL}${path}`, {
    headers: { 'Content-Type': 'application/json' },
    ...init
  })

  if (!response.ok) {
    let mensagem = `Erro ${response.status} ao comunicar com a API.`
    try {
      const payload = (await response.json()) as ApiErrorPayload
      mensagem = payload.erro ?? mensagem
    } catch {
      // corpo de erro não veio em JSON; mantém mensagem genérica
    }
    throw new ApiError(response.status, mensagem)
  }

  if (response.status === 204) {
    return undefined as T
  }

  return (await response.json()) as T
}

export const api = {
  pessoas: {
    listar: () => request<Pessoa[]>('/pessoas'),
    criar: (payload: PessoaCreatePayload) =>
      request<Pessoa>('/pessoas', { method: 'POST', body: JSON.stringify(payload) }),
    deletar: (id: string) => request<void>(`/pessoas/${id}`, { method: 'DELETE' })
  },
  transacoes: {
    listar: () => request<Transacao[]>('/transacoes'),
    criar: (payload: TransacaoCreatePayload) =>
      request<Transacao>('/transacoes', { method: 'POST', body: JSON.stringify(payload) })
  },
  relatorios: {
    totais: () => request<TotaisGerais>('/relatorios/totais')
  }
}
