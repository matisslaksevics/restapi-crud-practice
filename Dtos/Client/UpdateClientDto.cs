using System.ComponentModel.DataAnnotations;
namespace restapi_crud_practice.Dtos.Client;
public record class UpdateClientDto
(
    [Required][StringLength(20)] string FirstName,
    [Required][StringLength(30)] string LastName,
    [Required][StringLength(50)] string Email
);
