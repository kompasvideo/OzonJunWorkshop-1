namespace Workshop.Api.Responses.V2;

public record GetHistoryResponse(
    CargoResponse Cargo,
    double Price);