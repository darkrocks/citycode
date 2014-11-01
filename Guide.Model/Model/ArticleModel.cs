using System.Collections.Generic;

namespace Guide.Model.Model
{
    public class ArticleModel
    {
        public int ArticleId { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public CityModel City { get; set; }
        public string Teaser { get; set; }
        public string ThumbImageUrl { get; set; }
		public string StreetName { get; set; }
		public int? BuildingNumber { get; set; }
		public int? Housing { get; set; }
		public string Litera { get; set; }
		public short Importance { get; set; }
		public List<ImageModel> Images { get; set; }
		public string GoogleMapUrl { get; set; }
		public string Coordinate1 { get; set; }
		public string Coordinate2 { get; set; }
		public string Address { get; set; }
		public string FoursquareId { get; set; }
	    public string FoursquareUrl { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public string Website { get; set; }
		public string UrlParam { get; set; }
    }
}
