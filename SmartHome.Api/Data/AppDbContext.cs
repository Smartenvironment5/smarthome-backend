using Microsoft.EntityFrameworkCore;

using SmartHome.Api.Models;

namespace SmartHome.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
	public DbSet<User> Users { get; set; }
}