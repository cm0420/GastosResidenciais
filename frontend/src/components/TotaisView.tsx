import type { TotaisGerais } from '../types'
import { formatarMoeda } from '../utils/formatters'

interface TotaisViewProps {
  totais: TotaisGerais | null
  carregando: boolean
}

/** Aba de consulta de totais: receitas, despesas e saldo por pessoa, com fechamento geral. */
export function TotaisView({ totais, carregando }: TotaisViewProps) {
  return (
    <div>
      <h2 className="section-title">Fechamento do razão</h2>
      <p className="section-subtitle">Totais de receitas, despesas e saldo por pessoa.</p>

      {carregando || !totais ? (
        <p className="hint">Calculando totais…</p>
      ) : totais.pessoas.length === 0 ? (
        <div className="empty-state">Cadastre pessoas e transações para ver os totais aqui.</div>
      ) : (
        <table className="ledger">
          <thead>
            <tr>
              <th>Pessoa</th>
              <th style={{ textAlign: 'right' }}>Receitas</th>
              <th style={{ textAlign: 'right' }}>Despesas</th>
              <th style={{ textAlign: 'right' }}>Saldo</th>
            </tr>
          </thead>
          <tbody>
            {totais.pessoas.map((p) => (
              <tr key={p.pessoaId}>
                <td>{p.nome}</td>
                <td className="col-valor valor-receita">{formatarMoeda(p.totalReceitas)}</td>
                <td className="col-valor valor-despesa">{formatarMoeda(p.totalDespesas)}</td>
                <td className={`col-valor ${p.saldo < 0 ? 'valor-despesa' : 'valor-receita'}`}>
                  {formatarMoeda(p.saldo)}
                </td>
              </tr>
            ))}
          </tbody>
          <tfoot>
            <tr>
              <td>Total geral</td>
              <td className="col-valor valor-receita">{formatarMoeda(totais.totalReceitasGeral)}</td>
              <td className="col-valor valor-despesa">{formatarMoeda(totais.totalDespesasGeral)}</td>
              <td className={`col-valor ${totais.saldoGeral < 0 ? 'valor-despesa' : 'valor-receita'}`}>
                {formatarMoeda(totais.saldoGeral)}
              </td>
            </tr>
          </tfoot>
        </table>
      )}
    </div>
  )
}
