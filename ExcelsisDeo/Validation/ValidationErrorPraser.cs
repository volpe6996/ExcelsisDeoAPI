using FluentValidation.Results;

namespace ExcelsisDeo.Validation
{
    public static class ValidationErrorPraser
    {
        private static Dictionary<string, string[]> Errors { get; set; }

        public static Dictionary<string, string[]> Prase(List<ValidationFailure> failure)
        {
            Errors = failure
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());

            return Errors;
        }
    }
}
