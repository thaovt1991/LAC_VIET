using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MINI_PROJECT_APIs.Domain.Responses.Employee
{
    public class EmployeeView
    {
        public int EmployeeId { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public bool Deleted { get; set; }
        public string Position { get; set; }// chức vụ
        public string Title { get; set; } //chuc danh 
        public string AvatarPath { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }




    }
}
