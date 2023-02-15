namespace DataLayer;

public class Student
{
    public int Id { get; set; }

    public Guid StudentGlobalId { get; set; } // this is there external reference used through out the school

    public virtual  ICollection<Enrolment> Enrolments { get; set; }
}