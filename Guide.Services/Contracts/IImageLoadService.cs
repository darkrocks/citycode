namespace Guide.Services.Contracts
{
	using System.Collections.Generic;

	public interface IImageLoadService
	{
		bool LoadImage(object file, bool isThumbnail, out Guide.Model.Entities.Image imageEntity, out List<string> errors);
		bool FileExist(object file);
	}
}