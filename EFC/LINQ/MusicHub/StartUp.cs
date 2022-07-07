namespace MusicHub
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);
            var input = int.Parse(Console.ReadLine());
            var result = ExportAlbumsInfo(context, input);
            result = ExportSongsAboveDuration(context, input);
            Console.WriteLine(result);
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            StringBuilder sb = new StringBuilder();
            var albums = context.Albums
                 .Include(x => x.Producer)
                 .Where(x => x.ProducerId == producerId)
                 .ToList();
            foreach (var album in albums.OrderByDescending(x => x.Price))
            {
                var i = 1;
                sb.AppendLine($"-AlbumName: {album.Name}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate.ToString("MM/dd/yyyy")}");
                sb.AppendLine($"-ProducerName: {album.Producer.Name}");
                sb.AppendLine($"-Songs:");
                foreach (var song in album.Songs.OrderByDescending(x => x.Name).ThenBy(x => x.Writer.Name))
                {
                    sb.AppendLine($"---#{i}");
                    sb.AppendLine($"---SongName: {song.Name}");
                    sb.AppendLine($"---Price: {song.Price:f2}");
                    sb.AppendLine($"---Writer: {song.Writer.Name}");
                    i++;
                }
                sb.AppendLine($"-AlbumPrice: {album.Price:f2}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            StringBuilder sb = new StringBuilder();
            TimeSpan t2 = new TimeSpan(0, 0, duration);
            var songs = context.Songs
                            .Include(x => x.SongPerformers)
                            .ThenInclude(x => x.Performer)
                            .Include(x => x.Writer)
                            .Where(x => x.Duration > t2)
                            .ToList();
            var performer = string.Empty;
            var i = 1;
            foreach (var song in songs.OrderBy(x => x.Name).ThenBy(x => x.Writer.Name).ThenBy(x => x.SongPerformers.OrderBy(x => x.Performer.FirstName)))
            {
                sb.AppendLine($"-Song #{i}");
                sb.AppendLine($"---SongName: {song.Name}");
                sb.AppendLine($"---Writer: {song.Writer.Name}");
                sb.AppendLine($"---Performer: {song.SongPerformers.Select(x=>$"{x.Performer.FirstName} {x.Performer.LastName}").FirstOrDefault()}");
                sb.AppendLine($"---AlbumProducer: {song.Album.Producer.Name}");
                sb.AppendLine($"---Duration: {song.Duration.ToString("c")}");
                i++;
            }
            return sb.ToString().TrimEnd();
        }
    }
}