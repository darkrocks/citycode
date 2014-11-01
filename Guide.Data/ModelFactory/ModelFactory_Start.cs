// possibly outdated

using Guide.Services;
using Guide.Services.Contracts;

namespace Guide.Data.ModelFactory
{
    using Guide.Data.Contracts;

    public partial class ModelFactory : IModelFactory
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILanguageService _languageService;
        private readonly IUrlService _urlService;
        private readonly IArticleTeaserService _articleTeaserService;
	    private readonly IGeolocationService _geolocationService;
	    private readonly ITransliterationService _transliterationService;

	    private readonly IArticleListTitleService articleListTitleService;

	    public ModelFactory(IUnitOfWork unitOfWork, 
            ILanguageService languageService, 
            IUrlService urlService,
            IArticleTeaserService articleTeaserService,
			IGeolocationService geolocationService,
			ITransliterationService transliterationService,
			IArticleListTitleService articleListTitleService)
        {
            _unitOfWork = unitOfWork;
            _languageService = languageService;
            _urlService = urlService;
            _articleTeaserService = articleTeaserService;
	        _geolocationService = geolocationService;
		    _transliterationService = transliterationService;
		    this.articleListTitleService = articleListTitleService;
        }
    }
}