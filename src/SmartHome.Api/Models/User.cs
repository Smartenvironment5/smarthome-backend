namespace SmartHome.Api.Models;

public class User
{
	public int Id { get; set; }
	public string Username { get; set; }
	public string Password { get; set; }
	public DateTime Birthdate { get; set; }
	public string Address { get; set; }
	public string Phone { get; set; }
	public string Email { get; set; }
}