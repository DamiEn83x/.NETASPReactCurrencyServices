namespace CurrencyServiceAPI.Model
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public ICollection<Student> Students{ get; set; }

    }

    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public ICollection<Course> Courses{ get; set; }
    }
}
