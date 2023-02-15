using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class CourseEnrolmentDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public CourseEnrolmentDbContext(string connectionString) : base(new DbContextOptionsBuilder().UseSqlite(connectionString).UseLazyLoadingProxies()
        .Options)
    {
    }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(d =>
        {
            d.HasMany(x => x.Enrolments).WithOne(w => w.Student).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Course>(d =>
        {
            d.Property(w => w.CourseName).IsRequired().HasMaxLength(60);
            
            d.HasMany(w=> w.CoursesTerms).WithOne(q=>q.Course).OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<CoursesTerm>(d =>
        {
            d.HasMany(w=> w.Enrolments).WithOne(q=>q.CoursesTerm).OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<Student>(d =>
        {
            d.HasMany(w=> w.Enrolments).WithOne(q=>q.Student).OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<Term>(d =>
        {
            d.HasMany(w=> w.CoursesPerTerms).WithOne(q=>q.Term).OnDelete(DeleteBehavior.Restrict);
        });
    }
}