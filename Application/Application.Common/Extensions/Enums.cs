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
        public static string I_001 = "I_001";
        public static string I_002 = "I_002";
        public static string I_003 = "I_003";

        public static string E_001 = "E_001";
        public static string E_002 = "E_002";
        public static string E_003 = "E_003";
        public static string E_004 = "E_004";
        public static string E_005 = "E_005";
        public static string E_007 = "E_007";
        public static string E_008 = "E_008";
        public static string E_009 = "E_009";
        public static string E_010 = "E_010";

        public static string BE_003 = "BE_003";

        public static string W_001 = "W_001";
        public static string W_002 = "W_002";

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
