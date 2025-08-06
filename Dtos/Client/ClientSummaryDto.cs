namespace restapi_crud_practice.Dtos.Client;

public record class ClientSummaryDto
(
    int Id,
    string FirstName,
    string LastName,
    string Email
);

