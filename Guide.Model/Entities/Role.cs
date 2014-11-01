using System.Collections.Generic;

namespace Guide.Model.Entities
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public static class UserRoles
	{
		public static readonly Role User = new Role { RoleId = 1, RoleName = "User" };
		public static readonly Role ApplicationAdministrator = new Role { RoleId = 2, RoleName = "Application Administrator" };
	}

	[Table("webpages_Roles")]
	public class Role
	{
		[Key]
		public int RoleId { get; set; }

		[Required]
		[MaxLength(256)]
		public string RoleName { get; set; }

		public virtual ICollection<UserInRole> Users { get; set; }
	}
}
