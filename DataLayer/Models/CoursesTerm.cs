namespace DataLayer;

public class CoursesTerm
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    public int TermId { get; set; }

    public virtual Course Course { get; set; }
    public virtual Term Term { get; set; }

    public virtual ICollection<Enrolment> Enrolments { get; set; }
}