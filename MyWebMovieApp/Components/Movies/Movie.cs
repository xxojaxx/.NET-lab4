using System.ComponentModel.DataAnnotations;
namespace MyWebMovieApp.Components.Movies
{
    public class Movie
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ReleaseDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public float? Rate { get; set; }
        public int? NumberOfRatings { get; set; } = 0;
        public string? ImageUrl { get; set; }
    }
}