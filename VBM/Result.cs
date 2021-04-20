namespace VBM
{
    public enum ResultType
    {
        Canceled,
        Success,
        Failure
    }

    public struct Result
    {
        public static readonly Result Canceled = new Result(null, ResultType.Canceled);
        public static readonly Result Success = new Result(null, ResultType.Success);

        public string Message { get; }
        public ResultType ResultType { get; }

        public Result(string message, ResultType type)
        {
            Message = message;
            ResultType = type;
        }

        public static Result Fail(string message)
        {
            return new Result(message, ResultType.Failure);
        }
        public static Result Succeed(string message)
        {
            return new Result(message, ResultType.Success);
        }
        public override string ToString()
        {
            if (Message == null)
                return ResultType.ToString();

            if (ResultType == ResultType.Success)
                return Message;

            return string.Format("{0}: {1}", ResultType, Message);
        }
    }
}
