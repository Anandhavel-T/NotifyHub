namespace NotifyHub.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.customerEmails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        Email = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        IsPrimary = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => new { t.CustomerId, t.Email }, unique: true)
                .Index(t => t.Email, name: "IX_CustomerEmail_Email");
            
            CreateTable(
                "dbo.customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        IsActive = c.Boolean(nullable: false),
                        Phone = c.String(maxLength: 20, storeType: "nvarchar"),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        UpdatedAt = c.DateTime(precision: 0),
                        CreatedById = c.Int(),
                        UpdatedById = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.users", t => t.CreatedById)
                .ForeignKey("dbo.users", t => t.UpdatedById)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);
            
            CreateTable(
                "dbo.users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        UserName = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        PasswordHash = c.String(nullable: false, unicode: false),
                        Email = c.String(nullable: false, maxLength: 150, storeType: "nvarchar"),
                        IsActive = c.Boolean(nullable: false),
                        LastLoginDate = c.DateTime(nullable: false, precision: 0),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        UpdatedAt = c.DateTime(precision: 0),
                        CreatedById = c.Int(),
                        UpdatedById = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.users", t => t.CreatedById)
                .ForeignKey("dbo.users", t => t.UpdatedById)
                .Index(t => t.UserName, unique: true)
                .Index(t => t.Email, unique: true)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);
            
            CreateTable(
                "dbo.notification_recipients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        NotificationId = c.Int(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        IsDelivered = c.Boolean(nullable: false),
                        ReadAt = c.DateTime(precision: 0),
                        DeliveredAt = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.customers", t => t.CustomerId)
                .ForeignKey("dbo.notifications", t => t.NotificationId)
                .Index(t => t.CustomerId)
                .Index(t => t.NotificationId);
            
            CreateTable(
                "dbo.notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        ShortDescription = c.String(nullable: false, maxLength: 500, storeType: "nvarchar"),
                        LongDescription = c.String(nullable: false, unicode: false),
                        Type = c.Int(nullable: false),
                        ImageUrl = c.String(unicode: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        ScheduledAt = c.DateTime(nullable: false, precision: 0),
                        ExpiryAt = c.DateTime(precision: 0),
                        Status = c.Int(nullable: false),
                        Priority = c.Int(nullable: false),
                        IsGlobal = c.Boolean(nullable: false),
                        CreatedById = c.Int(nullable: false),
                        UpdatedById = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedAt = c.DateTime(precision: 0),
                        DeletedById = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.users", t => t.CreatedById, cascadeDelete: true)
                .ForeignKey("dbo.users", t => t.UpdatedById)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.customerEmails", "CustomerId", "dbo.customers");
            DropForeignKey("dbo.customers", "UpdatedById", "dbo.users");
            DropForeignKey("dbo.notification_recipients", "NotificationId", "dbo.notifications");
            DropForeignKey("dbo.notifications", "UpdatedById", "dbo.users");
            DropForeignKey("dbo.notifications", "CreatedById", "dbo.users");
            DropForeignKey("dbo.notification_recipients", "CustomerId", "dbo.customers");
            DropForeignKey("dbo.customers", "CreatedById", "dbo.users");
            DropForeignKey("dbo.users", "UpdatedById", "dbo.users");
            DropForeignKey("dbo.users", "CreatedById", "dbo.users");
            DropIndex("dbo.notifications", new[] { "UpdatedById" });
            DropIndex("dbo.notifications", new[] { "CreatedById" });
            DropIndex("dbo.notification_recipients", new[] { "NotificationId" });
            DropIndex("dbo.notification_recipients", new[] { "CustomerId" });
            DropIndex("dbo.users", new[] { "UpdatedById" });
            DropIndex("dbo.users", new[] { "CreatedById" });
            DropIndex("dbo.users", new[] { "Email" });
            DropIndex("dbo.users", new[] { "UserName" });
            DropIndex("dbo.customers", new[] { "UpdatedById" });
            DropIndex("dbo.customers", new[] { "CreatedById" });
            DropIndex("dbo.customerEmails", "IX_CustomerEmail_Email");
            DropIndex("dbo.customerEmails", new[] { "CustomerId", "Email" });
            DropIndex("dbo.customerEmails", new[] { "CustomerId" });
            DropTable("dbo.notifications");
            DropTable("dbo.notification_recipients");
            DropTable("dbo.users");
            DropTable("dbo.customers");
            DropTable("dbo.customerEmails");
        }
    }
}
