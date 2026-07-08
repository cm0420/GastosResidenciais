namespace GastosResidenciais.Api.Exceptions;

/// <summary>Lançada quando uma regra de negócio é violada (mapeada para HTTP 422).</summary>
public class BusinessRuleException(string message) : Exception(message);
