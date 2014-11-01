namespace Guide.Services.Contracts
{
	public interface IWebRequestService
	{
		dynamic GetJson(string url);
	}
}