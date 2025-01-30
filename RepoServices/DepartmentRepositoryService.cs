using EmployeeManagement.Models;
using Dapper;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.RepoServices
{
    public class DepartmentRepositoryService : IDepartmentRepository
    {
        private readonly IDbConnection _dbConnection;

        public DepartmentRepositoryService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public List<Department> GetDepartments()
        {
            var departmentSql = @"
                SELECT DepartmentId, DepartmentName
                FROM Department";

            var departments = _dbConnection.Query<Department>(departmentSql).ToList();
            return departments;
        }

        public Department GetDepartment(int id)
        {
            var departmentSql = @"
                SELECT DepartmentId, DepartmentName
                FROM Department
                WHERE DepartmentId = @Id";

            var department = _dbConnection.QueryFirstOrDefault<Department>(departmentSql, new { Id = id });

            if (department == null)
            {
                Console.WriteLine("Department not found");
            }

            return department;
        }

        public void AddDepartment(Department department)
        {
            try
            {
                var sql = @"INSERT INTO Department (DepartmentName) 
                            VALUES (@Name);
                            SELECT CAST(SCOPE_IDENTITY() AS INT)";

                var newDepartmentId = _dbConnection.Query<int>(sql, new { Name = department.DepartmentName }).Single();
                department.DepartmentId = newDepartmentId;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public void UpdateDepartment(int id, Department department)
        {
            var sql = @"UPDATE Department 
                        SET DepartmentName = @Name
                        WHERE DepartmentId = @Id";

            var rowsAffected = _dbConnection.Execute(sql, new { Name = department.DepartmentName, Id = id });

            if (rowsAffected == 0)
            {
                Console.WriteLine("Department not found");
            }
        }

        public void DeleteDepartment(int id)
        {
            var sql = @"DELETE FROM Department WHERE DepartmentId = @Id";

            var rowsAffected = _dbConnection.Execute(sql, new { Id = id });

            if (rowsAffected == 0)
            {
                Console.WriteLine("Department not found");
            }
        }
    }
}
