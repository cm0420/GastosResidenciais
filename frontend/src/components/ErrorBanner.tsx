interface ErrorBannerProps {
  message: string | null
  onDismiss: () => void
}

/** Exibe o erro de negócio/validação vindo da API em uma faixa dispensável. */
export function ErrorBanner({ message, onDismiss }: ErrorBannerProps) {
  if (!message) return null

  return (
    <div className="banner" role="alert">
      <span>{message}</span>
      <button type="button" onClick={onDismiss} aria-label="Dispensar aviso">
        ×
      </button>
    </div>
  )
}
