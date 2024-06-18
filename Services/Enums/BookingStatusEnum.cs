namespace Services.Enums
{
    public enum BookingStatusEnum
    {
        Success,
        Failed,
        Pending,
        NotExist
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
                case BookingStatusEnum.NotExist:
                    return "Booking does not exist";
                default:
                    return "Unknown";

            }
        }
    }
}
