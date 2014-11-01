using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guide.Services
{
	using System.Drawing.Imaging;
	using System.IO;
	using System.Web;

	using Guide.Config.Contracts;
	using Guide.Services.Contracts;

	public class ImageLoadService : IImageLoadService
	{
		private readonly IConfigService configService;
		private int ThumbnailWidth = 340;
		private int MaxWidth = 520;

		public ImageLoadService(IConfigService configService)
		{
			this.configService = configService;
		}

		private Image ScaleImage(Image image, int width, int height)
		{
			var result = new Bitmap(width, height);
			result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (Graphics graphics = Graphics.FromImage(result))
			{
				graphics.DrawImage(image, 0, 0, result.Width, result.Height);
			}
			return result;
		}

		public bool FileExist(object file)
		{
			var postedFilesArray = file as HttpPostedFileBase[];

			if (postedFilesArray == null || postedFilesArray.Length == 0 || postedFilesArray[0] == null)
			{
				return false;
			}
			return true;
		}

		public bool LoadImage(object file, bool isThumbnail, out Guide.Model.Entities.Image imageEntity, out List<string> errors)
		{
			System.Drawing.Image image;
			imageEntity = new Model.Entities.Image();

			const string BadFile = "Bad file";
			const string NoFile = "File field is required";
			const string WrongExtension = "Download file extension must be .jpg/.jpeg/.png";
			const string Ext = ".jpg";

			errors = new List<string>();
			string fileName = "";

			if (file == null)
			{
				errors.Add(NoFile);
				return false;
			}
			var postedFilesArray = file as HttpPostedFileBase[];

			if (!FileExist(file))
			{
				errors.Add(NoFile);
				return false;
			}

			var extension = Path.GetExtension(postedFilesArray[0].FileName);
			if (extension == null)
			{
				errors.Add(BadFile);
				return false;
			}

			if (!extension.Equals(".jpg", StringComparison.InvariantCultureIgnoreCase) &&
				!extension.Equals(".jpeg", StringComparison.InvariantCultureIgnoreCase) &&
				!extension.Equals(".png", StringComparison.InvariantCultureIgnoreCase))
			{
				errors.Add(WrongExtension);
				return false;
			}

			try
			{
				image = System.Drawing.Image.FromStream(postedFilesArray[0].InputStream);
				if (isThumbnail)
				{
					int newHeight = (int)((double)image.Height*((double)ThumbnailWidth/(double)image.Width));
					image = ScaleImage(image, ThumbnailWidth, newHeight);
				}
				else if (image.Width > MaxWidth)
				{
					int newHeight = (int)((double)image.Height * ((double)MaxWidth / (double)image.Width));
					image = ScaleImage(image, MaxWidth, newHeight);
				}
			}
			catch (Exception)
			{
				errors.Add(BadFile);
				return false;
			}

		    fileName = String.Format("{0}{1}", Guid.NewGuid(), Ext);

			var absoluteFolderPath = string.Format("{0}{1}", HttpContext.Current.Server.MapPath("~/"), configService.HttpImagesUrl.Replace("/", "\\"));
			if (!Directory.Exists(absoluteFolderPath))
			{
				Directory.CreateDirectory(absoluteFolderPath);
			}
			var fi = new FileInfo(string.Format("{0}\\{1}", absoluteFolderPath, fileName));

			try
			{
				image.Save(fi.FullName, ImageFormat.Jpeg);
			}
			catch (Exception e)
			{
				errors.Add(BadFile);
				return false;
			}
			imageEntity.FileName = fileName;
			imageEntity.Size = string.Format("{0}x{1}", image.Width, image.Height);
			return true;
		}
	}
}
