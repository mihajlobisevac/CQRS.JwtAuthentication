using System.Linq;

namespace Application.Common.Models
{
    public class Result
    {
        protected Result() { }

        protected Result(bool succeeded, string[] errors)
        {
            IsSuccessful = succeeded;
            Errors = errors.ToArray();
        }

        public bool IsSuccessful { get; protected set; }
        public string[] Errors { get; protected set; }

        public static Result Success() => new(true, null);
        public static Result Failure(string[] errors) => new(false, errors);
    }
}
