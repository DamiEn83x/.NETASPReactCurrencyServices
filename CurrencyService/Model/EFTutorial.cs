namespace CurrencyServiceAPI.Model
{
    public class Course
    {
        public string Name { get; set; }
        public ICollection<Student> Students{ get; set; }

    }

    public class Student
    {
        public string Name { get; set; }
        public ICollection<Course> Courses{ get; set; }
    }
}
