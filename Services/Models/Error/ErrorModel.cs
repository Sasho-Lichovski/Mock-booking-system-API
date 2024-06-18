namespace Services.Models.Error
{
    public class ErrorModel
    {
        public string ErrorMessage { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string StackTrace { get; set; }
        public string AdditionalDetails { get; set; }
        public DateTime LogTime { get; set; }
    }
}
