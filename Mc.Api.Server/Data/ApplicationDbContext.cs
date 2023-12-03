using Microsoft.EntityFrameworkCore;

namespace Mc.Api.Server
{
    /// <summary>
    /// The database representational model for the application
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="options">The <see cref="DbContextOptions"/></param>
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        /// <summary>
        /// The tags database table
        /// </summary>
        public DbSet<TagsDataModel> Tags { get; set; }

        /// <summary>
        /// The authors database table
        /// </summary>
        public DbSet<AuthorsDataModel> Authors { get; set; }

        /// <summary>
        /// The articles database table
        /// </summary>
        public DbSet<ArticlesDataModel> Articles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Override the save changes async
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Check the entries to db
            foreach (var entry in ChangeTracker.Entries<BaseDataModel>())
            {
                // Set the date modified
                entry.Entity.DateModified = DateTimeOffset.Now;

                // If a record is being added
                if (entry.State == EntityState.Added)
                {
                    // Set the date created
                    entry.Entity.DateCreated = DateTimeOffset.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}