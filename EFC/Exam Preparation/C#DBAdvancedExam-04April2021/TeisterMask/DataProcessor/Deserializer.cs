namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;

    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.Data.Models;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ImportDto;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            XmlRootAttribute rootAttribute = new XmlRootAttribute("Projects");
            XmlSerializer serializer = new XmlSerializer(typeof(ProjectImport[]), rootAttribute);
            var reader = new StringReader(xmlString);
            var projectsDto = serializer.Deserialize(reader) as ProjectImport[];
            var sb = new StringBuilder();
            List<Project> projects = new List<Project>();
            foreach (var project in projectsDto)
            {
                if (!IsValid(project))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var projectOpenDateValid = DateTime.TryParseExact(project.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime projectOpenDate);
                if (!projectOpenDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                DateTime? projectDueDate = null;
                if (!String.IsNullOrEmpty(project.DueDate))
                {
                    var projectDueDateValid = DateTime.TryParseExact(project.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dueDate);
                    if (!projectDueDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    projectDueDate = dueDate;
                }
                List<Task> tasks = new List<Task>();
                foreach (var task in project.Tasks)
                {
                    if (!IsValid(task))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    var taskOpenDateValid = DateTime.TryParseExact(task.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime taskOpenDate);
                    if (!taskOpenDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    var taskDueDateValid = DateTime.TryParseExact(task.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime taskDueDate);
                    if (!taskDueDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (taskOpenDate < projectOpenDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (projectDueDate.HasValue && taskDueDate > projectDueDate.Value)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    tasks.Add(new Task()
                    {
                        Name = task.Name,
                        ExecutionType = (ExecutionType)task.ExecutionType,
                        LabelType = (LabelType)task.LabelType,
                        DueDate = taskDueDate,
                        OpenDate = taskOpenDate,
                    });
                }
                sb.AppendLine(string.Format(SuccessfullyImportedProject, project.Name, tasks.Count));
                projects.Add(new Project()
                {
                    Name = project.Name,
                    OpenDate = projectOpenDate,
                    DueDate = projectDueDate,
                    Tasks = tasks
                });
            }
            context.Projects.AddRange(projects);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var employeesDto = JsonConvert.DeserializeObject<EmployeeImport[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<Employee> employees = new List<Employee>();
            foreach (var employee in employeesDto)
            {
                if (!IsValid(employee))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Employee employeeToAdd = new Employee()
                {
                    Phone = employee.Phone,
                    Username = employee.Username,
                    Email = employee.Email
                };
                foreach (var taskId in employee.Tasks.Distinct())
                {
                    Task task = context.Tasks.Find(taskId);

                    if (task == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    employeeToAdd.EmployeesTasks.Add(new EmployeeTask()
                    {
                        Task = task
                    });
                }
                employees.Add(employeeToAdd);
                sb.AppendLine(string.Format(SuccessfullyImportedEmployee, employee.Username, employeeToAdd.EmployeesTasks.Count));
            }
            context.Employees.AddRange(employees);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}