namespace DataLayer;

public class Term
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public virtual ICollection<CoursesTerm> CoursesPerTerms { get; set; }
}