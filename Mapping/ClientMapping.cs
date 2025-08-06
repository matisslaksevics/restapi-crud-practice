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
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email
            };
        }
        public static ClientSummaryDto ToClientSummaryDto(this Client client)
        {
            return new(
                client.Id,
                client.FirstName,
                client.LastName,
                client.Email
            );
        }
        public static ClientDetailsDto ToClientDetailsDto(this Client client)
        {
            return new(
                client.Id,
                client.FirstName,
                client.LastName,
                client.Email
            );
        }
        public static Client ToEntity(this UpdateClientDto client, int id)
        {
            return new Client()
            {
                Id = id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email
            };
        }
    }
}
