using EmployeeManagement.Models;
using EmployeeManagement.RepoServices;
using EmployeeManagement.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        public IEmployeeRepository EmployeeRepository { get; }
        public IDepartmentRepository DepartmentRepository { get; }

        public EmployeeController(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
        {
            EmployeeRepository = employeeRepository;
            DepartmentRepository = departmentRepository;
        }

        public IActionResult Index(string name, int? departmentId, int pageNumber = 1, int pageSize = 10)
        {
            var employees = EmployeeRepository.GetEmployees();

            if (!string.IsNullOrEmpty(name))
            {
                employees = employees.Where(e => e.EmployeeName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (departmentId.HasValue && departmentId.Value > 0)
            {
                employees = employees.Where(e => e.DepartmentId == departmentId.Value).ToList();
            }

            var totalCount = employees.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var paginatedEmployees = employees.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.AllDepartments = DepartmentRepository.GetDepartments();
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;

            return View(paginatedEmployees);
        }



        [HttpPost]
        public ActionResult Index(IFormCollection collection)
        {
            string name = collection["name"];
            var employees = EmployeeRepository.GetEmployees();

            if (!string.IsNullOrEmpty(name))
            {
                employees = employees.Where(e => e.EmployeeName.Contains(name)).ToList();
            }

            return View(employees);
        }

        public ActionResult Details(int id)
        {
            var employee = EmployeeRepository.GetEmployee(id);
            if (employee == null)
            {
                return NotFound();
            }

            var departmentName = DepartmentRepository.GetDepartments()
                .Where(d => d.DepartmentId == employee.DepartmentId)
                .Select(d => d.DepartmentName)
                .FirstOrDefault();

            ViewBag.Department = departmentName;
            return View("Details", employee);
        }

        public ActionResult Create()
        {
            ViewBag.AllDepartments = DepartmentRepository.GetDepartments();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            var employees = EmployeeRepository.GetEmployees();

            if (employees.Any(c => c.EmployeeName == employee.EmployeeName))
            {
                ModelState.AddModelError("EmployeeName", "Employee with this name already exists.");
            }

            if (ModelState.IsValid)
            {
                EmployeeRepository.AddEmployee(employee);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AllDepartments = DepartmentRepository.GetDepartments();
            return View(employee);
        }

        public ActionResult Edit(int id)
        {
            var employee = EmployeeRepository.GetEmployee(id);
            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.AllDepartments = DepartmentRepository.GetDepartments();
            return View(employee);
        }

        [HttpPost]
        public ActionResult Edit(int id, Employee employee)
        {
            var employees = EmployeeRepository.GetEmployees();


            if (employees.Any(c => c.EmployeeName == employee.EmployeeName && c.EmployeeId != id))
            {
                ModelState.AddModelError("EmployeeName", "Employee with this name already exists.");
            }

            if (ModelState.IsValid)
            {
                EmployeeRepository.UpdateEmployee(id, employee);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AllDepartments = DepartmentRepository.GetDepartments();
            return View(employee);
        }


        public ActionResult Delete(int id)
        {
            try
            {
                EmployeeRepository.DeleteEmployee(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
