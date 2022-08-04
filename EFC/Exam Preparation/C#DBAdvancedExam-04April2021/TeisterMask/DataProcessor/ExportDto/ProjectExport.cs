using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Project")]
    public class ProjectExport
    {
        [XmlAttribute]
        public int TasksCount { get; set; }
        [XmlElement]
        public string ProjectName { get; set; }
        [XmlElement]
        public string HasEndDate { get; set; }
        [XmlArray]
        public ProjectTaskExport[] Tasks { get; set; }
    }
    [XmlType("Task")]
    public class ProjectTaskExport
    {
        public string Name { get; set; }
        public string Label { get; set; }
    }
}
