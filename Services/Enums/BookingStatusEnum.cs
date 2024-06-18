namespace Services.Enums
{
    public enum BookingStatusEnum
    {
        Success,
        Failed,
        Pending
    }

    public static partial class EntitiesEnumExtensions
    {
        public static string ToFriendlyName(this BookingStatusEnum me)
        {
            switch (me)
            {
                case BookingStatusEnum.Success:
                    return "Success";
                case BookingStatusEnum.Failed:
                    return "Failed";
                case BookingStatusEnum.Pending:
                    return "Pending";
                default:
                    return "Unknown";

            }
        }
    }
}
