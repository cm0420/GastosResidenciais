import { useCallback, useEffect, useState } from 'react'
import { api, ApiError } from './api/client'
import type { Pessoa, Transacao, TotaisGerais, TipoTransacao } from './types'
import { SaldoSelo } from './components/SaldoSelo'
import { ErrorBanner } from './components/ErrorBanner'
import { PessoasView } from './components/PessoasView'
import { TransacoesView } from './components/TransacoesView'
import { TotaisView } from './components/TotaisView'

type Aba = 'pessoas' | 'transacoes' | 'totais'

export default function App() {
  const [aba, setAba] = useState<Aba>('pessoas')

  const [pessoas, setPessoas] = useState<Pessoa[]>([])
  const [transacoes, setTransacoes] = useState<Transacao[]>([])
  const [totais, setTotais] = useState<TotaisGerais | null>(null)

  const [carregandoPessoas, setCarregandoPessoas] = useState(true)
  const [carregandoTransacoes, setCarregandoTransacoes] = useState(true)
  const [carregandoTotais, setCarregandoTotais] = useState(true)

  const [erro, setErro] = useState<string | null>(null)

  const tratarErro = (e: unknown) => {
    if (e instanceof ApiError) {
      setErro(e.message)
    } else {
      setErro('Não foi possível falar com a API. Verifique se o backend está rodando.')
    }
  }

  const carregarPessoas = useCallback(async () => {
    setCarregandoPessoas(true)
    try {
      setPessoas(await api.pessoas.listar())
    } catch (e) {
      tratarErro(e)
    } finally {
      setCarregandoPessoas(false)
    }
  }, [])

  const carregarTransacoes = useCallback(async () => {
    setCarregandoTransacoes(true)
    try {
      setTransacoes(await api.transacoes.listar())
    } catch (e) {
      tratarErro(e)
    } finally {
      setCarregandoTransacoes(false)
    }
  }, [])

  const carregarTotais = useCallback(async () => {
    setCarregandoTotais(true)
    try {
      setTotais(await api.relatorios.totais())
    } catch (e) {
      tratarErro(e)
    } finally {
      setCarregandoTotais(false)
    }
  }, [])

  useEffect(() => {
    carregarPessoas()
    carregarTransacoes()
    carregarTotais()
  }, [carregarPessoas, carregarTransacoes, carregarTotais])

  const handleCriarPessoa = async (nome: string, idade: number) => {
    try {
      await api.pessoas.criar({ nome, idade })
      setErro(null)
      await carregarPessoas()
      await carregarTotais()
    } catch (e) {
      tratarErro(e)
    }
  }

  const handleDeletarPessoa = async (id: string) => {
    try {
      await api.pessoas.deletar(id)
      setErro(null)
      await Promise.all([carregarPessoas(), carregarTransacoes(), carregarTotais()])
    } catch (e) {
      tratarErro(e)
    }
  }

  const handleCriarTransacao = async (payload: {
    descricao: string
    valor: number
    tipo: TipoTransacao
    pessoaId: string
  }) => {
    try {
      await api.transacoes.criar(payload)
      setErro(null)
      await carregarTransacoes()
      await carregarTotais()
    } catch (e) {
      tratarErro(e)
    }
  }

  return (
    <div className="app-shell">
      <header className="masthead">
        <span className="masthead__eyebrow">Controle de gastos residenciais</span>
        <h1 className="masthead__title">
          Livro de <em>Casa</em>
        </h1>
        {totais && (
          <SaldoSelo
            totalReceitas={totais.totalReceitasGeral}
            totalDespesas={totais.totalDespesasGeral}
            saldo={totais.saldoGeral}
          />
        )}
      </header>

      <ErrorBanner message={erro} onDismiss={() => setErro(null)} />

      <nav className="tabs">
        <button
          className={`tabs__item ${aba === 'pessoas' ? 'tabs__item--active' : ''}`}
          onClick={() => setAba('pessoas')}
        >
          Pessoas
        </button>
        <button
          className={`tabs__item ${aba === 'transacoes' ? 'tabs__item--active' : ''}`}
          onClick={() => setAba('transacoes')}
        >
          Transações
        </button>
        <button
          className={`tabs__item ${aba === 'totais' ? 'tabs__item--active' : ''}`}
          onClick={() => setAba('totais')}
        >
          Totais
        </button>
      </nav>

      <section className="tabs__panel">
        {aba === 'pessoas' && (
          <PessoasView
            pessoas={pessoas}
            carregando={carregandoPessoas}
            onCriar={handleCriarPessoa}
            onDeletar={handleDeletarPessoa}
          />
        )}
        {aba === 'transacoes' && (
          <TransacoesView
            pessoas={pessoas}
            transacoes={transacoes}
            carregando={carregandoTransacoes}
            onCriar={handleCriarTransacao}
          />
        )}
        {aba === 'totais' && <TotaisView totais={totais} carregando={carregandoTotais} />}
      </section>
    </div>
  )
}
