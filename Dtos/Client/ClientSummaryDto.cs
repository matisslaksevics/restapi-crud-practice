namespace restapi_crud_practice.Dtos.Client;

public record class ClientSummaryDto
(
    Guid Id,
    string Username,
    string Role,
    DateTime PasswordChangedAt
);

