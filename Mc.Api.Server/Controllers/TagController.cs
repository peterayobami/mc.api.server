using Microsoft.AspNetCore.Mvc;

namespace Mc.Api.Server
{
    /// <summary>
    /// Manages tag standard Web API transactions
    /// </summary>
    public class TagController : ControllerBase
    {
        #region Private Members

        /// <summary>
        /// The singleton instance of the <see cref="ILogger{TCategoryName}"/>
        /// </summary>
        private readonly ILogger<TagManagement> logger;

        /// <summary>
        /// The scoped instance of the <see cref="TagManagement"/>
        /// </summary>
        private readonly TagManagement tagManagement;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="logger">The injected <see cref="ILogger"/></param>
        /// <param name="tagManagement">The injected <see cref="TagManagement"/></param>
        public TagController(ILogger<TagManagement> logger,
            TagManagement tagManagement)
        {
            this.logger = logger;
            this.tagManagement = tagManagement;
        }

        #endregion

        /// <summary>
        /// API endpoint to create tag based on specified credentials
        /// </summary>
        /// <param name="credentials">The specified tag credentials</param>
        [HttpPost(EndpointRoutes.CreateTag)]
        public async Task<ActionResult> CreateTagAsync([FromBody] TagCredentials credentials)
        {
            try
            {
                // Invoke the transaction
                var transaction = await tagManagement.CreateAsync(credentials);

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
        /// API endpoint to retrieve tags
        /// </summary>
        [HttpGet(EndpointRoutes.FetchTags)]
        public async Task<ActionResult> FetchTagsAsync()
        {
            try
            {
                // Invoke the transaction
                var transaction = await tagManagement.FetchAsync();

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
        /// API endpoint to retrieve specified tag
        /// </summary>
        [HttpGet(EndpointRoutes.FetchTag)]
        public async Task<ActionResult> FetchTagAsync(string id)
        {
            try
            {
                // Invoke the transaction
                var transaction = await tagManagement.FetchAsync(id);

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
        /// API endpoint to update specified tag
        /// </summary>
        [HttpPut(EndpointRoutes.UpdateTag)]
        public async Task<ActionResult> UpdateTagAsync([FromBody] TagCredentials credentials, string id)
        {
            try
            {
                // Invoke the transaction
                var transaction = await tagManagement.UpdateAsync(id, credentials);

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
        /// API endpoint to detete specified tag
        /// </summary>
        [HttpDelete(EndpointRoutes.DeleteTag)]
        public async Task<ActionResult> DeleteTagAsync(string id)
        {
            try
            {
                // Invoke the transaction
                var transaction = await tagManagement.DeleteAsync(id);

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