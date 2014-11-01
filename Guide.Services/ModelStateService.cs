using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace Guide.Services
{
	using Guide.Services.Contracts;

	public class ModelStateService : IModelStateService
	{
		public string GetErrorsUrlString(ModelStateDictionary modelStates)
		{
			var sb = new StringBuilder();
			foreach (ModelState state in modelStates.Values)
			{
				foreach (ModelError error in state.Errors)
				{
					sb.Append(error.ErrorMessage);
					sb.Append(";");
				}
			}
			return sb.ToString();
		}
	}
}
