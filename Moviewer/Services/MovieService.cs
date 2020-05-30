using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Moviewer.Models;
using Newtonsoft.Json;

namespace Moviewer.Services
{
    public class MovieService
    {
        readonly string _workingDirectory = Environment.CurrentDirectory;
        public async Task<IEnumerable<Movie>> GetMovies(int pageIndex)
        {
            var folderPath = $"{_workingDirectory}\\Data\\Page{pageIndex}";
            var dataFile = $"page{pageIndex}.json";
            var imageFolder = Path.Combine(folderPath, "Images");
            List<Movie> items;

            //read data
            using (var r = new StreamReader(Path.Combine(folderPath, dataFile)))
            {
                var json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<Movie>>(json);
            }

            //load images
            foreach (var item in items)
            {
                var imagePath = Path.Combine(imageFolder, $"{item.Title}.png");
                item.Poster = await GetPoster(imagePath);
            }
            return items;
        }

        private async Task<Bitmap> GetPoster(string posterUrl)
        {
            return await Task.Run(() =>
            {
                using var fileStream = new FileStream(posterUrl, FileMode.Open, FileAccess.Read) {Position = 0};
                var bitmap = new Bitmap(fileStream);
                return bitmap;
            });
        }
    }
}
