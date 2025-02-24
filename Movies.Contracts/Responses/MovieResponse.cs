﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Contracts.Responses
{
    public class MovieResponse 
    {
        public Guid Id { get; init; }
        public required string Title { get; init; }

        public float? Rating { get; init; }
        public int? UserRating { get; init; }
        public required string Slug { get; init; }
        public required int YearOfRealease { get; init; }
        public required IEnumerable<string> Genres { get; init; } = Enumerable.Empty<string>();


    }
}
