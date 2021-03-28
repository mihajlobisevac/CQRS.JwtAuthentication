namespace Web.Contracts
{
    public static class ApiRoutes
    {
        public static class Auth
        {
            public const string Register = "register";
            public const string Login = "login";
            public const string RefreshToken = "refreshtoken";
        }

        public static class Todos
        {
            public const string CreateById = "{id}";
        }
    }
}
