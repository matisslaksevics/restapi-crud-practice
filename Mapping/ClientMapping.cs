using restapi_crud_practice.Dtos.Client;
using restapi_crud_practice.Entities;

namespace restapi_crud_practice.Mapping
{
    public static class ClientMapping
    {
        public static Client ToEntity(this CreateClientDto client) 
        {
            return new Client()
            {
                Username = client.Username,
                Role = client.Role,
                PasswordChangedAt = client.PasswordChangedAt
            };
        }
        public static ClientSummaryDto ToClientSummaryDto(this Client client)
        {
            return new(
                client.Id,
                client.Username,
                client.Role,
                client.PasswordChangedAt
            );
        }
        public static ClientDetailsDto ToClientDetailsDto(this Client client)
        {
            return new(
                client.Id,
                client.Username,
                client.Role,
                client.PasswordChangedAt
            );
        }
        public static Client ToEntity(this UpdateClientDto client, int id)
        {
            return new Client()
            {
                Id = id,
                Username = client.Username,
                Role = client.Role,
                PasswordChangedAt = client.PasswordChangedAt
            };
        }
    }
}
