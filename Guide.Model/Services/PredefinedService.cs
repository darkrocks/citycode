using System.Collections.Generic;
using Guide.Model.Contracts;
using Guide.Model.Entities;

namespace Guide.Model.Services
{
	using Guide.Model.Model;

	public class PredefinedService : IPredefinedService
	{
		private List<Country> _countries;
		private List<City> _cities;
		private List<SightType> _sightTypes;
		private List<ImportanceModel> _importances;
		public List<Country> Countries
		{
			get
			{
				return _countries ?? (_countries = new List<Country>
				{
					new Country() { Id = 1, NameRu = "Россия", NameEn = "Russia" }
				});
			}
		}

		public List<City> Cities
		{
			get
			{
				return _cities ?? (_cities = new List<City>
				{
					new City
					{
						Id = 1,
						CountryId = 1,
						NameRu = "Санкт-Петербург",
						NameEn = "Saint Petersburg",
						ThreeLetterCode = "spb"
					}
				});
			}
		}
		public List<SightType> SightTypes
		{
			get
			{
				return _sightTypes ?? (_sightTypes = new List<SightType>
				{
					new SightType
					{
						Id = 1,
						NameRu = "Храм",
						NameEn = "Church",
						PluralNameRu = "Храмы",
						PluralNameEn = "Churches",
						Published = true
					},
					new SightType
					{
						Id = 2,
						NameRu = "Музей",
						NameEn = "Museum",
						PluralNameRu = "Музеи",
						PluralNameEn = "Museums",
						Published = true
					},
					new SightType
					{
						Id = 3,
						NameRu = "Памятник",
						NameEn = "Monument",
						PluralNameRu = "Памятники",
						PluralNameEn = "Monuments",
						Published = true
					},
					new SightType
					{
						Id = 4,
						NameRu = "Здание",
						NameEn = "Building",
						PluralNameRu = "Здания",
						PluralNameEn = "Buildings",
						Published = true
					},
					new SightType
					{
						Id = 5,
						NameRu = "Вокзал",
						NameEn = "Railway Station",
						PluralNameRu = "Вокзалы",
						PluralNameEn = "Railway stations",
						Published = true
					},
					new SightType
					{
						Id = 6,
						NameRu = "Метрополитен",
						NameEn = "Underground",
						PluralNameRu = "Станции метро",
						PluralNameEn = "Underground stations",
						Published = true
					},
					new SightType
					{
						Id = 7,
						NameRu = "Мост",
						NameEn = "Bridge",
						PluralNameRu = "Мосты",
						PluralNameEn = "Bridges",
						Published = true
					},
					new SightType
					{
						Id = 8,
						NameRu = "Площадь",
						NameEn = "Square",
						PluralNameRu = "Площади",
						PluralNameEn = "Squares",
						Published = true
					},
						new SightType
					{
						Id = 9,
						NameRu = "Улица",
						NameEn = "Street",
						PluralNameRu = "Улицы",
						PluralNameEn = "Streets",
						Published = true
					},
						new SightType
					{
						Id = 10,
						NameRu = "Парк",
						NameEn = "Park",
						PluralNameRu = "Парки",
						PluralNameEn = "Parks",
						Published = true
					},
					new SightType
					{
						Id = 11,
						NameRu = "Водоем",
						NameEn = "Pond",
						PluralNameRu = "Водоемы",
						PluralNameEn = "Ponds",
						Published = true
					},
					new SightType
					{
						Id = 12,
						NameRu = "Панорама",
						NameEn = "Panorama",
						PluralNameRu = "Панорамы",
						PluralNameEn = "Panoramas",
						Published = true
					},
					new SightType
					{
						Id = 13,
						NameRu = "Театр",
						NameEn = "Theatre",
						PluralNameRu = "Театры",
						PluralNameEn = "Theatres",
						Published = true
					},
				});
			}
		}

		public List<ImportanceModel> ImportanceModels
		{
			get
			{
				return _importances ?? (_importances = new List<ImportanceModel>
				{
					new ImportanceModel
					{
						Value = 1,
						NameRu = "Не обязательно",
						NameEn = "Not necessarily"
					},
					new ImportanceModel
					{
						Value = 2,
						NameRu = "Важно",
						NameEn = "Important"
					},
					new ImportanceModel
					{
						Value = 3,
						NameRu = "Нужно видеть",
						NameEn = "Must see"
					},
				});
			}
		}
	}
}