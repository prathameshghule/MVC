namespace VCSSYSTEMS.Models
{
    public class UpdateEmployeeViewModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Qualification { get; set; }
        public string ExistingImagePath { get; set; } 

        public IFormFile? ImagePath { get; set; } 
    }
}
