using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using ReactProject.Models;
using Microsoft.Extensions.Hosting;


namespace ReactProject.Data
{
    public class TwitterDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<TwitterPost> TwitterPosts { get; set; }

        public DbSet<Like> Likes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=JonsiTwitterDB;Trusted_Connection=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(i => i.ImageUrl)
                .HasDefaultValue("https://localhost:7167/images/default.jpg");


                modelBuilder.Entity<User>()
                    .HasIndex(u => u.UserName)
                    .IsUnique();


            modelBuilder.Entity<User>().HasMany(u => u.Following)
                .WithMany(u => u.Followers).UsingEntity<Dictionary<string, object>>("UserFollows", b => b.HasOne<User>()
                .WithMany().HasForeignKey("FollowerId").OnDelete(DeleteBehavior.Restrict), b => b.HasOne<User>().WithMany()
                .HasForeignKey("FollowingId").OnDelete(DeleteBehavior.Restrict));

            modelBuilder.Entity<TwitterPost>()
                .Property(d => d.Date)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Like>()
                .HasKey(l => new { l.UserId, l.TwitterPostId });

            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict); // restrict cascade delete

            modelBuilder.Entity<Like>()
                .HasOne(l => l.TwitterPost)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.TwitterPostId)
                .OnDelete(DeleteBehavior.Cascade);




            User u1= new User { UserId = 1, UserName = "MissUser", Password = "TestPassword", Email = "Test@Test.com", CreatedOnDate = DateTime.UtcNow.Date ,};
            User u2 = new User { UserId = 2, UserName = "MisterUser", Password = "TestPassword", Email = "Test@Test.com", CreatedOnDate = DateTime.UtcNow.Date};
            User u3 = new User { UserId = 3, UserName = "TestUser", Password = "TestPassword", Email = "Test@Test.com", CreatedOnDate = DateTime.UtcNow.Date, ImageUrl= "https://localhost:7167/images/cat.png"};
            User u4 = new User { UserId = 4, UserName = "Miss4User", Password = "TestPassword", Email = "Test@Test.com", CreatedOnDate = DateTime.UtcNow.Date };





            TwitterPost p1 = new TwitterPost
            {
                TwitterPostId= 1,
                Text = "Hello world!",
                ImageURl = "https://localhost:7167/images/cat.png",
                UserId = 1,
            };

            TwitterPost p2 = new TwitterPost
            {
                TwitterPostId = 2,
                Text = "Hello 23 world!",
                ImageURl = "https://localhost:7167/images/cat.png",
                UserId = 3,
            };

            TwitterPost p3 = new TwitterPost
            {
                TwitterPostId = 3,
                Text = "Hello 3 world!",
                ImageURl = "https://localhost:7167/images/cat.png",
                UserId = 2,
            };

            modelBuilder.Entity<User>().HasData(u1);
            modelBuilder.Entity<User>().HasData(u2);
            modelBuilder.Entity<User>().HasData(u3);
            modelBuilder.Entity<User>().HasData(u4);
            modelBuilder.Entity<TwitterPost>().HasData(p1);
            modelBuilder.Entity<TwitterPost>().HasData(p2);
            modelBuilder.Entity<TwitterPost>().HasData(p3);

            modelBuilder.Entity("UserFollows").HasData(new { FollowerId = u1.UserId, FollowingId = u2.UserId }, new { FollowerId = u1.UserId, FollowingId = u3.UserId }, new { FollowerId = u4.UserId, FollowingId = u1.UserId });
        }
    }
}