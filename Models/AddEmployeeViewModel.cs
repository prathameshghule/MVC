public class AddEmployeeViewModel
{
    public string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
    public List<string> Qualification { get; set; } = new List<string>();
    public IFormFile? ImagePath { get; set; } 
}
