namespace EmergencySituations.Other.Model
{
    public class MyRequestResult
    {
        public string Message { get; set; }
        public bool isError { get; set; }
        public MyRequestResult(string msg, bool error = false)
        {
            Message = msg;
            isError = error;
        }
    }
}
