using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Mc.Api.Server
{
    public class AuthorManagement
    {
        #region Private Members

        /// <summary>
        /// The scoped instance of the <see cref="ApplicationDbContext"/>
        /// </summary>
        private readonly ApplicationDbContext context;

        /// <summary>
        /// The singleton instance of the <see cref="ILogger{CategoryName}"/>
        /// </summary>
        private ILogger<AuthorManagement> logger;

        /// <summary>
        /// The scoped instance of the <see cref="CloudinaryService"/>
        /// </summary>
        private readonly CloudinaryService cloudinaryService;

        #endregion

        #region Constructor

        public AuthorManagement(ApplicationDbContext context,
            CloudinaryService cloudinaryService,
            ILogger<AuthorManagement> logger)
        {
            this.context = context;
            this.cloudinaryService = cloudinaryService;
            this.logger = logger;
        }

        #endregion

        /// <summary>
        /// Creates author based on provided credentials
        /// </summary>
        /// <param name="credentials">The provided author credentials</param>
        /// <returns></returns>
        public async Task<OperationResult> CreateAsync(AuthorCredentials credentials)
        {
            try
            {
                // Upload the photo
                var uploadResult = await cloudinaryService
                    .UploadAsync(credentials.Photo, uploadPreset: CloudinaryUploadPresets.AuthorPhoto);

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

                // Set the author's credentials
                var author = new AuthorsDataModel
                {
                    Title = credentials.Title,
                    FirstName = credentials.FirstName,
                    LastName = credentials.LastName,
                    PhotoId = uploadResult.ImageId,
                    PhotoUrl = uploadResult.ImageUrl
                };

                // Create the author
                await context.Authors.AddAsync(author);

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
                // Retrieve the authors
                var authors = await context.Authors
                    .Select(author => new AuthorApiModel
                    {
                        Id = author.Id,
                        Title = author.Title,
                        FirstName = author.FirstName,
                        LastName = author.LastName,
                        PhotoUrl = author.PhotoUrl,
                        DateModified = author.DateModified
                    })
                    .OrderByDescending(author => author.DateModified)
                    .ToListAsync();

                // Return the result
                return new OperationResult
                {
                    Result = authors,
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
                    logger.LogError("The author id is required");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "BAD REQUEST",
                        StatusCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = "The author id is required"
                    };
                }

                // Retrieve the author
                var author = await context.Authors
                    .Include(author => author.Articles)
                    .FirstOrDefaultAsync(author => author.Id == id);

                // If author was not found
                if (author == null)
                {
                    // Log the error
                    logger.LogError("Author with the specified id could not be found");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "NOT FOUND",
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Author with the specified id could not be found"
                    };
                }

                // Initialize author result
                AuthorApiModel authorResult = new()
                {
                    Id = author.Id,
                    Title = author.Title,
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    PhotoUrl = author.PhotoUrl,
                    DateModified = author.DateModified,
                    Articles = new()
                };

                // For each author's article...
                foreach (var article in author.Articles)
                {
                    // Add author's article
                    authorResult.Articles.Add(new ArticleApiModel
                    {
                        Id = article.Id,
                        Title = article.Title,
                        Description = article.Description,
                        Content = article.Content,
                        DateModified = article.DateModified
                    });
                }

                // Return the result
                return new OperationResult
                {
                    Result = authorResult,
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

        public async Task<OperationResult> UpdateAsync(string id, UpdateAuthorApiModel credentials)
        {
            try
            {
                // If id was not specified...
                if (string.IsNullOrEmpty(id))
                {
                    // Log the error
                    logger.LogError("The author id is required");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "BAD REQUEST",
                        StatusCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = "The author id is required"
                    };
                }

                // Retrieve the author
                var author = await context.Authors.FindAsync(id);

                // If author was not found
                if (author == null)
                {
                    // Log the error
                    logger.LogError("Author with the specified id could not be found");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "NOT FOUND",
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Author with the specified id could not be found"
                    };
                }

                // Modify the author
                author.Title = credentials.Title ?? author.Title;
                author.FirstName = credentials.FirstName ?? author.FirstName;
                author.LastName = credentials.LastName ?? author.LastName;

                // If photo was specified...
                if (!string.IsNullOrEmpty(credentials.Photo))
                {
                    // Upload the photo
                    var uploadResult = await cloudinaryService
                        .UploadAsync(credentials.Photo, uploadPreset: CloudinaryUploadPresets.AuthorPhoto);

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

                    // Delete the photo from cloud
                    await cloudinaryService.DeleteFromCloudinaryAsync(author.PhotoId);

                    // Modify the author image credentials
                    author.PhotoId = uploadResult.ImageId;
                    author.PhotoUrl = uploadResult.ImageUrl;
                }

                // Update the author
                context.Authors.Update(author);

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
                    logger.LogError("The author id is required");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "BAD REQUEST",
                        StatusCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = "The author id is required"
                    };
                }

                // Retrieve the author
                var author = await context.Authors.FindAsync(id);

                // If author was not found
                if (author == null)
                {
                    // Log the error
                    logger.LogError("Author with the specified id could not be found");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "NOT FOUND",
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Author with the specified id could not be found"
                    };
                }

                // Delete the caption from cloud
                await cloudinaryService.DeleteFromCloudinaryAsync(author.PhotoId);

                // Delete the author
                context.Authors.Remove(author);

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