namespace Theatre.DataProcessor.ExportDto
{
    public class TheatreExport
    {
        public string Name { get; set; }
        public sbyte Halls { get; set; }
        public decimal TotalIncome { get; set; }
        public TheatreTicketExport[] Tickets { get; set; }

    }
    public class TheatreTicketExport
    {
        public decimal Price { get; set; }
        public sbyte RowNumber { get; set; }
    }
}
