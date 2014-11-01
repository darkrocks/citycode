namespace Guide.Model.Entities
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("webpages_UsersInRoles")]
	public class UserInRole
	{
		[Key, Column(Order = 0)]
		public int UserId { get; set; }
		public User User { get; set; }

		[Key, Column(Order = 1)]
		[Required]
		public int RoleId { get; set; }
		public Role Role { get; set; }
	}
}
