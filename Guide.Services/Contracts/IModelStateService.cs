namespace Guide.Services.Contracts
{
	using System.Web.ModelBinding;

	public interface IModelStateService
	{
		string GetErrorsUrlString(System.Web.Mvc.ModelStateDictionary modelStates);
	}
}