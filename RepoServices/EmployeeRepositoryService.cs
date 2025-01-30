using Dapper;
using System.Data;
using System.Linq;
using EmployeeManagement.Models;
using System.Collections.Generic;

namespace EmployeeManagement.RepoServices
{
    public class EmployeeRepositoryService : IEmployeeRepository
    {
        private readonly IDbConnection _dbConnection;

        public EmployeeRepositoryService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public List<Employee> GetEmployees()
        {
            var sql = @"SELECT e.EmployeeId, e.EmployeeName, e.DepartmentId, e.Salary, e.DateOfJoining, 
                               d.DepartmentId AS DepartmentId, d.DepartmentName
                        FROM Employee e
                        LEFT JOIN Department d ON e.DepartmentId = d.DepartmentId
WHERE IsDeleted = 0";

            var employeeDictionary = new Dictionary<int, Employee>();

            var employees = _dbConnection.Query<Employee, Department, Employee>(
                sql,
                (employee, department) =>
                {
                    if (!employeeDictionary.TryGetValue(employee.EmployeeId, out var currentEmployee))
                    {
                        currentEmployee = employee;
                        employeeDictionary.Add(employee.EmployeeId, currentEmployee);
                    }

                    currentEmployee.Department = department;
                    return currentEmployee;
                },
                splitOn: "DepartmentId").ToList();

            return employees;
        }



        public Employee GetEmployee(int id)
        {
            var sql = @"SELECT e.EmployeeId, e.EmployeeName, e.DepartmentId, e.Salary, e.DateOfJoining, 
                               d.DepartmentId, d.DepartmentName
                        FROM Employee e
                        INNER JOIN Department d ON e.DepartmentId = d.DepartmentId
                        WHERE e.EmployeeId = @EmployeeId AND IsDeleted =0";

            var employee = _dbConnection.Query<Employee, Department, Employee>(
                sql,
                (employee, department) =>
                {
                    employee.Department = department;
                    return employee;
                },
                new { EmployeeId = id },
                splitOn: "DepartmentId"
            ).FirstOrDefault();

            if (employee == null)
            {
                throw new KeyNotFoundException("Employee not found");
            }

            return employee;
        }


        public void AddEmployee(Employee employee)
        {
            var sql = @"INSERT INTO Employee (EmployeeName,  DepartmentId, Salary, DateOfJoining) 
                        VALUES (@Name, @DepartmentId, @Salary, @DateOfJoining)";

            _dbConnection.Execute(sql, new
            {
                Name = employee.EmployeeName,
                DepartmentId = employee.DepartmentId,
                Salary = employee.Salary,
                DateOfJoining = employee.DateOfJoining
            });
        }

        public void UpdateEmployee(int id, Employee employee)
        {
            var sql = @"UPDATE Employee 
                        SET EmployeeName = @Name,  
                            DepartmentId = @DepartmentId, 
                            Salary = @Salary, 
                            DateOfJoining = @DateOfJoining
                        WHERE EmployeeId = @EmployeeId";

            var rowsAffected = _dbConnection.Execute(sql, new
            {
                Name = employee.EmployeeName,
                DepartmentId = employee.DepartmentId,
                Salary = employee.Salary,
                DateOfJoining = employee.DateOfJoining,
                EmployeeId = id
            });

            if (rowsAffected == 0)
            {
                throw new KeyNotFoundException("Employee not found");
            }
        }

        public void DeleteEmployee(int employeeId)
        {
            var sql = @"UPDATE Employee 
                        SET IsDeleted = 1
                        WHERE EmployeeId = @EmployeeId";

            var rowsAffected = _dbConnection.Execute(sql, new { EmployeeId = employeeId });

            if (rowsAffected == 0)
            {
                throw new KeyNotFoundException("Employee not found");
            }
        }
    }
}
