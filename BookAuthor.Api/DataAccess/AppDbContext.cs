using BookAuthor.Api.Configurations.EF;
using BookAuthor.Api.Model;
using BookAuthor.Api.Model.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookAuthor.Api.DataAccess
{
    public class AppDbContext : IdentityDbContext<ApiUser>
    {
        private readonly IConfiguration _configuration;

        public AppDbContext(
            DbContextOptions opt, 
            IConfiguration configuration
        ) : base(opt)
        {
            _configuration = configuration;
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<AuthorBook> AuthorBooks { get; set; }
        public DbSet<BookScore> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new RoleEntityConfiguration(_configuration));

            builder.ApplyConfiguration(new ApiUserEntityConfiguration(_configuration));

            builder.ApplyConfiguration(new UserRoleEntityConfiguration(_configuration));

            builder.ApplyConfiguration(new AuthorBookConfiguration());

            builder.ApplyConfiguration(new BookScoreConfiguration());

            foreach(var entityType in builder.Model.GetEntityTypes())
            {
                var entity = builder.Entity(entityType.ClrType);

                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    entity.Property<bool>(nameof(ISoftDelete.Deleted)).HasDefaultValue(false);
                    var queryFilterExpression_SoftDelete = GetISoftDeleteLE(entityType.ClrType);
                    entity.HasQueryFilter(queryFilterExpression_SoftDelete);
                }
            }
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateEntitiesState();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateEntitiesState();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }


        private void UpdateEntitiesState()
        {
            var entries = ChangeTracker.Entries().Where(e => e.State != EntityState.Detached && e.State != EntityState.Unchanged);

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        if(entry.Entity is ISoftDelete)
                        {
                            entry.Property("Deleted").CurrentValue = true;
                            entry.Property("DeletedAt").CurrentValue = DateTimeOffset.Now;
                            entry.State = EntityState.Modified;
                        }
                        break;
                    default:
                        break;
                }
            }

        }

        private LambdaExpression GetISoftDeleteLE(Type type)
        {
            var entityParameter = Expression.Parameter(type, "entity");
            var deletedProperty = Expression.Property(entityParameter, nameof(ISoftDelete.Deleted));
            var queryFilterExpression_SoftDelete = Expression
                .Lambda(Expression.Equal(deletedProperty, Expression.Constant(false)), entityParameter);

            return queryFilterExpression_SoftDelete;
        }
    }
}
