namespace Guide.Model.Model
{
    public class ImageModel
    {
	    public int ImageId { get; set; }
	    public int ArticleId { get; set; }
		public string FileName { get; set; }
        public string Url { get; set; }
	    public string Caption { get; set; }
		public int Rank { get; set; }
		public object File { get; set; }
    }
}
