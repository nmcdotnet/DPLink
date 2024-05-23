using System.Windows;
using static DipesLink.Datatypes.CommonDataType;

namespace DipesLink.Extensions
{
    public class AuthorizationHelper
    {
        public static bool IsUserInRole(UserRole role)
        {
            if (Application.Current.Properties["UserRole"] != null)
            {
                UserRole currentUserRole = (UserRole)Application.Current.Properties["UserRole"];
                return currentUserRole == role;
            }
            return false;
        }

        public static UserRole GetRole(object stringRole)
        {
            switch (stringRole)
            {
                case "admin":
                    return UserRole.Admin;
                case "operator":
                    return UserRole.Operator;
                case "user":
                    return UserRole.User;
                case "guest":
                    return UserRole.Guest;
                default:
                    return UserRole.Guest;
            }
        }

        public static bool IsAdmin()
        {
            return IsUserInRole(UserRole.Admin);
        }

        public static bool IsOperator()
        {
            return IsUserInRole(UserRole.Operator);
        }

        public static bool IsUser()
        {
            return IsUserInRole(UserRole.User);
        }

        public static bool IsGuest()
        {
            return IsUserInRole(UserRole.Guest);
        }
    }
}
