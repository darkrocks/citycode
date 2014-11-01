using System.ComponentModel.DataAnnotations.Schema;

namespace Guide.Model.Entities
{
	using System.ComponentModel.DataAnnotations;

	public enum ImageTypes
	{
		Article = 1,
		Qr740x740 = 2
	}

	public class Image
	{
		public Image()
		{
			ImageType = (short) ImageTypes.Article;
		}
		public int Id { get; set; }
		public string FileName { get; set; }
		public string Name { get; set; }
		public string Size { get; set; }
		public short ImageType { get; set; }
		[NotMapped]
		public string Url { get; set; }
		[NotMapped]
		public object File { get; set; }

	}
}
