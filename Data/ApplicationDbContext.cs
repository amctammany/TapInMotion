using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TapInMotion.Models;

namespace TapInMotion.Data;

    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<TapInMotion.Models.Student> Student { get; set; } = default!;

    public DbSet<TapInMotion.Models.School> School { get; set; } = default!;

    public DbSet<TapInMotion.Models.Vehicle> Vehicle { get; set; } = default!;

    public DbSet<TapInMotion.Models.Station> Station { get; set; } = default!;

    public DbSet<TapInMotion.Models.Administrator> Administrator { get; set; } = default!;
    public DbSet<TapInMotion.Models.Trip> Trip { get; set; } = default!;
}
