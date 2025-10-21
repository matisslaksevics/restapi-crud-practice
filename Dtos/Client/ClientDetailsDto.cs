namespace restapi_crud_practice.Dtos.Client;
public record class ClientDetailsDto
(
    Guid Id,
    string Username,
    string Role,
    DateTime PasswordChangedAt
);
