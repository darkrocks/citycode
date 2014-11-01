namespace Guide.Data.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.Mvc;

	using Guide.Config;
	using Guide.Data.Contracts;
	using Guide.Model.Entities;
	using Guide.Model.Model;
	using Guide.Services.Contracts;

	public class AlphabeticalPage
	{
		public string Letter { get; set; }
		public string Url { get; set; }
		public bool Active { get; set; }
		public bool Enabled { get; set; }
		public List<ArticleModel> Articles { get; set; }
	}

	public class AlphabeticPaginationService : IAlphabeticPaginationService
	{
		private readonly ILanguageService _languageService;

		private readonly IModelFactory modelFactory;

		public AlphabeticPaginationService(ILanguageService languageService, IModelFactory modelFactory)
		{
			this._languageService = languageService;
			this.modelFactory = modelFactory;
		}

		private List<string> _englishAlphabet;
		private IEnumerable<string> EnglishAlphabet
		{
			get
			{
				if (this._englishAlphabet == null)
				{
					this._englishAlphabet = new List<string>();

					this._englishAlphabet.AddRange(new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" });
				}
				return this._englishAlphabet;
			}
		}

		private List<string> _russianAlphabet;
		private IEnumerable<string> RussianAlphabet
		{
			get
			{
				if (this._russianAlphabet == null)
				{
					this._russianAlphabet = new List<string>();
					this._russianAlphabet.AddRange(new[] { "А", "Б", "В", "Г", "Д", "Е", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я" });
				}
				return this._russianAlphabet;
			}
		}

		public List<AlphabeticalPage> GetPageLinks(List<Article> articles, string cityCode, string activeLetter)
		{
			var result = new List<AlphabeticalPage>();
			var url = new UrlHelper(HttpContext.Current.Request.RequestContext);
			switch (this._languageService.CurrentLanguage)
			{
				case Languages.En:
					result.AddRange(from letter1 in this.EnglishAlphabet
									let articles1 = articles.Where(a => a.City.ThreeLetterCode == cityCode && a.HeaderEn.StartsWith(letter1, StringComparison.InvariantCultureIgnoreCase)).OrderBy(a => a.HeaderEn).ToList().
									Select(a => this.modelFactory.Create(a)).ToList()
									select 
									new AlphabeticalPage 
									{ 
										Letter = letter1, 
										Url = url.Action("List", "Articles", 
										new 
										{ 
											language = Languages.En.ToString(), 
											id = cityCode, 
											letter = letter1 
										}), 
										Active = letter1 == activeLetter,
										Articles = articles1, 
										Enabled = articles1.Any() 
									});
					break;
				case Languages.Ru:
					result.AddRange(from letter1 in this.RussianAlphabet
									let articles1 = articles.Where(a => a.City.ThreeLetterCode == cityCode && a.HeaderRu.StartsWith(letter1, StringComparison.InvariantCultureIgnoreCase)).OrderBy(a => a.HeaderRu).ToList().
									Select(a => this.modelFactory.Create(a)).ToList()
									select
									new AlphabeticalPage
									{
										Letter = letter1,
										Url = url.Action("List", "Articles",
										new
										{
											language = Languages.Ru.ToString(),
											id = cityCode,
											letter = letter1
										}),
										Active = letter1 == activeLetter,
										Articles = articles1,
										Enabled = articles1.Any()
									});
					break;
			}
			return result;
		}
	}
}
