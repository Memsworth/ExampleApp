using System.Linq.Expressions;
using ConsoleTableExt;
using DataLayer;
using Microsoft.EntityFrameworkCore;

public class Manager
{
    private readonly CourseEnrolmentDbContext _dbContext;

    public Manager(CourseEnrolmentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Entry()
    {
        while (true)
        {
            Console.WriteLine("Options:");
            Console.WriteLine("0 go back");
            Console.WriteLine("1 list students");
            Console.WriteLine("2 list courses");
            Console.WriteLine("3 list students on course");
            var selection = GetSelection(Enumerable.Range(0, 4));
            switch (selection)
            {
                case 0:
                    return;
                case 1:
                    await ListStudents(f => f.Id != null);
                    break;
                case 2:
                    await ListCourses();
                    break;
                case 3:
                    await ListStudentsByCourse();
                    break;
            }
        }
    }

    private async Task ListStudentsByCourse()
    {
        var courses = await _dbContext.Set<Course>().Select(x => x.Id).ToArrayAsync();
        var selection = GetSelection(courses);
        await ListStudents(e => e.Enrolments.Any(q => q.CoursesTerm.CourseId == selection));
    }

    private async Task ListCourses()
    {
        var courses = await _dbContext.Set<Course>().ToArrayAsync();
        ConsoleTableBuilder.From(courses.Select(w => new {w.CourseName, w.Id}).ToList()).ExportAndWriteLine();
    }

    private async Task ListStudents(Expression<Func<Student, bool>> expression, int skipAmount = 0)
    {
        var students = await _dbContext.Set<Student>().Where(expression).Skip(skipAmount).Take(10).ToArrayAsync();
        DisplayStudents(students);

        Console.WriteLine("Options:");
        Console.WriteLine("0 go back");
        Console.WriteLine("1 list next 10");
        Console.WriteLine("2 list prior 10");
        var selection = GetSelection(Enumerable.Range(0, 3));
        switch (selection)
        {
            case 0:
                return; 
            case 1:
                await ListStudents(expression, skipAmount + 10);
                return; 
            case 2:
                if (skipAmount < 10)
                {
                    skipAmount += 10;
                }

                await ListStudents(expression, skipAmount - 10);
                return; 
        }
    }


    private static int GetSelection(IEnumerable<int> acceptedAnswers)
    {
        Console.Write($"Please enter a number in {string.Join(",", acceptedAnswers)}:");
        var item = Console.ReadLine();
        if (!int.TryParse(item, out var output) || !acceptedAnswers.Contains(output))
        {
            Console.WriteLine("Invalid");
            return GetSelection(acceptedAnswers);
        }

        return output;
    }


    private static void DisplayStudents(IEnumerable<Student> students) =>
        ConsoleTableBuilder
            .From(students.Select(w => new
                {w.StudentGlobalId, w.Id, Courses = string.Join(",", w.Enrolments.Select(w => w.CoursesTerm.Course.CourseName))}).ToList())
            .ExportAndWriteLine();
}