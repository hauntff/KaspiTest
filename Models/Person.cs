using System.ComponentModel.DataAnnotations;

namespace KaspiTest.Models
{
	public class Person
	{
		public int Id { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
		public string Role { get; set; }
	}
}
