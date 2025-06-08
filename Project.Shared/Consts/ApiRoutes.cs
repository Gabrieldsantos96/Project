namespace Project.Shared.Consts;

public static class ApiRoutes
{

    public static class Users
    {
        public static class Account
        {
            public const string GetUserProfile = "/account/profile";
            public const string ChangeJobRole = "/account/change-job-role";
            public const string CreateAccount = "/account/create";
            public const string AuthenticateUser = "/account/auth";
            public const string AuthenticateWithTenant = "/account/tenant-auth";
            public const string Logout = "/account/logout";
            public const string ResetPassword = "/account/reset-password";
            public const string ResetPasswordRequest = "/account/reset-password/request";
            public const string ChangePassword = "/account/change-password";
        }

    }
}
