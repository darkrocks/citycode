using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guide.Config.Contracts;
using Guide.Services.Contracts;

namespace Guide.Services
{
    public class ArticleTeaserService : IArticleTeaserService
    {
        private readonly IConfigService _configService;

        public ArticleTeaserService(IConfigService configService)
        {
            _configService = configService;
        }

        public string GetArticleTeaser(string body)
        {
            return String.Format("{0}...", body.Substring(0, _configService.TeaserLetterCount));
        }
    }
}
