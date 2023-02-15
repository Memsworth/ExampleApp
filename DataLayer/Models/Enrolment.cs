namespace DataLayer;

public class Enrolment
{
    public int Id { get; set; }

    public int CoursesTermId { get; set; }

    public int StudentId { get; set; }
    
    public DateTime EnrolledOn { get; set; }
    

    public virtual Student Student { get; set; }
    public virtual CoursesTerm CoursesTerm { get; set; }
}