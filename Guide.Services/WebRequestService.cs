using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Guide.Services.Contracts;

namespace Guide.Services
{
	public class WebRequestService : IWebRequestService
	{
		public dynamic GetJson(string url)
		{
			var request = WebRequest.Create(url);
			request.Credentials = CredentialCache.DefaultCredentials;
			request.ContentType = "application/json; charset=utf-8";
			var response = (HttpWebResponse)request.GetResponse();
			var dataStream = response.GetResponseStream();
			if (dataStream != null)
			{
				var reader = new StreamReader(dataStream);
				string responseFromServer = reader.ReadToEnd();
				reader.Close();
				dataStream.Close();
				response.Close();

				return Newtonsoft.Json.JsonConvert.DeserializeObject(responseFromServer);
			}
			return null;
		}
	}
}
