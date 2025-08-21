namespace restapi_crud_practice.Dtos.Client;
public record class ClientDetailsDto
(
    int Id,
    string Username,
    string Role,
    DateTime PasswordChangedAt
);
