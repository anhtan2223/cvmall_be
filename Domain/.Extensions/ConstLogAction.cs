using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Extensions
{
    public class LogActionFeature
    {
        public const string OrgType = "OrgType";
        public const string OrgInfo = "OrgInfo";
        public const string Locality = "Locality";
        public const string BankInfo = "BankInfo";
    }
    public class LogActionMethod
    {
        public const string Read = "Read";

        public const string ReadOne = "ReadOne";

        public const string Create = "Create";
        public const string Update = "Update";
        public const string Delete = "Delete";
    }
}
