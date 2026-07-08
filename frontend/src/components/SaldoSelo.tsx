import { formatarMoeda } from '../utils/formatters'

interface SaldoSeloProps {
  totalReceitas: number
  totalDespesas: number
  saldo: number
}

/**
 * Elemento de assinatura da interface: um "fechamento de razão" no topo da página,
 * nos moldes de um livro-caixa contábil, com o saldo em destaque e o double-rule
 * clássico de fechamento de conta.
 */
export function SaldoSelo({ totalReceitas, totalDespesas, saldo }: SaldoSeloProps) {
  return (
    <div className="saldo-selo">
      <div className="saldo-selo__principal">
        <span className="saldo-selo__label">Saldo geral da casa</span>
        <span className={`saldo-selo__valor ${saldo < 0 ? 'negativo' : ''}`}>
          {formatarMoeda(saldo)}
        </span>
      </div>
      <div className="saldo-selo__breakdown">
        <div>
          <span>Receitas</span>
          <span className="valor-receita">{formatarMoeda(totalReceitas)}</span>
        </div>
        <div>
          <span>Despesas</span>
          <span className="valor-despesa">{formatarMoeda(totalDespesas)}</span>
        </div>
      </div>
    </div>
  )
}
