using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyWebMovieApp.Components.Movies;

namespace MyWebMovieApp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{

public DbSet<MyWebMovieApp.Components.Movies.Movie> Movie { get; set; } = default!;
}
