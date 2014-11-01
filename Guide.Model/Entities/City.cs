namespace Guide.Model.Entities
{
    using System.ComponentModel.DataAnnotations;

	public class City
    {
        public int Id { get; set; }
        [MaxLength(256)]
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public string ThreeLetterCode { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
