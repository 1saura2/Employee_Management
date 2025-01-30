using EmployeeManagement.Models;
using EmployeeManagement.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class DepartmentController : Controller
    {
        public IDepartmentRepository DepartmentRepository { get; }

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            DepartmentRepository = departmentRepository;
        }

        public ActionResult Index()
        {
            var departments = DepartmentRepository.GetDepartments();
            return View("Index", departments);
        }

        public ActionResult Details(int id)
        {
            var department = DepartmentRepository.GetDepartment(id);
            if (department == null)
            {
                return NotFound();
            }

            return View("Details", department);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Department department)
        {
            var departments = DepartmentRepository.GetDepartments();
            if (departments.Any(c => c.DepartmentName == department.DepartmentName && c.DepartmentId != department.DepartmentId))
            {
                ModelState.AddModelError("DepartmentName", "Department with this name already exists.");
            }
            if (ModelState.IsValid)
            {
                DepartmentRepository.AddDepartment(department);
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        public ActionResult Edit(int id)
        {
            var department = DepartmentRepository.GetDepartment(id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpPost]
        public ActionResult Edit(int id, Department department)
        {
            var departments = DepartmentRepository.GetDepartments();
            if (departments.Any(c => c.DepartmentName == department.DepartmentName && c.DepartmentId != department.DepartmentId))
            {
                ModelState.AddModelError("DepartmentName", "Department with this name already exists.");
            }
            if (ModelState.IsValid)
            {
                DepartmentRepository.UpdateDepartment(id, department);
                return RedirectToAction(nameof(Index));
            }

            return View(department);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                DepartmentRepository.DeleteDepartment(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
