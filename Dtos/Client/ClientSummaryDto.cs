namespace restapi_crud_practice.Dtos.Client;

public record class ClientSummaryDto
(
    int Id,
    string Username,
    string Role,
    DateTime PasswordChangedAt
);

