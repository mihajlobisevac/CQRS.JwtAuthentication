﻿namespace Web.Contracts
{
    public static class ApiRoutes
    {
        public const string GetByName = "by-name/{name}";
        public const string GetById = "by-id/{id}";

        public static class Auth
        {
            public const string Register = "register";
            public const string Login = "login";
            public const string RefreshToken = "refreshtoken";
        }

        public static class Users
        {
            public const string AddToRoleByName = "addtorole";
        }
    }
}
