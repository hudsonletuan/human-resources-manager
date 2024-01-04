using ManagerWebApplication.DAL;
using ManagerWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

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
            Employee employee = _dal.GetEmployeeByID(id);
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
                    string imagePath = Path.Combine(_hostEnvironment.WebRootPath, "images", imageName);

                    // Save image to path
                    using (var imageStream = new FileStream(imagePath, FileMode.Create))
                    {
                        imageFile.CopyTo(imageStream);
                    }

                    // Set URL to save
                    employee.UrlImage = "/images/" + imageName;
                }
                else
                {
                    // No new image selected, retain the existing URL
                    employee.UrlImage = "/images/ava-default.png";
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

        [HttpGet]
        public IActionResult GetEmployeeEdit(string ID)
        {
            Employee employee = _dal.GetEmployeeByID(ID);
            List<Employee> departments = _dal.GetAllDepartments();
            ViewData["Departments"] = departments;

            List<Employee> positions = _dal.GetAllPositions();
            ViewData["Positions"] = positions;
            return PartialView("_EditEmployeePartial", employee);
        }

        [HttpPost]
        public IActionResult Update(Employee employee, IFormFile imageFile)
        {
            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Image file name
                    string imageName = employee.ID + "-" + employee.FirstName + "-" + employee.LastName + Path.GetExtension(imageFile.FileName);

                    // Path to save image
                    string imagePath = Path.Combine(_hostEnvironment.WebRootPath, "images", imageName);

                    // Save image to path
                    using (var imageStream = new FileStream(imagePath, FileMode.Create))
                    {
                        imageFile.CopyTo(imageStream);
                    }

                    // Set URL to save
                    employee.UrlImage = "/images/" + imageName;
                }
                else
                {
                    // No new image selected, retain the existing URL
                    Employee existingEmployee = _dal.GetEmployeeByID(employee.ID);
                    employee.UrlImage = existingEmployee.UrlImage;
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
