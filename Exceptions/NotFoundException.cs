namespace GastosResidenciais.Api.Exceptions;

/// <summary>Lançada quando um recurso solicitado não é encontrado (mapeada para HTTP 404).</summary>
public class NotFoundException(string message) : Exception(message);
