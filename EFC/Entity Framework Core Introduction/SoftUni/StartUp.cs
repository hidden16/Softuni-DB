using SoftUni.Data;
using System;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();
            string result = GetEmployeesFullInformation(context);
            result = GetEmployeesWithSalaryOver50000(context);
            result = GetEmployeesFromResearchAndDevelopment(context);
            result = AddNewAddressToEmployee(context);
            Console.WriteLine(result);
        }
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var employee in context.Employees.OrderBy(x => x.EmployeeId))
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}");
            }
            return sb.ToString();
        }
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var employee in context.Employees.Where(x=>x.Salary>50_000).OrderBy(x=>x.FirstName))
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:f2}");
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var employee in context.Employees.Include(e=>e.Department).Where(e=>e.Department.Name == "Research and Development").OrderBy(e=>e.Salary).ThenByDescending(e=>e.FirstName))
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.Department.Name} - ${employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd(); 
        }
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            Address address = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };
            context.Addresses.Add(address);

            context.Employees
                .First(x => x.LastName == "Nakov")
                .Address = address;
            context.SaveChanges();
            var employeeAddresses = context.Employees
            .OrderByDescending(employee => employee.Address.AddressId)
            .Take(10)
            .Select(employee => employee.Address.AddressText);
            foreach (var employeeAddress in employeeAddresses)
            {
                sb.AppendLine(employeeAddress);
            }
            return sb.ToString().TrimEnd();
        }
    }
}
