import { FormEvent, useState } from 'react'
import type { Pessoa } from '../types'

interface PessoasViewProps {
  pessoas: Pessoa[]
  carregando: boolean
  onCriar: (nome: string, idade: number) => Promise<void>
  onDeletar: (id: string) => Promise<void>
}

/** Aba de gerenciamento de pessoas: formulário de cadastro + listagem com remoção. */
export function PessoasView({ pessoas, carregando, onCriar, onDeletar }: PessoasViewProps) {
  const [nome, setNome] = useState('')
  const [idade, setIdade] = useState('')
  const [enviando, setEnviando] = useState(false)
  const [idRemovendo, setIdRemovendo] = useState<string | null>(null)

  const handleSubmit = async (event: FormEvent) => {
    event.preventDefault()
    if (!nome.trim() || idade === '') return

    setEnviando(true)
    try {
      await onCriar(nome.trim(), Number(idade))
      setNome('')
      setIdade('')
    } finally {
      setEnviando(false)
    }
  }

  const handleDeletar = async (pessoa: Pessoa) => {
    const confirmar = window.confirm(
      `Remover "${pessoa.nome}"? Todas as transações dessa pessoa também serão apagadas.`
    )
    if (!confirmar) return

    setIdRemovendo(pessoa.id)
    try {
      await onDeletar(pessoa.id)
    } finally {
      setIdRemovendo(null)
    }
  }

  return (
    <div>
      <h2 className="section-title">Moradores da casa</h2>
      <p className="section-subtitle">
        Cadastre cada pessoa antes de lançar suas transações. Menores de 18 anos só podem ter
        despesas registradas.
      </p>

      <form className="form-grid" onSubmit={handleSubmit}>
        <div className="field">
          <label htmlFor="nome">Nome</label>
          <input
            id="nome"
            type="text"
            value={nome}
            onChange={(e) => setNome(e.target.value)}
            placeholder="Ex.: Ana Beatriz"
            required
          />
        </div>
        <div className="field">
          <label htmlFor="idade">Idade</label>
          <input
            id="idade"
            type="number"
            min={0}
            max={150}
            value={idade}
            onChange={(e) => setIdade(e.target.value)}
            placeholder="Ex.: 34"
            required
          />
        </div>
        <div className="field">
          <button className="btn btn--selo" type="submit" disabled={enviando}>
            {enviando ? 'Cadastrando…' : 'Cadastrar pessoa'}
          </button>
        </div>
      </form>

      {carregando ? (
        <p className="hint">Carregando pessoas…</p>
      ) : pessoas.length === 0 ? (
        <div className="empty-state">Nenhuma pessoa cadastrada ainda.</div>
      ) : (
        <table className="ledger">
          <thead>
            <tr>
              <th>Nome</th>
              <th>Idade</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {pessoas.map((pessoa) => (
              <tr key={pessoa.id}>
                <td>{pessoa.nome}</td>
                <td>
                  {pessoa.idade}
                  {pessoa.idade < 18 && (
                    <span className="tag tag--despesa" style={{ marginLeft: 8 }}>
                      menor de idade
                    </span>
                  )}
                </td>
                <td style={{ textAlign: 'right' }}>
                  <button
                    className="btn btn--fantasma"
                    onClick={() => handleDeletar(pessoa)}
                    disabled={idRemovendo === pessoa.id}
                  >
                    {idRemovendo === pessoa.id ? 'Removendo…' : 'Remover'}
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  )
}
