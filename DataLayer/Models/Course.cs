namespace DataLayer;

public class Course
{
    public int Id { get; set; }

    public string CourseName { get; set; }

    public virtual ICollection<CoursesTerm> CoursesTerms { get; set; }
}