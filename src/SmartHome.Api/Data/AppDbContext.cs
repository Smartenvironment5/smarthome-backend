
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartHome.Api.Models;

namespace SmartHome.Api.Data;

public class AppDbContext : IdentityDbContext<User>
{

    public DbSet<Room> Rooms => Set<Room>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        
    }
}
