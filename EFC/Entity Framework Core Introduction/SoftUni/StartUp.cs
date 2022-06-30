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
            result = GetEmployeesInPeriod(context);
            result = GetAddressesByTown(context);
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
            foreach (var employee in context.Employees.Where(x => x.Salary > 50_000).OrderBy(x => x.FirstName))
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:f2}");
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var employee in context.Employees.Include(e => e.Department).Where(e => e.Department.Name == "Research and Development").OrderBy(e => e.Salary).ThenByDescending(e => e.FirstName))
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
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            DateTime smallDate = new DateTime(2001, 1, 1);
            DateTime bigDate = new DateTime(2003, 1, 1);
            var employees = context.Employees
                .Where(x => x.EmployeesProjects.Any(x => x.Project.StartDate >= smallDate && x.Project.StartDate <= bigDate))
                .Select(x => new
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName,
                    Projects = x.EmployeesProjects.Select(p => new
                    {
                        ProjectName = p.Project.Name,
                        ProjectStart = p.Project.StartDate,
                        ProjectEnd = p.Project.EndDate
                    })
                })
                .Take(10);
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");
                foreach (var project in employee.Projects)
                {
                    var startDate = project.ProjectStart.ToString("M/d/yyyy h:mm:ss tt");
                    var endDate = project.ProjectEnd.HasValue ? project.ProjectEnd.Value.ToString("M/d/yyyy h:mm:ss tt") : "not finished";
                    sb.AppendLine($"--{project.ProjectName} - {startDate} - {endDate}");
                }
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employeesTown = context.Addresses
                                    .Select(x=> new
                                    {
                                        AddressName = x.AddressText,
                                        TownName = x.Town.Name,
                                        EmployeeCount = x.Employees.Count
                                    })
                                    .OrderByDescending(x=>x.EmployeeCount)
                                    .ThenBy(x=>x.TownName)
                                    .ThenBy(x=>x.AddressName)
                                    .Take(10);

            foreach (var employee in employeesTown)
            {
                sb.AppendLine($"{employee.AddressName}, {employee.TownName} - {employee.EmployeeCount} employees");
            }
            return sb.ToString().TrimEnd();
        }
    }
}
