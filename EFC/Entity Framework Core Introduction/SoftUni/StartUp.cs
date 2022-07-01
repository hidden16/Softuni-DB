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
            result = GetEmployee147(context);
            result = GetDepartmentsWithMoreThan5Employees(context);
            result = GetLatestProjects(context);
            result = IncreaseSalaries(context);
            result = GetEmployeesByFirstNameStartingWithSa(context);
            result = DeleteProjectById(context);
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
                                    .Select(x => new
                                    {
                                        AddressName = x.AddressText,
                                        TownName = x.Town.Name,
                                        EmployeeCount = x.Employees.Count
                                    })
                                    .OrderByDescending(x => x.EmployeeCount)
                                    .ThenBy(x => x.TownName)
                                    .ThenBy(x => x.AddressName)
                                    .Take(10);

            foreach (var employee in employeesTown)
            {
                sb.AppendLine($"{employee.AddressName}, {employee.TownName} - {employee.EmployeeCount} employees");
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                                .Where(x => x.EmployeeId == 147)
                                .Select(x => new
                                {
                                    FirstName = x.FirstName,
                                    LastName = x.LastName,
                                    JobTitle = x.JobTitle,
                                    Projects = x.EmployeesProjects.Select(x => x.Project.Name)
                                });
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                foreach (var project in employee.Projects.OrderBy(x => x))
                {
                    sb.AppendLine(project);
                }
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departments = context.Departments
                                .Where(x => x.Employees.Count > 5)
                                .OrderBy(x => x.Employees.Count)
                                .ThenBy(x => x.Name)
                                .Select(x => new
                                {
                                    DepartmentName = x.Name,
                                    ManagerFirstName = x.Manager.FirstName,
                                    ManagerLastName = x.Manager.LastName,
                                    Employees = x.Employees.ToList()
                                });

            foreach (var deparment in departments)
            {
                sb.AppendLine($"{deparment.DepartmentName} - {deparment.ManagerFirstName} {deparment.ManagerLastName}");
                foreach (var employee in deparment.Employees.OrderBy(x => x.FirstName).ThenBy(x => x.LastName))
                {
                    sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var projects = context.Projects
                .OrderByDescending(x => x.StartDate)
                .Select(p => new
                {
                    ProjectName = p.Name,
                    ProjectDescription = p.Description,
                    ProjectStart = p.StartDate
                })
                .Take(10)
                .OrderBy(x => x.ProjectName);
            foreach (var project in projects)
            {
                sb.AppendLine($"{project.ProjectName}");
                sb.AppendLine($"{project.ProjectDescription}");
                sb.AppendLine($"{project.ProjectStart.ToString($"M/d/yyyy h:mm:ss tt")}");
            }
            return sb.ToString().TrimEnd();
        }
        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.Department.Name == "Engineering" || e.Department.Name == "Tool Design" || e.Department.Name == "Marketing" || e.Department.Name == "Information Services")
                .OrderBy(x => x.FirstName)
                .Select(e=> new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Salary = e.Salary * 1.12m
                });

            foreach (var item in employees)
            {
                sb.AppendLine($"{item.FirstName} {item.LastName} (${item.Salary:f2})");
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e=>e.FirstName.StartsWith("Sa"))
                .OrderBy(e=>e.FirstName)
                .ThenBy(e=>e.LastName);

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:f2})");
            }
            return sb.ToString().TrimEnd();
        }
        public static string DeleteProjectById(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var projectToDelete = context.Projects.Find(2);
            var projectReferencesToDelete = context.EmployeesProjects.Where(x=>x.ProjectId == 2);
            foreach (var item in projectReferencesToDelete)
            {
                context.EmployeesProjects.Remove(item);
            }
            context.Projects.Remove(projectToDelete);
            var projects = context.Projects.Take(10);
            foreach (var project in projects)
            {
                sb.AppendLine($"{project.Name}");
            }
            return sb.ToString().TrimEnd();
        }
    }
}
