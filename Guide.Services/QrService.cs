using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Razor.Generator;
using System.Windows.Media.Imaging;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using Gma.QrCodeNet.Encoding.Windows.Render;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Windows.Media;
using Guide.Config.Contracts;
using Guide.Model.Entities;
using Guide.Services.Contracts;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;
using PixelFormat = System.Windows.Media.PixelFormat;


namespace Guide.Services
{
	using System.Runtime.InteropServices;

	using Color = System.Drawing.Color;
	using Image = System.Drawing.Image;

	public class QrService : IQrService
	{
		private readonly IConfigService _config;

		public QrService(IConfigService config)
		{
			_config = config;
		}

		public string GeneratePng(string text, ImageTypes size)
		{
			try
			{
				var encoder = new QrEncoder(ErrorCorrectionLevel.M);
				QrCode qrCode;
				encoder.TryEncode(text, out qrCode);


				int sizeInt = 20;
				switch (size)
				{
					case ImageTypes.Qr740x740:
						sizeInt = 20;
						break;
				}
				var wRenderer = new WriteableBitmapRenderer(
					new FixedModuleSize(sizeInt, QuietZoneModules.Four),
					Colors.Black,
					//(Color)System.Windows.Media.Color.FromRgb(66, 111, 128),
					Colors.White);
				var ms = new MemoryStream();
				wRenderer.WriteToStream(qrCode.Matrix, ImageFormatEnum.PNG, ms);



				Bitmap bmp = (Bitmap)Image.FromStream(ms);


			

				for (int x = 0; x < bmp.Width; x++)
				{
					for (int y = 0; y < bmp.Height; y++)
					{
						if (bmp.GetPixel(x, y) != Color.FromArgb(255, 255, 255, 255))
						{
							bmp.SetPixel(x, y, Color.FromArgb(66, 111, 128));
						}

					}
				}
				AForge.Imaging.Filters.OilPainting filter = new AForge.Imaging.Filters.OilPainting(9);
				filter.ApplyInPlace(bmp);

				var fileName = String.Format("{0}{1}", Guid.NewGuid(), ".png");

				var absoluteFolderPath = string.Format("{0}{1}", HttpContext.Current.Server.MapPath("~/"), _config.HttpQrImagesUrl.Replace("/", "\\"));
				if (!Directory.Exists(absoluteFolderPath))
				{
					Directory.CreateDirectory(absoluteFolderPath);
				}
				var path = string.Format("{0}\\{1}", absoluteFolderPath, fileName);

				bmp.Save(path, System.Drawing.Imaging.ImageFormat.Png);
				//using (var file = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
				//{
				//	ms.WriteTo(file);
				//}
				return fileName;
			}
			catch (Exception e)
			{
				return null;
			}
		}


	}
}
