namespace Application.Common.Extensions
{
    public class Modules
    {
        public static string Core = "Core";
        public static string Inventory = "Inventory";
        public static string Maufacture = "Maufacture";
        public static string Log = "Log";
    }

    public class Screen
    {
        public static string Message = "Message";
        public static string LogAction = "LogAction";
        public static string User = "User";
        public static string OrgMember = "OrgMember";
        public static string Timesheet = "Timesheet";
    }

    public class ScreenKey
    {
        public static string COMMON = "Common";
        public static string ORGANIZATION_LEVEL = "OrganizationLevel";
        public static string ORGANIZATION_SUBORDINATE = "OrgSubordinate";
        public static string PARTY_CHARGE = "PartyCharge";
    }

    public class MessageKey
    {
        public static string I_001 = "Information processed successfully.";
        public static string I_002 = "Information saved.";
        public static string I_003 = "Request accepted.";

        public static string E_001 = "Error occurred.";
        public static string E_002 = "Invalid input.";
        public static string E_003 = "Operation failed.";
        public static string E_004 = "Resource not found.";
        public static string E_005 = "Access denied.";
        public static string E_007 = "Database error.";
        public static string E_008 = "Network error.";
        public static string E_009 = "Timeout error.";
        public static string E_010 = "Unknown error.";

        public static string BE_003 = "Unable to connect.";

        public static string W_001 = "Warning: Check your input.";
        public static string W_002 = "Warning: Action might not be reversible.";

        //Success
        public const string S_CREATE = "S_CREATE";
        public const string S_UPDATE = "S_UPDATE";
        public const string S_DELETE = "S_DELETE";

        //Error
        public const string E_CREATE = "E_CREATE";
        public const string E_UPDATE = "E_UPDATE";
        public const string E_DELETE = "E_DELETE";

        //Error File
        public const string E_FILE_INFO = "E_FILE_INFO";
        public const string E_FILE_VALID = "E_FILE_VALID";
        public const string E_FILE_FORMAT = "E_FILE_FORMAT";
        public const string E_FILE_NOT_FOUND = "E_FILE_NOT_FOUND";

    }

    public class CompanyType
    {
        public const string PLACE = "place";
        public const string BRANCH = "branch";
        public const string CUSTOMER = "customer";
        public const string SUPPLIER = "supplier";
        public const string OUTSOURCER = "outsourcer";
        public const string DESTINATION = "destination";
        public const string TRANSPOST = "transpost";
        public const string MAKER = "maker";    
    }
}
