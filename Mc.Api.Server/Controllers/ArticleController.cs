using Microsoft.AspNetCore.Mvc;

namespace Mc.Api.Server
{
    /// <summary>
    /// Manages article management standard Web API transactions
    /// </summary>
    public class ArticleController : ControllerBase
    {
        #region Private Members

        /// <summary>
        /// The singleton instance of the <see cref="ILogger{TCategoryName}"/>
        /// </summary>
        private readonly ILogger<ArticleManagement> logger;

        /// <summary>
        /// The scoped instance of the <see cref="ArticleManagement"/>
        /// </summary>
        private readonly ArticleManagement articleManagement;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="logger">The injected <see cref="ILogger"/></param>
        /// <param name="articleManagement">The injected <see cref="ArticleManagement"/></param>
        public ArticleController(ILogger<ArticleManagement> logger,
            ArticleManagement articleManagement)
        {
            this.logger = logger;
            this.articleManagement = articleManagement;
        }

        #endregion

        /// <summary>
        /// API endpoint to create article based on specified credentials
        /// </summary>
        /// <param name="credentials">The specified article credentials</param>
        [HttpPost(EndpointRoutes.CreateArticle)]
        public async Task<ActionResult> CreateArticleAsync([FromBody] ArticleCredentials credentials)
        {
            try
            {
                // Invoke the transaction
                var transaction = await articleManagement.CreateAsync(credentials);

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
        /// API endpoint to retrieve articles
        /// </summary>
        [HttpGet(EndpointRoutes.FetchArticles)]
        public async Task<ActionResult> FetchArticlesAsync()
        {
            try
            {
                // Invoke the transaction
                var transaction = await articleManagement.FetchAsync();

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
        /// API endpoint to retrieve specified article
        /// </summary>
        [HttpGet(EndpointRoutes.FetchArticle)]
        public async Task<ActionResult> FetchArticleAsync(string id)
        {
            try
            {
                // Invoke the transaction
                var transaction = await articleManagement.FetchAsync(id);

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
        /// API endpoint to update specified article
        /// </summary>
        [HttpPut(EndpointRoutes.UpdateArticle)]
        public async Task<ActionResult> UpdateArticleAsync([FromBody] UpdateArticleApiModel credentials, string id)
        {
            try
            {
                // Invoke the transaction
                var transaction = await articleManagement.UpdateAsync(id, credentials);

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
        /// API endpoint to detete specified article
        /// </summary>
        [HttpDelete(EndpointRoutes.DeleteArticle)]
        public async Task<ActionResult> DeleteArticleAsync(string id)
        {
            try
            {
                // Invoke the transaction
                var transaction = await articleManagement.DeleteAsync(id);

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