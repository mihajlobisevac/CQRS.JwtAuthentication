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

        public static class Roles
        {
            public const string GetByName = "{name}";
        }

        public static class Todos
        {
            public const string GetById = "{id}";
        }
    }
}
