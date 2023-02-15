using Microsoft.EntityFrameworkCore;

namespace DataLayer.QuickDataBuilder;

public static class BuildData
{
    private static string[] Courses = new[]
        {"Computer Science", "Art", "English lit 1", "Maths", "Science", "German", "French", "English spoken", "Sociology"};

    public static async Task<CourseEnrolmentDbContext> GetDbContext(string dbName)
    {
        if (File.Exists(dbName))
        {
            Console.Write("Would you like a fresh db Y/N:");
            var selection = Console.ReadLine().ToLower();
            if (selection == "y")
            {
                File.Delete(dbName);
            }
        }
  
        var courseEnrolmentDb = new CourseEnrolmentDbContext($"Data Source={dbName}");

        await courseEnrolmentDb.Database.MigrateAsync();
        await FillDbWithData(courseEnrolmentDb);

        return courseEnrolmentDb;
    }
    
    private static async Task FillDbWithData(CourseEnrolmentDbContext dbContext)
    {
        if (!await dbContext.Set<Course>().AnyAsync())
        {
            await dbContext.Set<Course>().AddRangeAsync(Courses.Select(x => new Course {CourseName = x}));
            await dbContext.SaveChangesAsync();
        }

        if (!await dbContext.Set<Term>().AnyAsync())
        {
            var dateRange = Enumerable.Range(0, 8).Select(x => new DateTime(DateTime.Now.Year, 1, 1).AddMonths(x * 3));
            await dbContext.Set<Term>().AddRangeAsync(dateRange.Select(x => new Term {StartDate = x, EndDate = x.AddMonths(3)}));
            await dbContext.SaveChangesAsync();
        }

        if (!await dbContext.Set<CoursesTerm>().AnyAsync())
        {
            var courses = await dbContext.Set<Course>().ToArrayAsync();
            var terms = await dbContext.Set<Term>().ToArrayAsync();

            foreach (var course in courses)
            {
                foreach (var term in terms)
                {
                    if (Random.Shared.Next(0, 10) > 8) continue;
                    await dbContext.Set<CoursesTerm>().AddAsync(new CoursesTerm
                    {
                        CourseId = course.Id,
                        TermId = term.Id
                    });
                }
            }

            await dbContext.SaveChangesAsync();
        }


        if (!await dbContext.Set<Student>().AnyAsync())
        {
            await dbContext.AddRangeAsync(Enumerable.Range(0, 150).Select(x => new Student
            {
                StudentGlobalId = Guid.NewGuid()
            }));
            await dbContext.SaveChangesAsync();
        }

        if (!await dbContext.Set<Enrolment>().AnyAsync())
        {
            var students = await dbContext.Set<Student>().ToArrayAsync();
            var coursesTerms = await dbContext.Set<CoursesTerm>().ToArrayAsync();
            var lookUp = coursesTerms.GroupBy(x => x.Term.StartDate).ToDictionary(x => x.Key, x => x.ToArray());

            foreach (var student in students)
            {
                if (Random.Shared.Next(0, 30) > 28) continue;

                var item = lookUp.Skip(Random.Shared.Next(0, lookUp.Count)).FirstOrDefault();

                var courseTerms = item.Value.OrderBy(x => Guid.NewGuid().GetHashCode()).Take(Random.Shared.Next(1, 4)).ToArray();
                await dbContext.AddRangeAsync(courseTerms.Select(x => new Enrolment
                {
                    StudentId = student.Id,
                    CoursesTermId = x.Id,
                    EnrolledOn = item.Key.AddDays(-(Random.Shared.Next(0, 188)))
                }));
                await dbContext.SaveChangesAsync();
            }
        }
        await dbContext.SaveChangesAsync();
    }
}