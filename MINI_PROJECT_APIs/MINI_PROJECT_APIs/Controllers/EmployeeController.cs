using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MINI_PROJECT_APIs.Data;
using MINI_PROJECT_APIs.Domain.Request;
using MINI_PROJECT_APIs.Domain.Responses.Employee;
using MINI_PROJECT_APIs.Models;
using MINI_PROJECT_APIs.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MINI_PROJECT_APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService employeeService;
        private readonly IDepartmentService departmentService;


        public EmployeeController(IEmployeeService employeeService, IDepartmentService departmentService)
        {
            this.employeeService = employeeService;
            this.departmentService = departmentService;
        }
        // GET: api/<EmployeeController>
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            //List<Employee> employees = await employeeService.GetEmployees();
            //List<EmployeeView> employeeViews = new List<EmployeeView>();
            //foreach (Employee e in employees)
            //{
            //    EmployeeView ev = toEmployeeView(e);
            //    employeeViews.Add(ev);
            //}

            //return employeeViews;
            return await employeeService.GetEmployees();
        }

        [HttpGet]
        [Route("/api/EmployeeByDepartmentId/{id}")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployeesByDepartmnetId(int id)
        {
            return await employeeService.GetEmployeesByDerpartmentId(id);
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await employeeService.GetEmployeeById(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee([FromBody]EmployeeRequest employeeRequest)
        {
            //_context.Employees.Add(employee);
            //await _context.SaveChangesAsync();
            

            Department department = await departmentService.GetDepartmentById(employeeRequest.DepartmentId);

 
            Employee employee = new Employee();
            if (department != null)
            {
                employee.FirstName = employeeRequest.FirstName;
                employee.LastName = employeeRequest.LastName;
                employee.AvatarPath = employeeRequest.AvatarPath;
                employee.Position = employeeRequest.Position;
                employee.Title = employeeRequest.Title;
                employee.Deleted = false;
                employee.Department = department;
            }
            else
            {
                return BadRequest();
            }


            if (ModelState.IsValid)
            {
                await employeeService.Create(employee);

                return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
            }
            return BadRequest();
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, [FromBody] EmployeeRequest employeeRequest)
        {
            if (id != employeeRequest.EmployeeId)
            {
                return BadRequest();
            }

            //_context.Entry(employee).State = EntityState.Modified;
            Department department = await departmentService.GetDepartmentById(employeeRequest.DepartmentId);

            Employee employee = await employeeService.GetEmployeeById(id);
            if (department != null && employee !=null)
            {
                employee.FirstName = employeeRequest.FirstName;
                employee.LastName = employeeRequest.LastName;
                employee.AvatarPath = employeeRequest.AvatarPath;
                employee.Position = employeeRequest.Position;
                employee.Title = employeeRequest.Title;
                employee.Deleted = false;
                employee.Department = department;
            }
            else
            {
                return BadRequest();
            }

            try
            {
                await employeeService.Modify(employee);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!employeeService.EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            var employee = await employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }

            await employeeService.Remove(id);
            //employee.Deleted = true;

            //await _context.SaveChangesAsync();

            return employee;
        }

        //private bool EmployeeExists(int id)
        //{
        //    return _context.Employees.Any(e => e.Id == id);
        //}

        private EmployeeView toEmployeeView(Employee employee)
        {
            EmployeeView employeeView = new EmployeeView();

            employeeView.EmployeeId = employee.Id;
            employeeView.EmployeeFirstName = employee.FirstName;
            employeeView.EmployeeLastName = employee.LastName;
            employeeView.AvatarPath = employee.AvatarPath;
            employeeView.Deleted = employee.Deleted;
            employeeView.Position = employee.Position;
            employeeView.Title = employee.Title;
            employeeView.DepartmentId = employee.Department.Id;
            employeeView.DepartmentName = employee.Department.Name;


            return employeeView;
        }

    }
}
