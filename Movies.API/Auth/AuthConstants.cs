namespace Movies.API.Auth
{
    public class AuthConstants
    {
        public const string AdminUserPolicyName = "Admin";
        public const string UserAdminClaimName = "admin";

        public const string TrustedUserPolicyName = "Trusted";
        public const string TrustedUserClaimName = "trusted";

        public const string ApiKeyHeaderName = "x-api-key";
    }
}
    