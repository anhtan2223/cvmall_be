using System.Runtime.Serialization;

namespace Domain.Enums
{
    public enum Gender
    {
        [EnumMember(Value = "M")]
        Male = 0,
        [EnumMember(Value = "F")]
        FeMale = 1,
        [EnumMember(Value = "O")]
        Other = 2
    }

    public enum CalendarStatus
    {
        FullDay = 0,
        PartDay = 1,
        Absent = 2
    }

    public enum ReceiveOrderStatus
    {
        UnConfirmed = 0,
        Confirmed = 1
    }
}
