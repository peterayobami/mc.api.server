using System.Net;

namespace Mc.Api.Server
{
    /// <summary>
    /// The result of an operation / function / sub-routine
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        /// True if operation failed
        /// </summary>
        public bool Successful => ErrorMessage == null;

        /// <summary>
        /// The status code of the result
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// The title of the error
        /// </summary>
        public string ErrorTitle { get; set; }

        /// <summary>
        /// Represents a detailed error summary of an operation
        /// </summary>
        public object ErrorResult { get; set; }

        /// <summary>
        /// The error message of a failed operation
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// The result of the operation
        /// </summary>
        public object Result { get; set; }
    }

    /// <summary>
    /// The operation result for specifiec result objects
    /// </summary>
    /// <typeparam name="TResult">The result type</typeparam>
    public class OperationResult<TResult> : OperationResult
    {
        /// <summary>
        /// The generic result object
        /// </summary>
        public new TResult Result { get; set; }
    }
}
