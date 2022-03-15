using Microsoft.EntityFrameworkCore;
using MINI_PROJECT_APIs.Data;
using MINI_PROJECT_APIs.Domain.Responses.Department;
using MINI_PROJECT_APIs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MINI_PROJECT_APIs.Service
{
    public class DepartmentService : IDepartmentService
    {

        private readonly AppDbContext context;

        public DepartmentService(AppDbContext context)
        {
            this.context = context;
        }


        public async Task<List<Department>> GetDepartments()
        {
            return await context.Departments.Include(d => d.Employees).Where(d => d.Deleted == false).ToListAsync();
        }
        public async Task<Department> Create(Department department)
        {
            try
            {
                context.Departments.Add(department);
                var departmentId = await context.SaveChangesAsync();
                department.Id = departmentId;
                return department;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Department> GetDepartmentById(int id)
        {
            //return await context.Departments.FindAsync(id);
            return await context.Departments.Include(d => d.Employees).Where(d => !d.Deleted && d.Id == id).FirstOrDefaultAsync();
        }



        public async Task<Department> Modify(Department department)
        {
            try
            {
                context.Attach(department);
                context.Entry<Department>(department).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return department;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Department> Remove(int id)
        {
            try
            {
                var department = await GetDepartmentById(id);
                department.Deleted = true;
                context.Attach(department);
                context.Entry<Department>(department).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return department;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<Department> Create(DepartmentService department)
        {
            throw new NotImplementedException();
        }

        public Task<Department> Modify(DepartmentService department)
        {
            throw new NotImplementedException();
        }

      
        public async Task<List<DepartmentRes>> GetDepartmentsTree(int id)
        {
            List<Department> departments = await context.Departments.Include(d => d.Employees).Where(d => d.Deleted == false).ToListAsync();

            List<DepartmentRes> departmentsRes = new List<DepartmentRes>();
            foreach (Department d in departments)
            {
                departmentsRes.Add(toDepartmentRes(d));
            }
            ///thong tin
            //
            List<DepartmentRes> hierarchy = new List<DepartmentRes>();
            //

            hierarchy = departmentsRes
                   .Where(c => c.ParentId == id)
                   .Select(c => new DepartmentRes()
                   {
                       Id = c.Id,
                       Name = c.Name,
                       ParentId = c.ParentId,
                       Children = GetChildren(departmentsRes, c.Id)
                   })
                   .ToList();

            return hierarchy;
        }

        private List<DepartmentRes> GetChildren(List<DepartmentRes> departmentsRes, int parentId)
        {
            return departmentsRes
                    .Where(c => c.ParentId == parentId)
                    .Select(c => new DepartmentRes
                    {
                        Id = c.Id,
                        Name = c.Name,
                        ParentId = c.ParentId,
                        Children = GetChildren(departmentsRes, c.Id)
                    })
                    .ToList();
        }

        public  List<int> getAllIdDepartmnet(List<DepartmentRes> departmentsRes, List<int> listId)
        {

            foreach (DepartmentRes d in departmentsRes)
            {
                if (d.Children != null)
                {
                    listId.Add(d.Id);
                    listId = getAllIdDepartmnet(d.Children, listId);
                }
                  
            }
            return listId;
        }

        private DepartmentRes toDepartmentRes(Department department)
        {
            DepartmentRes departmentRes = new DepartmentRes();
            departmentRes.Id = department.Id;
            departmentRes.Name = department.Name;
            departmentRes.ParentId = department.ParentId;
            return departmentRes;
        }
    }
}
