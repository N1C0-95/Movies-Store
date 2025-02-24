using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Movies.Application.Model
{
    public class Movie
    {
        public required Guid Id { get; init; }
        public required string Title { get; set; }

        public string Slug => GenerateSlug();

        public float? Rating { get; set; }
        public int? UserRating { get; set; }
        public required int YearOfRelease { get; set; }

        public required List<string> Genres { get; set; } = new List<string>();

        private string GenerateSlug()
        {
            var sluggedTitle = Regex.Replace(Title, "[^0-9A-Za-z _-]", string.Empty).ToLower().Replace(" ", "-");           

            return $"{sluggedTitle}-{YearOfRelease}";
        }


       
    }
}
