namespace CurrencyServiceAPI.Model
{
    public class Course
    {
        public Course()
        {
            this.Students = new HashSet<Student>();
        }
        public int CourseId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Student> Students{ get; set; }

    }

    public class Student
    {
        public Student()
        {
            this.Courses = new HashSet<Course>();
        }
        public int StudentId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Course> Courses{ get; set; }
    }
}
