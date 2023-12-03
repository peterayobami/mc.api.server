using Microsoft.AspNetCore.Mvc;

namespace Mc.Api.Server
{
    /// <summary>
    /// Manages author management standard Web API transactions
    /// </summary>
    public class AuthorController : ControllerBase
    {
        #region Private Members

        /// <summary>
        /// The singleton instance of the <see cref="ILogger{TCategoryName}"/>
        /// </summary>
        private readonly ILogger<AuthorManagement> logger;

        /// <summary>
        /// The scoped instance of the <see cref="AuthorManagement"/>
        /// </summary>
        private readonly AuthorManagement authorManagement;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="logger">The injected <see cref="ILogger"/></param>
        /// <param name="AuthorManagement">The injected <see cref="AuthorManagement"/></param>
        public AuthorController(ILogger<AuthorManagement> logger,
            AuthorManagement authorManagement)
        {
            this.logger = logger;
            this.authorManagement = authorManagement;
        }

        #endregion

        /// <summary>
        /// API endpoint to create author based on specified credentials
        /// </summary>
        /// <param name="credentials">The specified author credentials</param>
        [HttpPost(EndpointRoutes.CreateAuthor)]
        public async Task<ActionResult> CreateAuthorAsync([FromBody] AuthorCredentials credentials)
        {
            try
            {
                // Invoke the transaction
                var transaction = await authorManagement.CreateAsync(credentials);

                // If transaction failed...
                if (!transaction.Successful)
                {
                    // Return error response
                    return Problem(title: transaction.ErrorTitle,
                        statusCode: transaction.StatusCode, detail: transaction.ErrorMessage);
                }

                // Return response
                return Created(string.Empty, transaction.Result);
            }
            catch (Exception ex)
            {
                // Log the error
                logger.LogError(ex.Message);

                // Return error response
                return Problem(title: "SYSTEM ERROR",
                    statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
            }
        }

        /// <summary>
        /// API endpoint to retrieve authors
        /// </summary>
        [HttpGet(EndpointRoutes.FetchAuthors)]
        public async Task<ActionResult> FetchAuthorsAsync()
        {
            try
            {
                // Invoke the transaction
                var transaction = await authorManagement.FetchAsync();

                // If transaction failed...
                if (!transaction.Successful)
                {
                    // Return error response
                    return Problem(title: transaction.ErrorTitle,
                        statusCode: transaction.StatusCode, detail: transaction.ErrorMessage);
                }

                // Return response
                return Ok(transaction.Result);
            }
            catch (Exception ex)
            {
                // Log the error
                logger.LogError(ex.Message);

                // Return error response
                return Problem(title: "SYSTEM ERROR",
                    statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
            }
        }

        /// <summary>
        /// API endpoint to retrieve specified author
        /// </summary>
        [HttpGet(EndpointRoutes.FetchAuthor)]
        public async Task<ActionResult> FetchAuthorAsync(string id)
        {
            try
            {
                // Invoke the transaction
                var transaction = await authorManagement.FetchAsync(id);

                // If transaction failed...
                if (!transaction.Successful)
                {
                    // Return error response
                    return Problem(title: transaction.ErrorTitle,
                        statusCode: transaction.StatusCode, detail: transaction.ErrorMessage);
                }

                // Return response
                return Ok(transaction.Result);
            }
            catch (Exception ex)
            {
                // Log the error
                logger.LogError(ex.Message);

                // Return error response
                return Problem(title: "SYSTEM ERROR",
                    statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
            }
        }

        /// <summary>
        /// API endpoint to update specified author
        /// </summary>
        [HttpPut(EndpointRoutes.UpdateAuthor)]
        public async Task<ActionResult> UpdateAuthorAsync([FromBody] UpdateAuthorApiModel credentials, string id)
        {
            try
            {
                // Invoke the transaction
                var transaction = await authorManagement.UpdateAsync(id, credentials);

                // If transaction failed...
                if (!transaction.Successful)
                {
                    // Return error response
                    return Problem(title: transaction.ErrorTitle,
                        statusCode: transaction.StatusCode, detail: transaction.ErrorMessage);
                }

                // Return response
                return Ok(transaction.Result);
            }
            catch (Exception ex)
            {
                // Log the error
                logger.LogError(ex.Message);

                // Return error response
                return Problem(title: "SYSTEM ERROR",
                    statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
            }
        }

        /// <summary>
        /// API endpoint to detete specified author
        /// </summary>
        [HttpDelete(EndpointRoutes.DeleteAuthor)]
        public async Task<ActionResult> DeleteAuthorAsync(string id)
        {
            try
            {
                // Invoke the transaction
                var transaction = await authorManagement.DeleteAsync(id);

                // If transaction failed...
                if (!transaction.Successful)
                {
                    // Return error response
                    return Problem(title: transaction.ErrorTitle,
                        statusCode: transaction.StatusCode, detail: transaction.ErrorMessage);
                }

                // Return response
                return Ok(transaction.Result);
            }
            catch (Exception ex)
            {
                // Log the error
                logger.LogError(ex.Message);

                // Return error response
                return Problem(title: "SYSTEM ERROR",
                    statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
            }
        }
    }
}