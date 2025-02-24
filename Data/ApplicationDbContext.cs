using MySql.Data.EntityFramework;
using NotifyHub.Models.Domain;
using System.Data.Entity.ModelConfiguration.Conventions;
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

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerEmail> CustomerEmails { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationRecipient> NotificationRecipients { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Remove Pluralizing Table Name Convention (optional)
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Configure User table
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

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

            // Configure cascade delete for CustomerEmail
            // Customer & CustomerEmails (One-to-Many)
            modelBuilder.Entity<CustomerEmail>()
                .HasRequired(ce => ce.Customer)
                .WithMany(c => c.CustomerEmails)
                .HasForeignKey(ce => ce.CustomerId)
                .WillCascadeOnDelete(true);

            // Configure cascade delete for NotificationRecipient
            // Customer & NotificationRecipients (One-to-Many)
            modelBuilder.Entity<NotificationRecipient>()
                .HasRequired(nr => nr.Customer)
                .WithMany(c => c.NotificationRecipients)
                .HasForeignKey(nr => nr.CustomerId)
                .WillCascadeOnDelete(true);

            // Notification & NotificationRecipients (One-to-Many)
            modelBuilder.Entity<NotificationRecipient>()
                .HasRequired(nr => nr.Notification)
                .WithMany(n => n.Recipients)
                .HasForeignKey(nr => nr.NotificationId)
                .WillCascadeOnDelete(true);

            // Configure composite index for NotificationRecipient
            modelBuilder.Entity<NotificationRecipient>()
                .HasIndex(nr => new { nr.CustomerId, nr.NotificationId });

            // Ensure email uniqueness per customer
            modelBuilder.Entity<CustomerEmail>()
                .HasIndex(ce => new { ce.CustomerId, ce.Email })
                .IsUnique();

            // Additional index for faster email lookups
            modelBuilder.Entity<CustomerEmail>()
                .HasIndex(ce => ce.Email);
        }
    }
}