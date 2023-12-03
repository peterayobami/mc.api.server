using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Mc.Api.Server
{
    public class ArticleManagement
    {
        #region Private Members

        /// <summary>
        /// The scoped instance of the <see cref="ApplicationDbContext"/>
        /// </summary>
        private readonly ApplicationDbContext context;

        /// <summary>
        /// The singleton instance of the <see cref="ILogger{CategoryName}"/>
        /// </summary>
        private ILogger<ArticleManagement> logger;

        /// <summary>
        /// The scoped instance of the <see cref="CloudinaryService"/>
        /// </summary>
        private readonly CloudinaryService cloudinaryService;

        #endregion

        #region Constructor

        public ArticleManagement(ApplicationDbContext context,
            CloudinaryService cloudinaryService,
            ILogger<ArticleManagement> logger)
        {
            this.context = context;
            this.cloudinaryService = cloudinaryService;
            this.logger = logger;
        }

        #endregion

        /// <summary>
        /// Creates article based on provided credentials
        /// </summary>
        /// <param name="credentials">The provided article credentials</param>
        /// <returns></returns>
        public async Task<OperationResult> CreateAsync(ArticleCredentials credentials)
        {
            try
            {
                // If author id does not exist...
                if (!await context.Authors.AnyAsync(author => author.Id == credentials.AuthorId))
                {
                    // Log error
                    logger.LogError("The specified author id does not match an existing author");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "BAD REQUEST",
                        StatusCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = "The specified author id does not match any existing author"
                    };
                }
                
                // Upload the caption
                var uploadResult = await cloudinaryService
                    .UploadAsync(credentials.Caption, uploadPreset: CloudinaryUploadPresets.ArticleCaption);

                // If upload failed...
                if (!uploadResult.Successful)
                {
                    // Log the error
                    logger.LogError(uploadResult.ErrorMessage + "\n" + JsonSerializer.Serialize(uploadResult.Errors));

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "SYSTEM ERROR",
                        StatusCode = StatusCodes.Status500InternalServerError,
                        ErrorMessage = uploadResult.ErrorMessage + "\n" + JsonSerializer.Serialize(uploadResult.Errors)
                    };
                }

                // Set the article credentials
                var article = new ArticlesDataModel
                {
                    Title = credentials.Title,
                    Description = credentials.Description,
                    AuthorId = credentials.AuthorId,
                    Content = credentials.Content,
                    Tags = credentials.Tags.Length < 1 ? null : string.Join(',', credentials.Tags),
                    ImageId = uploadResult.ImageId,
                    ImageUrl = uploadResult.ImageUrl
                };

                // Create the article
                await context.Articles.AddAsync(article);

                // Save changes
                await context.SaveChangesAsync();

                // Return result
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status201Created
                };
            }
            catch (Exception ex)
            {
                // Log the error
                logger.LogError(ex.Message);

                // Log the stack trace
                logger.LogError(ex.StackTrace);

                // Return error result
                return new OperationResult
                {
                    ErrorTitle = "SYSTEM ERROR",
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<OperationResult> FetchAsync()
        {
            try
            {
                // Retrieve the articles
                var articles = await context.Articles
                    .Include(article => article.Author)
                    .Select(article => new ArticleApiModel
                    {
                        Id = article.Id,
                        Title = article.Title,
                        Description = article.Description,
                        Content = article.Content,
                        Author = new AuthorApiModel
                        {
                            FirstName = article.Author.FirstName,
                            LastName = article.Author.LastName,
                            PhotoUrl = article.Author.PhotoUrl
                        },
                        DateModified = article.DateModified
                    })
                    .OrderByDescending(author => author.DateModified)
                    .ToListAsync();

                // Return the result
                return new OperationResult
                {
                    Result = articles,
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {
                // Log the error
                logger.LogError(ex.Message);

                // Log the stack trace
                logger.LogError(ex.StackTrace);

                // Return error result
                return new OperationResult
                {
                    ErrorTitle = "SYSTEM ERROR",
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<OperationResult> FetchAsync(string id)
        {
            try
            {
                // If id was not specified...
                if (string.IsNullOrEmpty(id))
                {
                    // Log the error
                    logger.LogError("The article id is required");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "BAD REQUEST",
                        StatusCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = "The article id is required"
                    };
                }

                // Retrieve the article
                var article = await context.Articles
                    .Include(article => article.Author)
                    .Select(article => new ArticleApiModel
                    {
                        Id = article.Id,
                        Title = article.Title,
                        Description = article.Description,
                        Content = article.Content,
                        Author = new AuthorApiModel
                        {
                            FirstName = article.Author.FirstName,
                            LastName = article.Author.LastName,
                            PhotoUrl = article.Author.PhotoUrl
                        },
                        DateModified = article.DateModified
                    })
                    .FirstOrDefaultAsync(article => article.Id == id);

                // If article was not found
                if (article == null)
                {
                    // Log the error
                    logger.LogError("Article with the specified id could not be found");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "NOT FOUND",
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Article with the specified id could not be found"
                    };
                }

                // Return the result
                return new OperationResult
                {
                    Result = article,
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {
                // Log the error
                logger.LogError(ex.Message);

                // Log the stack trace
                logger.LogError(ex.StackTrace);

                // Return error result
                return new OperationResult
                {
                    ErrorTitle = "SYSTEM ERROR",
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<OperationResult> UpdateAsync(string id, UpdateArticleApiModel credentials)
        {
            try
            {
                // If id was not specified...
                if (string.IsNullOrEmpty(id))
                {
                    // Log the error
                    logger.LogError("The article id is required");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "BAD REQUEST",
                        StatusCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = "The article id is required"
                    };
                }

                // Retrieve the article
                var article = await context.Articles.FindAsync(id);

                // If article was not found...
                if (article == null)
                {
                    // Log the error
                    logger.LogError("Article with the specified id could not be found");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "NOT FOUND",
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Article with the specified id could not be found"
                    };
                }

                // Modify the article
                article.Title = credentials.Title ?? article.Title;
                article.Description = credentials.Description ?? article.Description;
                article.Content = credentials.Content ?? article.Content;
                article.AuthorId = credentials.AuthorId ?? article.AuthorId;
                article.Tags = credentials.Tags.Length < 1 ? article.Tags : string.Join(',', credentials.Tags);

                // If image was specified...
                if (!string.IsNullOrEmpty(credentials.Caption))
                {
                    // Upload the image
                    var uploadResult = await cloudinaryService
                        .UploadAsync(credentials.Caption, uploadPreset: CloudinaryUploadPresets.ArticleCaption);

                    // If upload failed...
                    if (!uploadResult.Successful)
                    {
                        // Log the error
                        logger.LogError(uploadResult.ErrorMessage + "\n" + JsonSerializer.Serialize(uploadResult.Errors));

                        // Return error result
                        return new OperationResult
                        {
                            ErrorTitle = "SYSTEM ERROR",
                            StatusCode = StatusCodes.Status500InternalServerError,
                            ErrorMessage = uploadResult.ErrorMessage + "\n" + JsonSerializer.Serialize(uploadResult.Errors)
                        };
                    }

                    // Delete the caption from cloud
                    await cloudinaryService.DeleteFromCloudinaryAsync(article.ImageId);

                    // Modify the article image credentials
                    article.ImageId = uploadResult.ImageId;
                    article.ImageUrl = uploadResult.ImageUrl;
                }

                // Update the article
                context.Articles.Update(article);

                // Save changes
                await context.SaveChangesAsync();

                // Return the result
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status201Created
                };
            }
            catch (Exception ex)
            {
                // Log the error
                logger.LogError(ex.Message);

                // Log the stack trace
                logger.LogError(ex.StackTrace);

                // Return error result
                return new OperationResult
                {
                    ErrorTitle = "SYSTEM ERROR",
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<OperationResult> DeleteAsync(string id)
        {
            try
            {
                // If id was not specified...
                if (string.IsNullOrEmpty(id))
                {
                    // Log the error
                    logger.LogError("The article id is required");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "BAD REQUEST",
                        StatusCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = "The article id is required"
                    };
                }

                // Retrieve the article
                var article = await context.Articles.FindAsync(id);

                // If article was not found...
                if (article == null)
                {
                    // Log the error
                    logger.LogError("Article with the specified id could not be found");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "NOT FOUND",
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Article with the specified id could not be found"
                    };
                }

                // Delete the caption from cloud
                await cloudinaryService.DeleteFromCloudinaryAsync(article.ImageId);

                // Delete the article
                context.Articles.Remove(article);

                // Save changes
                await context.SaveChangesAsync();

                // Return the result
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status204NoContent
                };
            }
            catch (Exception ex)
            {
                // Log the error
                logger.LogError(ex.Message);

                // Log the stack trace
                logger.LogError(ex.StackTrace);

                // Return error result
                return new OperationResult
                {
                    ErrorTitle = "SYSTEM ERROR",
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}