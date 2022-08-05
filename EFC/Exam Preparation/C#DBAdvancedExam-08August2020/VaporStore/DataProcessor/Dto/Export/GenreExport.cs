namespace VaporStore.DataProcessor.Dto.Export
{
    public class GenreExport
    {
        public int Id { get; set; }
        public string Genre { get; set; }
        public GenreGameExport[] Games { get; set; }
        public int TotalPlayers { get; set; }
    }
    public class GenreGameExport
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Developer { get; set; }
        public string Tags { get; set; }
        // purchase count
        public int Players { get; set; }
    }
}
