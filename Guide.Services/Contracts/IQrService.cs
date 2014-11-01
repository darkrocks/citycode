using Guide.Model.Entities;

namespace Guide.Services.Contracts
{
	public interface IQrService
	{
		string GeneratePng(string text, ImageTypes imageType);
	}
}