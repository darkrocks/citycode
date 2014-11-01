namespace Guide.Model.Entities
{
    using System.Collections.Generic;

	public class User
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public virtual ICollection<UserInRole> UserInRoles { get; set; }
    }
}
