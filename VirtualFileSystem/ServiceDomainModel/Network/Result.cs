using System;

namespace ServiceDomainModel
{
    [Serializable]
    public class Result
    {
        public string Message { get; protected set; }

        public Result(string message)
        {
            Message = message;
        }
    }

    [Serializable]
    public class ResultReturnValue : Result
    {
        public object Result { get; private set; }

        public ResultReturnValue(string message, object result)
            : base(message)
        {
            Message = message;
            Result = result;
        }
    }
}
