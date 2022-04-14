using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace BankingSystem.IdentityServer
{
    public static class IdentityConfiguration
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("BankingSystemApi", "Banking System Api")
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("BankingSystemApi", "Banking system api", new []
                    { JwtClaimTypes.Name })
                {
                    Scopes = { "BankingSystemApi" }
                }
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "BankingSystem",
                    ClientName = "BankingSystem",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret> { new Secret("secret".Sha256()) },
                    AllowedScopes = { "BankingSystemApi" }
                }
            };
    }
}
