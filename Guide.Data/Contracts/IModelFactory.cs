using Guide.Model.Entities;
using Guide.Model.Model;

namespace Guide.Data.Contracts
{
    public interface IModelFactory
    {
	    Article Gridify(Article entity);
		Image Gridify(Image entity);
		Image Gridify(Image entity, ImageTypes imageType = ImageTypes.Article);
        ArticleModel Create(Article entity);
        CountryModel Create(Country entity);
        CityModel Create(City entity);
		SightTypeModel Create(City city, SightType entity);
	    ArticleEditModel CreateArticleEditModel(Article entity);
	    ArticleEditInlineModel CreateArticleEditInlineModel(Article entity);
	    Article Parse(ArticleEditModel model);
	    ImageModel Create(Image entity, int articleId, int rank, string captionRu, string captionEn);
	    Image Parse(ImageModel model);
    }
}