namespace Guide.Model.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Country
    {
        public int Id { get; set; }
        [MaxLength(256)]
		[Required]
        public string NameRu { get; set; }
				[Required]
        public string NameEn { get; set; }
    }
}
