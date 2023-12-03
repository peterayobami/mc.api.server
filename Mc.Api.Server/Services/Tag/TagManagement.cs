using Microsoft.EntityFrameworkCore;

namespace Mc.Api.Server
{
    public class TagManagement
    {
        #region Private Members

        /// <summary>
        /// The scoped instance of the <see cref="ApplicationDbContext"/>
        /// </summary>
        private readonly ApplicationDbContext context;

        /// <summary>
        /// The singleton instance of the <see cref="ILogger{TCategoryName}"/>
        /// </summary>
        private ILogger<TagManagement> logger;

        /// <summary>
        /// The scoped instance of the <see cref="CloudinaryService"/>
        /// </summary>
        private readonly CloudinaryService cloudinaryService;

        #endregion

        #region Constructor

        public TagManagement(ApplicationDbContext context,
            CloudinaryService cloudinaryService,
            ILogger<TagManagement> logger)
        {
            this.context = context;
            this.cloudinaryService = cloudinaryService;
            this.logger = logger;
        }

        #endregion

        /// <summary>
        /// Creates tag based on provided credentials
        /// </summary>
        /// <param name="credentials">The provided tag credentials</param>
        /// <returns></returns>
        public async Task<OperationResult> CreateAsync(TagCredentials credentials)
        {
            try
            {
                // Set the tag credentials
                var tag = new TagsDataModel
                {
                    Title = credentials.Title
                };

                // Create the tag
                await context.Tags.AddAsync(tag);

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
                // Retrieve the tags
                var tags = await context.Tags
                    .Select(tag => new TagApiModel
                    {
                        Id = tag.Id,
                        Title = tag.Title,
                        DateModified = tag.DateModified
                    })
                    .OrderByDescending(tag => tag.DateModified)
                    .ToListAsync();

                // Return the result
                return new OperationResult
                {
                    Result = tags,
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
                    logger.LogError("The tag id is required");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "BAD REQUEST",
                        StatusCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = "The tag id is required"
                    };
                }

                // Retrieve the tag
                var tag = await context.Tags
                    .Select(tag => new TagApiModel
                    {
                        Id = tag.Id,
                        Title = tag.Title,
                        DateModified = tag.DateModified
                    })
                    .FirstOrDefaultAsync(tag => tag.Id == id);

                // If tag was not found
                if (tag == null)
                {
                    // Log the error
                    logger.LogError("Tag with the specified id could not be found");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "NOT FOUND",
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Tag with the specified id could not be found"
                    };
                }

                // Return the result
                return new OperationResult
                {
                    Result = tag,
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

        public async Task<OperationResult> UpdateAsync(string id, TagCredentials credentials)
        {
            try
            {
                // If id was not specified...
                if (string.IsNullOrEmpty(id))
                {
                    // Log the error
                    logger.LogError("The tag id is required");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "BAD REQUEST",
                        StatusCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = "The tag id is required"
                    };
                }

                // Retrieve the tag
                var tag = await context.Tags.FindAsync(id);

                // If tag was not found
                if (tag == null)
                {
                    // Log the error
                    logger.LogError("Tag with the specified id could not be found");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "NOT FOUND",
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Tag with the specified id could not be found"
                    };
                }

                // Modify the tag
                tag.Title = credentials.Title ?? tag.Title;

                // Update the tag
                context.Tags.Update(tag);

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
                    logger.LogError("The tag id is required");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "BAD REQUEST",
                        StatusCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = "The tag id is required"
                    };
                }

                // Retrieve the tag
                var tag = await context.Tags.FindAsync(id);

                // If tag was not found
                if (tag == null)
                {
                    // Log the error
                    logger.LogError("Tag with the specified id could not be found");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "NOT FOUND",
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Tag with the specified id could not be found"
                    };
                }

                // Delete the tag
                context.Tags.Remove(tag);

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