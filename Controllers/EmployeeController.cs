using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VCSSYSTEMS.Data;
using VCSSYSTEMS.Models;
using VCSSYSTEMS.Models.Domain;

namespace VCSSYSTEMS.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly MVCDemoDbContext mvcDemoDbContext;

        public EmployeeController(MVCDemoDbContext mvcDemoDbContext)
        {
            this.mvcDemoDbContext = mvcDemoDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await mvcDemoDbContext.Employees.ToListAsync();
            return View(employees);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid ID)         {
            var employee = await mvcDemoDbContext.Employees.FindAsync(ID);

            if (employee != null)
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", employee.ImagePath);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                mvcDemoDbContext.Employees.Remove(employee);
                await mvcDemoDbContext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            string uniqueFileName = "default.jpg"; 

            if (addEmployeeRequest.ImagePath != null && addEmployeeRequest.ImagePath.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(addEmployeeRequest.ImagePath.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await addEmployeeRequest.ImagePath.CopyToAsync(fileStream);
                }
            }

            var employee = new Employee()
            {
                ID = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                DateOfBirth = addEmployeeRequest.DateOfBirth,
                Gender = addEmployeeRequest.Gender,
                Qualification = string.Join(", ", addEmployeeRequest.Qualification), 
                ImagePath = uniqueFileName 
            };

            await mvcDemoDbContext.Employees.AddAsync(employee);
            await mvcDemoDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> View(Guid Id)
        {
            var employee = await mvcDemoDbContext.Employees.FirstOrDefaultAsync(x => x.ID == Id);

            if (employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    ID = employee.ID,
                    Name = employee.Name,
                    Email = employee.Email,
                    DateOfBirth = employee.DateOfBirth,
                    Gender = employee.Gender,
                    Qualification = employee.Qualification,
                    ExistingImagePath = employee.ImagePath 
                };

                return View("View", viewModel);
            }

            return RedirectToAction("Index");
        }



        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel viewModel)
        {
            var employee = await mvcDemoDbContext.Employees.FindAsync(viewModel.ID);

            if (employee != null)
            {
                employee.Name = viewModel.Name;
                employee.Email = viewModel.Email;
                employee.DateOfBirth = viewModel.DateOfBirth;
                employee.Gender = viewModel.Gender;
                employee.Qualification = viewModel.Qualification;

                if (viewModel.ImagePath != null && viewModel.ImagePath.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.ImagePath.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.ImagePath.CopyToAsync(fileStream);
                    }
                    if (!string.IsNullOrEmpty(employee.ImagePath))
                    {
                        string oldFilePath = Path.Combine(uploadsFolder, employee.ImagePath);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    employee.ImagePath = uniqueFileName;
                }

                await mvcDemoDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }


    }
}
