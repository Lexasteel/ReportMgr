namespace Models
{
    public class ReportEventArgs
    {
        public int Value { get; }
        public string? Message { get; }
        public  int? Level { get; }

        public bool WriteLog;
        public ReportEventArgs(int value, int? level, string? message)
        {
            Value = value;
            Message = message;
            Level = level;
        }
    }

}
