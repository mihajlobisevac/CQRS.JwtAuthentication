namespace Application.Common.Contracts
{
    public static class AuthConstants
    {
        public static class Policies
        {
            public const string Admin = "RequireAdminRole";
            public const string Editor = "RequireEditorRole";
            public const string User = "RequireUserRole";
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
