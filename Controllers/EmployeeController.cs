using ManagerWebApplication.DAL;
using ManagerWebApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace ManagerWebApplication.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly Employee_DAL _dal;
        private readonly IWebHostEnvironment _hostEnvironment;

        public EmployeeController(Employee_DAL dal, IWebHostEnvironment hostEnvironment)
        {
            _dal = dal;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                employees = _dal.GetAllEmployees();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            List<Employee> departments = _dal.GetAllDepartments();
            ViewData["Departments"] = departments;

            List<Employee> positions = _dal.GetAllPositions();
            ViewData["Positions"] = positions;

            return View(employees);
        }

        [HttpGet]
        public IActionResult GetEmployeeList()
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                employees = _dal.GetAllEmployees();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return PartialView("_EmployeeListPartial", employees);
        }

        [HttpGet]
        public IActionResult GetEmployeeDetails(string id)
        {
            Employee? employee = _dal.GetEmployeeByID(id);
            if (employee != null)
            {
                return PartialView("_EmployeeDetailsPartial", employee);
            }
            return Json(new { error = "Employee not found" });
        }

        [HttpGet]
        public IActionResult CreateEmployee()
        {
            List<Employee> departments = _dal.GetAllDepartments();
            ViewData["Departments"] = departments;
            return PartialView("_CreateEmployeePartial");
        }

        [HttpPost]
        public IActionResult Create(Employee employee, IFormFile imageFile)
        {
            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Image file name
                    string imageName = employee.ID + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(imageFile.FileName);

                    // Path to save image
                    string imagePath = Path.Combine(_hostEnvironment.WebRootPath, "hr_images/avatar", imageName);

                    // Save image to path
                    using (var imageStream = new FileStream(imagePath, FileMode.Create))
                    {
                        imageFile.CopyTo(imageStream);
                    }

                    // Set URL to save
                    employee.AvaName = "/hr_images/avatar/" + imageName;
                }
                else
                {
                    // No new image selected, retain the existing URL
                    employee.AvaName = "/hr_images/avatar/ava-default.png";
                }

                if (ModelState.IsValid || (employee.ID != null && employee.FirstName != null && employee.LastName != null && employee.PositionID != null))
                {
                    _dal.CreateEmployee(employee);
                    ViewBag.SuccessMessage = "Inserted";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ImportEmployees(IFormFile eFile)
        {
            if (eFile != null && eFile.Length > 0)
            {
                string eFileExtension = Path.GetExtension(eFile.FileName);
                string tempDirectory = Path.GetTempPath();
                string tempFileName = Guid.NewGuid().ToString() + eFileExtension;
                string eFilePath = Path.Combine(tempDirectory, tempFileName);
                using (var stream = new FileStream(eFilePath, FileMode.Create))
                {
                    eFile.CopyTo(stream);
                }

                var employees = _dal.ReadEmployeeFile(eFilePath);
                _dal.InsertEmployees(employees);

                // Delete the file after importing
                System.IO.File.Delete(eFilePath);

                return Json(new { success = true });
            }
            return Json(new { success = false, message = "No file uploaded or the file is empty." });
        }

        [HttpPost]
        public async Task<IActionResult> UploadEmployeeImagesFolder(List<IFormFile> employeeImages)
        {
            var uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "hr_images/avatar");

            foreach (var image in employeeImages)
            {
                if (image != null && image.Length > 0 && new[] { ".jpg", ".jpeg", ".png", ".gif" }.Contains(Path.GetExtension(image.FileName).ToLowerInvariant()))
                {
                    var imageName = Path.GetFileName(image.FileName);
                    var imagePath = Path.Combine(uploadPath, imageName);

                    using (var imageStream = new FileStream(imagePath, FileMode.Create))
                    {
                        await image.CopyToAsync(imageStream);
                    }
                }
            }
            return Json(new { message = "Uploaded!" });
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 52428800)]
        public IActionResult UploadEmployeeImagesZip()
        {
            var files = Request.Form.Files;
            if (files.Count == 0)
            {
                return BadRequest("No file uploaded or the file is empty.");
            }

            var tempPath = Path.Combine(_hostEnvironment.WebRootPath, "temp");
            var uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "hr_images/avatar");

            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    if (Path.GetExtension(formFile.FileName).Equals(".zip", StringComparison.OrdinalIgnoreCase))
                    {
                        var tempZipPath = Path.Combine(_hostEnvironment.WebRootPath, "temp", formFile.FileName);
                        using (var stream = new FileStream(tempZipPath, FileMode.Create))
                        {
                            formFile.CopyTo(stream);
                        }
                        // Open zip file
                        using (var zip = ZipFile.OpenRead(tempZipPath))
                        {
                            foreach (var entry in zip.Entries)
                            {
                                if (new[] {".jpg", ".jpeg", ".png", ".gif"}.Contains(Path.GetExtension(entry.FullName).ToLowerInvariant()))
                                {
                                    var extractPath = Path.Combine(uploadPath, Path.GetFileName(entry.FullName));
                                    entry.ExtractToFile(extractPath, overwrite: true);
                                }
                            }
                        }
                        System.IO.File.Delete(tempZipPath);
                    }
                    else
                    {
                        if (new[] {".jpg", ".jpeg", ".png", ".gif"}.Contains(Path.GetExtension(formFile.FileName).ToLowerInvariant()))
                        {
                            var imagePath = Path.Combine(uploadPath, formFile.FileName);
                            using (var imageStream = new FileStream(imagePath, FileMode.Create))
                            {
                                formFile.CopyTo(imageStream);
                            }
                        }
                    }
                }
            }
            return Json(new { message = "Uploaded!" });
        }

        [HttpGet]
        public IActionResult Delete(string ID)
        {
            try
            {
                _dal.DeleteEmployee(ID);
                TempData["successMessage"] = "Deleted!";
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return RedirectToAction("GetEmployeeList");
        }

        [HttpPost]
        public IActionResult BulkDelete(List<string> IDs)
        {
            try
            {
                if (IDs == null || IDs.Count == 0)
                {
                    TempData["errorMessage"] = "No employee selected";
                    return RedirectToAction("Index");
                }
                
                foreach (var ID in IDs)
                {
                    _dal.DeleteEmployee(ID);
                }
                return Json(new { success = true, message = "Deleted!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetEmployeeEdit(string ID)
        {
            Employee? employee = _dal.GetEmployeeByID(ID);
            List<Employee> departments = _dal.GetAllDepartments();
            ViewData["Departments"] = departments;

            List<Employee> positions = _dal.GetAllPositions();
            ViewData["Positions"] = positions;
            return PartialView("_EditEmployeePartial", employee);
        }

        [HttpPost]
        public IActionResult Update(Employee employee, IFormFile imageFileUpdate)
        {
            try
            {
                if (imageFileUpdate != null && imageFileUpdate.Length > 0)
                {
                    // Image file name
                    string imageName = employee.ID + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(imageFileUpdate.FileName);

                    // Path to save image
                    string imagePath = Path.Combine(_hostEnvironment.WebRootPath, "hr_images/avatar", imageName);

                    // Save image to path
                    using (var imageStream = new FileStream(imagePath, FileMode.Create))
                    {
                        imageFileUpdate.CopyTo(imageStream);
                    }

                    // Set URL to save
                    employee.AvaName = "/hr_images/avatar/" + imageName;
                }
                else
                {
                    // No new image selected, retain the existing URL
                    Employee? existingEmployee = _dal.GetEmployeeByID(employee.ID!);
                    if (existingEmployee != null)
                    {
                        employee.AvaName = existingEmployee.AvaName;
                    }
                }

                if (ModelState.IsValid || (employee.ID != null && employee.FirstName != null && employee.LastName != null && employee.PositionID != null))
                {
                    _dal.UpdateEmployee(employee);
                    ViewBag.SuccessMessage = "Updated";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetPositionsByDepartment(string departmentID)
        {
            List<Employee> positionsbyDepartment = new List<Employee>();
            try
            {
                positionsbyDepartment = _dal.GetPositionByDepartment(departmentID);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return Json(positionsbyDepartment);
        }
    }
}
