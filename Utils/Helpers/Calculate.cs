namespace Utils.Helpers
{
    public static class Calculate
    {
        public static bool IsLastMinuteCall(DateTime dateFrom, int daysCount)
        {
            DateTime today = DateTime.Now;
            DateTime targetDate = today.AddDays(daysCount);

            return dateFrom > today && dateFrom <= targetDate;
        }

        public static string GetOptionCode(int length)
        {
            var random = new Random();
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, length)
                .Select(x => pool[random.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }

        public static double GetRandomNumberBetween(int small, int big)
        {
            Random rng = new Random();
            return rng.Next(small, big) + rng.NextSingle();
        }
    }
}
