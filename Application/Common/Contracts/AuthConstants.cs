namespace Application.Common.Contracts
{
    public static class AuthConstants
    {
        public static class Policies
        {
            public const string Admin = "AdminPolicy";
            public const string Editor = "EditorPolicy";
            public const string User = "UserPolicy";
        }

        public static class Roles
        {
            public const string Admin = "Admin";
            public const string Editor = "Editor";
            public const string User = "User";
        }

        public static class Claims
        {
            public const string ManageUsers = "CanManageUsers";
            public const string ManageTodos = "CanManageTodos";
            public const string ViewTodos = "CanViewTodos";
        }
    }
}
