using SoftUni.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SoftUni
{
    public class SoftUniDatabaseCommands
    {
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            using (context)
            {
                var result = context.Employees
                    .Select(e => new 
                    {
                        e.EmployeeId,
                        e.FirstName,
                        e.LastName,
                        e.MiddleName,
                        e.JobTitle,
                        e.Salary
                    })
                    .OrderBy(x=>x.EmployeeId)
                    .ToList();

                StringBuilder sb = new StringBuilder();
                foreach (var item in result)
                {
                    sb.AppendLine($"{item.FirstName} {item.LastName} {item.MiddleName} {item.JobTitle} {item.Salary:f2}");
                }
                return sb.ToString();
            }
        }
    }
}
