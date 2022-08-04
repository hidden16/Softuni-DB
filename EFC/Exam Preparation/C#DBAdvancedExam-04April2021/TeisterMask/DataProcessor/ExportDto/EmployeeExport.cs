using System;
using TeisterMask.Data.Models.Enums;

namespace TeisterMask.DataProcessor.ExportDto
{
    public class EmployeeExport
    {
        public string Username { get; set; }
        public EmployeeTaskExport[] Tasks { get; set; }
    }
    public class EmployeeTaskExport
    {
        public string TaskName { get; set; }
        public string OpenDate { get; set; }
        public string DueDate { get; set; }
        public string LabelType { get; set; }
        public string ExecutionType { get; set; }
    }
}
