using Microsoft.EntityFrameworkCore;
using backend.Models;


namespace backend.Data;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Attendance> Attendances { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<CashIn> CashIns { get; set; }

        public DbSet<CashOut> CashOuts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

                // Attendance -> Player
                modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Player)
                .WithMany(u => u.Attendances)
                .HasForeignKey(a => a.PlayerId);

                modelBuilder.Entity<Attendance>()
                    .HasIndex(a => new { a.PlayerId, a.AttendanceDate })
                    .IsUnique();


                // CashIn -> Player
                modelBuilder.Entity<CashIn>()
                    .HasOne(c => c.Player)
                    .WithMany(p => p.CashIns)
                    .HasForeignKey(c => c.PlayerId);
        }

    }
