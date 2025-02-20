using MySql.Data.EntityFramework;
using NotifyHub.Models.Domain;
using System.Data.Entity;

namespace NotifyHub.Data
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
            // Disable automatic migrations
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configure User self-referencing relationships
            modelBuilder.Entity<User>()
                .HasOptional(u => u.CreatedBy)
                .WithMany()
                .HasForeignKey(u => u.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasOptional(u => u.UpdatedBy)
                .WithMany()
                .HasForeignKey(u => u.UpdatedById)
                .WillCascadeOnDelete(false);

            // Configure cascade delete for NotificationRecipient
            modelBuilder.Entity<NotificationRecipient>()
                .HasRequired(nr => nr.Customer)
                .WithMany(c => c.NotificationRecipients)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NotificationRecipient>()
                .HasRequired(nr => nr.Notification)
                .WithMany(n => n.Recipients)
                .WillCascadeOnDelete(false);

            // Configure cascade delete for CustomerEmail
            modelBuilder.Entity<CustomerEmail>()
                .HasRequired(ce => ce.Customer)
                .WithMany(c => c.CustomerEmails)
                .HasForeignKey(e => e.CustomerId)
                .WillCascadeOnDelete(true);

            // Configure email uniqueness per customer
            modelBuilder.Entity<CustomerEmail>()
                .HasIndex(e => new { e.CustomerId, e.Email })
                .IsUnique();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationRecipient> NotificationRecipients { get; set; }
        public DbSet<CustomerEmail> CustomerEmails { get; set; }
    }
}