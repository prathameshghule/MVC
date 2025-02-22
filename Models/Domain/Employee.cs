namespace VCSSYSTEMS.Models.Domain
{
    public class Employee
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Qualification { get; set; }
        public string ImagePath { get; set; }
    }
}
