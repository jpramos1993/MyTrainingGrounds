﻿namespace Movies.API.Auth;

public static class AuthConstants
{
    public const string AdminUserPolicyName = "Admin";
    public const string AdminUserClaimName = "admin";
    public const string TrustMemberPolicyName = "TrustedMember";
    public const string TrustedUserClaimName = "trustedMember";

}

public static class IdentityExtensions
{
    public static Guid? GetUserId(this HttpContext context)
    {
        var userId = context.User.Claims.SingleOrDefault(x=> x.Type == "userId");
        
        if(Guid.TryParse(userId?.Value, out var parsedId))
        {
            return parsedId;
        }
        
        return null;
    }
}
