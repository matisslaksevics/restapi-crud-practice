using System.ComponentModel.DataAnnotations;
namespace restapi_crud_practice.Dtos.Client;

public record class CreateClientDto
(
    [Required][StringLength(20)] string Username,
    [Required][StringLength(30)] string Role,
    DateTime PasswordChangedAt
);

