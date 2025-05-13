namespace LivefrontCartonCaps.Models.Shared
{
    /// <summary>
    /// Standard API response structure for consistent success and error handling.
    /// </summary>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indicates whether the API call was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Optional message for success or failure context.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Data returned on success.
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// List of error details (for validation or processing errors).
        /// </summary>
        public List<string>? Errors { get; set; }

        /// <summary>
        /// Creates a successful API response with the provided data and optional message.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResponse<T> Ok(T data, string? message = null) =>
            new() { Success = true, Data = data, Message = message };

        /// <summary>
        /// Creates a failed API response with the provided message and optional list of errors.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static ApiResponse<T> Fail(string message, List<string>? errors = null) =>
            new() { Success = false, Message = message, Errors = errors };
    }
}