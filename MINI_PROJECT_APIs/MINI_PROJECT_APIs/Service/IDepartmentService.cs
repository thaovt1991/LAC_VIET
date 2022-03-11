using MINI_PROJECT_APIs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MINI_PROJECT_APIs.Service
{
    public interface IDepartmentService
    {

        Task<List<Department>> GetDepartments();
        Task<Department> Create(DepartmentService department);
        Task<Department> GetDepartmentById(int id);
        Task<Department> Modify(DepartmentService department);
        Task<Department> Remove(int id);
    }
}
