namespace s28201_Project.Model.Employee;

public enum Role
{
    Admin,
    User,
    NoRole
}

public static class RoleExtensions
{
    public static string ToIndexString(this Role value)
    {
        switch (value)
        {
            case Role.Admin:
                return "0";
            case Role.User:
                return "1";
            case Role.NoRole:
                return "2";
            default:
                throw new ArgumentOutOfRangeException(nameof(value), value, null);
        }
    }
}