using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RVezy.Infra.Infra.Entities;
using System.Reflection;

namespace RVezy.Infra.Infra.Context
{
    public class ApplicationDbContext : DbContext
    {
        #region Constructors

        private readonly ILoggerFactory _loggerFactory;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILoggerFactory loggerFactory) : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        #endregion Constructors

        #region Properties

        #region Tables

        public DbSet<Listing> Listings { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Review> Reviews { get; set; }

        #endregion Tables

        #endregion Properties

        #region IdentityDbContext overrides

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseLoggerFactory(_loggerFactory);

        #endregion IdentityDbContext overrides
    }
}
