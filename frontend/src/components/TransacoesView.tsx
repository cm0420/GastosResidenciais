import { FormEvent, useMemo, useState } from 'react'
import type { Pessoa, Transacao, TipoTransacao } from '../types'
import { formatarMoeda } from '../utils/formatters'

interface TransacoesViewProps {
  pessoas: Pessoa[]
  transacoes: Transacao[]
  carregando: boolean
  onCriar: (payload: {
    descricao: string
    valor: number
    tipo: TipoTransacao
    pessoaId: string
  }) => Promise<void>
}

/** Aba de gerenciamento de transações: formulário de lançamento + listagem. */
export function TransacoesView({ pessoas, transacoes, carregando, onCriar }: TransacoesViewProps) {
  const [descricao, setDescricao] = useState('')
  const [valor, setValor] = useState('')
  const [tipo, setTipo] = useState<TipoTransacao>('Despesa')
  const [pessoaId, setPessoaId] = useState('')
  const [enviando, setEnviando] = useState(false)

  const pessoaSelecionada = useMemo(
    () => pessoas.find((p) => p.id === pessoaId),
    [pessoas, pessoaId]
  )

  const somenteDespesaObrigatoria = Boolean(pessoaSelecionada && pessoaSelecionada.idade < 18)

  const handleSubmit = async (event: FormEvent) => {
    event.preventDefault()
    if (!descricao.trim() || valor === '' || !pessoaId) return

    setEnviando(true)
    try {
      await onCriar({
        descricao: descricao.trim(),
        valor: Number(valor),
        tipo,
        pessoaId
      })
      setDescricao('')
      setValor('')
    } finally {
      setEnviando(false)
    }
  }

  return (
    <div>
      <h2 className="section-title">Lançamentos</h2>
      <p className="section-subtitle">Registre receitas e despesas de cada morador.</p>

      <form className="form-grid" onSubmit={handleSubmit}>
        <div className="field">
          <label htmlFor="pessoa">Pessoa</label>
          <select
            id="pessoa"
            value={pessoaId}
            onChange={(e) => {
              setPessoaId(e.target.value)
              const p = pessoas.find((x) => x.id === e.target.value)
              if (p && p.idade < 18) setTipo('Despesa')
            }}
            required
          >
            <option value="" disabled>
              Selecione…
            </option>
            {pessoas.map((p) => (
              <option key={p.id} value={p.id}>
                {p.nome}
              </option>
            ))}
          </select>
        </div>

        <div className="field">
          <label htmlFor="descricao">Descrição</label>
          <input
            id="descricao"
            type="text"
            value={descricao}
            onChange={(e) => setDescricao(e.target.value)}
            placeholder="Ex.: Supermercado"
            required
          />
        </div>

        <div className="field">
          <label htmlFor="valor">Valor (R$)</label>
          <input
            id="valor"
            type="number"
            min={0.01}
            step="0.01"
            value={valor}
            onChange={(e) => setValor(e.target.value)}
            placeholder="0,00"
            required
          />
        </div>

        <div className="field">
          <label htmlFor="tipo">Tipo</label>
          <select
            id="tipo"
            value={tipo}
            onChange={(e) => setTipo(e.target.value as TipoTransacao)}
            disabled={somenteDespesaObrigatoria}
          >
            <option value="Despesa">Despesa</option>
            <option value="Receita">Receita</option>
          </select>
        </div>

        <div className="field">
          <button
            className="btn btn--selo"
            type="submit"
            disabled={enviando || pessoas.length === 0}
          >
            {enviando ? 'Lançando…' : 'Lançar transação'}
          </button>
        </div>
      </form>

      {somenteDespesaObrigatoria && (
        <p className="hint" style={{ marginTop: -8 }}>
          {pessoaSelecionada?.nome} é menor de idade — apenas despesas podem ser lançadas.
        </p>
      )}

      {pessoas.length === 0 && (
        <p className="hint">Cadastre uma pessoa na aba "Pessoas" antes de lançar transações.</p>
      )}

      {carregando ? (
        <p className="hint">Carregando transações…</p>
      ) : transacoes.length === 0 ? (
        <div className="empty-state">Nenhuma transação lançada ainda.</div>
      ) : (
        <table className="ledger">
          <thead>
            <tr>
              <th>Descrição</th>
              <th>Pessoa</th>
              <th>Tipo</th>
              <th style={{ textAlign: 'right' }}>Valor</th>
            </tr>
          </thead>
          <tbody>
            {transacoes.map((t) => (
              <tr key={t.id}>
                <td>{t.descricao}</td>
                <td>{t.pessoaNome ?? '—'}</td>
                <td>
                  <span className={`tag ${t.tipo === 'Receita' ? 'tag--receita' : 'tag--despesa'}`}>
                    {t.tipo === 'Receita' ? 'Receita' : 'Despesa'}
                  </span>
                </td>
                <td className={`col-valor ${t.tipo === 'Receita' ? 'valor-receita' : 'valor-despesa'}`}>
                  {t.tipo === 'Receita' ? '+' : '−'} {formatarMoeda(t.valor)}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  )
}
