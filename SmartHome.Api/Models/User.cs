using Microsoft.AspNetCore.Identity;

namespace SmartHome.Api.Models;

public class User : IdentityUser
{
	public int Id { get; set; }
	public string Username { get; set; }
	public string Password { get; set; }
	public DateTime Birthdate { get; set; }
	public string Address { get; set; }
	public string Phone_number { get; set; }
	public string Email { get; set; }
}