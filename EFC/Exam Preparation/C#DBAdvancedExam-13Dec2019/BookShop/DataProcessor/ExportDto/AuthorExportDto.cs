using System.Collections.Generic;
namespace BookShop.DataProcessor.ExportDto
{
    public class AuthorExportDto
    {
        public AuthorExportDto()
        {
            Books = new List<AuthorBookExportDto>();
        }
        public string AuthorName { get; set; }
        public ICollection<AuthorBookExportDto> Books { get; set; }
    }
    public class AuthorBookExportDto
    {
        public string BookName { get; set; }
        public string BookPrice { get; set; }
    }
}
