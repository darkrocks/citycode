using Guide.Model.Entities;

namespace Guide.Model.Model
{
    public class CityModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
		public string ThreeLetterCode { get; set; }
        public CountryModel Country { get; set; }
    }
}
