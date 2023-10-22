namespace Workshop.Api.Responses.V1;

public record GetHistoryResponse(
    CargoResponse Cargo,
    double Price);