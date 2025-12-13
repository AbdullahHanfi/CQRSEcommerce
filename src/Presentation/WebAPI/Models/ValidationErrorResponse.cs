namespace WebAPI.Models
{
    public class ValidationErrorResponse
    {
        public ValidationErrorResponse(string field, string[] messages)
        {
            Field = field;
            Messages = messages;
        }
        /// <summary>
        /// The field that has a validation error.
        /// </summary>
        /// <example>Password</example>
        public string Field { get; }

        /// <summary>
        /// The list of validation error messages for the field.
        /// </summary>
        /// <example>["Password is required.", "Passwords do not match."]</example>
        public string[] Messages { get; }
    }
}
