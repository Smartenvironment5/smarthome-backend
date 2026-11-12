using Microsoft.EntityFrameworkCore;

namespace SmartHome.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
	
}