﻿using Guide.Model.Contracts;
using Guide.Services.Contracts;

using System.Data.Entity;
using System.Linq;

namespace Guide.Data
{
	using Guide.Data.Contracts;
	using Guide.Data.Services;
	using Guide.Model.Entities;

	public class GuideDatabaseInitializer
	{
		private readonly ITextGenerator textGenerator;
		private readonly IPredefinedService _predefinedService;

		private readonly IFullTextSearchService fullTextSearch;
		private readonly ITransliterationService _transliterationService;

		public GuideDatabaseInitializer(ITextGenerator textGenerator, IPredefinedService predefinedService, IFullTextSearchService fullTextSearch, ITransliterationService transliterationService)
		{
			this.textGenerator = textGenerator;
			_predefinedService = predefinedService;
			this.fullTextSearch = fullTextSearch;
			_transliterationService = transliterationService;
		}

		public void Seed(GuideDbContext context)
		{
			SeedRoles(context);
			SeedSightTypes(context);
			this.SeedCountries(context);
			this.SeedCities(context);
			SeedImages(context);
			SeedArticles(context);
			context.SaveChanges();
			fullTextSearch.AddUpdateList(context.Articles.Include(a => a.ArticleToSightTypes.Select(atst => atst.SightType)).ToList(), SearchType.Articles);
			fullTextSearch.AddUpdateList(context.Images.ToList(), SearchType.Images);
		}

		public void SeedRoles(GuideDbContext context)
		{
			context.Roles.Add(UserRoles.User);
			context.Roles.Add(UserRoles.ApplicationAdministrator);
		}

		public void SeedSightTypes(GuideDbContext context)
		{
			context.SightTypes.AddRange(_predefinedService.SightTypes);
		}


		public void SeedCountries(GuideDbContext context)
		{
			context.Countries.AddRange(_predefinedService.Countries);
		}

		public void SeedCities(GuideDbContext context)
		{
			context.Cities.AddRange(_predefinedService.Cities);
		}

		public void SeedImages(GuideDbContext context)
		{
			context.Images.Add(new Image() { Id = 1, Name = "wow", Size = "300x300", FileName = "1.jpg", });
			context.Images.Add(new Image() { Id = 2, Name = "good", Size = "300x300", FileName = "1t.jpg" });
			context.Images.Add(new Image() { Id = 3, Name = "test", Size = "400x300", FileName = "2.jpg" });
			context.Images.Add(new Image() { Id = 4, Name = "4", Size = "300x300", FileName = "2t.jpg" });
			context.Images.Add(new Image() { Id = 5, Name = "5", Size = "300x300", FileName = "3.jpg" });
			context.Images.Add(new Image() { Id = 6, Name = "6", Size = "300x300", FileName = "3t.jpg" });
			context.Images.Add(new Image() { Id = 7, Name = "7", Size = "500x300", FileName = "4.jpg" });
			context.Images.Add(new Image() { Id = 8, Name = "8", Size = "300x300", FileName = "4t.jpg" });
			context.Images.Add(new Image() { Id = 9, Name = "9", Size = "300x300", FileName = "5.jpg" });
			context.Images.Add(new Image() { Id = 10, Name = "10", Size = "300x300", FileName = "5t.jpg" });
		}

		public void SeedArticles(GuideDbContext context)
		{
			for (int i = 0; i < 50; i++)
			{
				var article = new Article();
				article.Id = i + 1;
				article.CityId = 1;
				article.HeaderRu = textGenerator.GenWords(3);
				article.HeaderEn = textGenerator.GenWords(3);
				article.BodyRu = textGenerator.GenSentences(25);
				article.BodyEn = textGenerator.GenSentences(25);
				article.StreetNameEn = textGenerator.GenWords(2);
				article.StreetNameRu = textGenerator.GenWords(2);
				var teaser = textGenerator.GenWords(15);
				if (teaser.Length > 100)
				{
					teaser = teaser.Substring(0, 100);
				}
				article.TeaserEn = teaser;
				teaser = textGenerator.GenWords(15);
				if (teaser.Length > 100)
				{
					teaser = teaser.Substring(0, 100);
				}
				article.TeaserRu = teaser;
				article.BuildingNumber = 1;
				article.Housing = 1;
				article.Published = true;
				article.ThumbnailId = 1;
				if (i == 0)
				{
					article.HeaderRu = HeaderRu;
					article.HeaderEn = HeaderEn;
					article.BodyRu = BodyRu;
					article.BodyEn = BodyEn;
					article.StreetNameEn = StreetNameEn;
					article.StreetNameRu = StreetNameRu;
					article.TeaserRu = TeaserRu.Substring(0, 100);
					article.TeaserEn = TeaserEn.Substring(0, 100);
					article.BuildingNumber = 4;
					article.Housing = null;
				}
				article.UrlParam = string.Format("{0}-{1}", _transliterationService.Transliterate(article.HeaderRu.ToLower()),
			article.HeaderEn.ToLower().Trim().Replace(" ", "-"));

				context.Articles.Add(article);
				context.ArticleToImages.Add(new ArticleToImage()
				{
					ArticleId = i + 1,
					ImageId = 1,
					Rank = 1,
					CaptionRu = textGenerator.GenWords(2),
					CaptionEn = textGenerator.GenWords(2)
				});

				context.ArticleToSightTypes.Add(new ArticleToSightType()
				{
					ArticleId = i + 1,
					SightTypeId = 1
				});
				context.ArticleToSightTypes.Add(new ArticleToSightType()
				{
					ArticleId = i + 1,
					SightTypeId = 2
				});
			}
		}

		private const string StreetNameRu = "Исаакиевская пл";
		private const string HeaderRu = @"Исаакиевский собор";
		private const string TeaserRu = @"Крупнейший православный храм Санкт-Петербурга. Расположен на Исаакиевской площади. Имеет статус музея; зарегистрированная в июне 1991 года церковная община имеет возможность совершать богослужение по особым дням с разрешения дирекции музея.";
		private const string BodyRu = @"<p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Исаа́киевский собо́р (официальное название — собор преподобного Исаакия Далматского) — крупнейший православный храм Санкт-Петербурга. Расположен на Исаакиевской площади. Имеет статус музея; зарегистрированная в июне 1991 года церковная община имеет возможность совершать богослужение по особым дням с разрешения дирекции музея. Освящён во имя преподобного Исаакия Далматского, почитаемого Петром I святого, так как император родился в день его памяти — 30 мая по юлианскому календарю.</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Построен в 1818—1858 годы по проекту архитектора Огюста Монферрана; строительство курировал император Николай I, председателем комиссии построения былКарл Опперман.</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Торжественное освящение 30 мая (11 июня) 1858 года нового кафедрального собора совершил митрополит Новгородский, Санкт-Петербургский, Эстляндский и Финляндский Григорий (Постников). &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'><span style='line-height: 1.428571429;'>Творение Монферрана — четвёртый по счёту храм в честь Исаакия Далматского, построенный в Санкт-Петербурге.</span></p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Высота — 101,5 м, внутренняя площадь — более 4 000 м².</p><h3 style='margin-top: 0.4em; margin-bottom: 0.5em;'>История</h3><h4 style='margin-top: 0.4em; margin-bottom: 0.5em;'>Первая Исаакиевская церковь</h4><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Первая Исаакиевская церковь. Литография с рисунка О. Монферрана. 1845 год.</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>К 1706 году на Адмиралтейских верфях работало более 10 тысяч человек, но церквей, куда могли бы они ходить, не было. Чтобы решить эту проблему, Пётр I отдал приказание найти подходящее помещение для будущей церкви. Было выбрано здание большого чертёжного амбара, расположенного с западной стороны Адмиралтейства на расстоянии 15—20 м от канала (проходящего вокруг Адмиралтейства) и в 40—50 м от берега Невы.</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Строительство как первой, так и последующих Исаакиевских церквей велось за счёт казны. Первый храм был возведён на деньги, выделенные на строительство Адмиралтейства под руководством графа Ф. М. Апраксина, для возведения шпиля церкви был приглашён голландский архитектор Х. ван Болес. Первый деревянный храм, названный Исаакиевской церковью, был освящён в 1707 году. Её простота типична для первых построек Петербурга петровского периода. Это был сруб из круглых брёвен длиной до 18 м, шириной 9 м и высотой до крыши 4—4,5 м. Внешние стены были обиты горизонтальными досками шириной до 20 см. Чтобы обеспечить хороший сход снега и дождя, крыша имела угол наклона не менее 45 градусов. Её также сделали деревянной и покрыли водонепроницаемым воско-битумным составом чёрно-коричневого цвета, которым тогда смолили днища кораблей.</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>В 1709 году Пётр I распорядился о проведении реставрационных работ в церкви. Это решение было обусловлено желанием улучшить вид самой церкви, а также решить ряд проблем, возникших по ходу эксплуатации (отмечалось, что в церкви постоянно сыро и холодно, что приводило к разрушению деревянных конструкций).</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Эта скромная церковь играла роль одной из главных в городе. Здесь 19 февраля (1 марта) 1712 года венчались Пётр I и Екатерина Алексеевна. В походном журнале есть запись за этот день:</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>В наступившем году, уже не представлявшем ожидания невзгод, Пётр I обвенчался с Екатериной Алексеевной 19-го, во вторник, на всеядной неделе. Венчание его величества совершено утром в Исаакиевском соборе. В 10 часов утра высокобрачные при залпах с бастионов Петропавловской и Адмиралтейской крепости вступили в свой зимний дом.</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>С 1723 года, по императорскому указу, только в Исаакиевском храме могли приносить присягу моряки Балтийского флота и служащие Адмиралтейства.</p><h4 style='margin-top: 0.4em; margin-bottom: 0.5em;'>Вторая Исаакиевская церковь</h4><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Вторая Исаакиевская церковь, в камне, была заложена в 1717 году, так как первая к тому времени уже обветшала. 6 (17) августа 1717 года Пётр I собственноручно заложил первый камень в основание новой церкви во имя Исаакия Далматского. Вторая Исаакиевская церковь строилась в стиле «петровского барокко» по проекту видного зодчего петровской эпохи Г. И. Маттарнови, находившегося на службе в Петербурге с 1714 года. После его кончины в 1721 году строительство возглавил Н. Ф. Гербель. Но через три года он тоже умирает, и фактически завершает строительство каменных дел мастер Яков Неупокоев</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Церковь, построенная в 1727 году в плане имела форму равноконечного греческого креста длиной 28 сажень (60.5 м). Ширина от южных дверей до северных составляла 15 сажень (32.4 м), в других местах — 9.5 сажень (20.5 м). Купол церкви опирался на четыре столба и был покрыт снаружи простым железом. Колокольня имела в высоту 12 сажень и 2 аршина (27.4 м), шпиль — 6 сажень (13 м). Купол и шпиль колокольни были увенчаны медными вызолоченными крестами высотой в 7 футов 8 дюймов и шириной 5 футов. Своды церкви были деревянные. Фасады между окон были украшены пилястрами</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>По облику она напоминала Петропавловский собор. Это сходство ещё более усиливалось благодаря стройной колокольне с часами-курантами, привезёнными Петром I из Амстердама вместе с часами для Петропавловского собора. И. П. Зарудным для церкви был сделан резной золочёный иконостас, подобный иконостасу в Петропавловском соборе</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Церковь была построена на берегу Невы, там, где сейчас стоит Медный всадник. Место было выбрано явно неудачно, вода, размывая берег воздействовала на фундамент, разрушая кладку. К тому же в мае 1735 годаудар молнии вызвал пожар в церкви и она серьёзно пострадала. Так, например, описывает положение дел в церкви кабинет-министр граф А. И. Остерман, просящий 28 мая (8 июня) 1735 года позволение у синода устроить у него в доме церковь для своей больной жены и определить туда священника:</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Церковь Исаакия Далмацкого, у которого дом мой в приходе обретается, в недавнем времени погорела и службы в ней не токмо литургии, но и вечерень, и утрень, и часов, ныне нет</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Уже в июне того же года была составлена смета на исправление церкви. На эти цели было выделено две тысячи рублей, а руководить работами был назначен майор Любим Пустошкин. В соответствующем указе было говорилось:</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Церковь Исаакия Далмацкого, как скоро возможно начать ныне, хотя только над алтарём в скорости покрыть досками, а потом и над всею тою церковию подрядить делать стропила и крыши, дабы нынче в ней могла быть служба</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>В результате ремонта отстроили заново стены и галереи, вместо железа купол был покрыт медью, а своды заменены каменными. В церкви вновь стали проходить богослужения. Но при призводстве работ стало ясно, что из-за осадки грунта храму требуется бо́льшие исправления или даже совершенная перестройка[7].</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Для обследования состояния церкви Сенат направил архитектора С. И. Чевакинского, который констатировал невозможность сохранения здания. Церковь решили разобрать и построить новую дальше от берега.</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Третий Исаакиевский собор</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Проект А. Ринальди третьего Исаакиевского собора. Литография с рисунка О. Монферрана.</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Указом Сената 15 июля 1761 года руководителем строительства нового Исаакиевского собора был назначен С. И. Чевакинский. Но начало работ затянулось. В 1762 годувступает на престол Екатерина II. Она одобрила идею воссоздать Исаакиевский собор, связанный с именем Петра I. Вскоре С. И. Чевакинский подаёт в отставку и строительство поручается архитектору А. Ринальди. В 1766 году был издан указ о начале работ на новой строительной площадке, намеченной С. И. Чевакинским. Торжественная закладка здания состоялась 8 августа 1768 года, и в память об этом событии была выбита медаль</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>По проекту А. Ринальди собор должен был иметь пять сложных по рисунку куполов и высокую стройную колокольню. Стены по всей поверхности облицовывались мрамором. Макет и чертежи проекта хранятся в музее Академии Художеств. Обстоятельства сложились так, что Ринальди не смог завершить начатую работу. Здание было доведено лишь до карниза, когда после смерти Екатерины II строительство прекратилось, и Ринальди уехал за границу</p><h4 style='margin-top: 0.4em; margin-bottom: 0.5em;'>Третий Исаакиевский собор на гравюре. 1816 год</h4><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Вступивший на престол Павел I поручил архитектору В. Бренна срочно завершить работу. Выполняя желание царя, архитектор был вынужден исказить проект Ринальди — уменьшить размеры верхней части здания и главного купола и отказаться от возведения четырёх малых куполов. Мрамор для облицовки верхней части собора был передан на строительство резиденции Павла I — Михайловского замка. Собор получился приземистым, а в художественном отношении даже нелепым — на роскошном мраморном основании высились безобразные кирпичные стены</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Это сооружение вызывало насмешки и горькую иронию современников. К примеру, приехавший в Россию после длительного пребывания в Англии флотский офицер Акимов написал эпиграмму:</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Се памятник двух царств,</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Обоим столь приличный</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>На мраморном низу</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Воздвигнут верх кирпичный</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>При попытке прикрепить листок с этим четверостишем к фасаду собора Акимов был арестован. Он дорого поплатился за своё остроумие: ему отрезали язык и сослали в Сибирь</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>В различных вариантах петербуржцы пересказывали опасную эпиграмму:</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Сей храм покажет нам,</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Кто лаской, кто бичом,</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Он начат мрамором,</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Окончен кирпичом.</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Позднее, при императоре Александре I, когда при исполнении окончательного монферрановского варианта собора стали разбирать кирпичную кладку, фольклор откликнулся на это новой эпиграммой.</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Сей храм трёх царствований изображение:</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Гранит, кирпич и разрушение.</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>30 мая 1802 года третий Исаакиевский собор был освящён.</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Современный Исаакиевский собор</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Несоответствие Исаакиевского собора парадному облику центральной части Петербурга вызвало необходимость уже в 1809 году объявить конкурс на возведение нового храма. Условием было сохранение трёх освящённых алтарей существующего собора. Программу конкурса, утверждённую Александром I, составил президент Академии художеств А. С. Строганов. В ней говорилось:</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>Изыскать средство к украшению храма… не закрывая… богатой мраморной его одежды… приискать форму купола, могущую придать величие и красоту столь знаменитому зданию… придумать способ к украшению площади, к сему храму принадлежащей, приведя окружность оной в надлежащую правильность</p><p style='margin-top: 0.4em; margin-bottom: 0.5em;'>В конкурсе приняли участие архитекторы А. Д. Захаров, А. Н. Воронихин, В. П. Стасов, Д. Кваренги, Ч. Камерон и другие. Но все проекты были отвергнуты Александром I, так как авторы предлагали не перестройку собора, а строительство нового. В 1813 году на тех же условиях опять был объявлен конкурс, и вновь ни один из проектов не удовлетворил императора. Тогда в 1816 году Александр I поручает инженеру А. Бетанкуру, председателю только что образованного «Комитета по делам строений и гидравлических работ», заняться подготовкой проекта перестройки Исаакиевского собора. Бетанкур предложил поручить проект молодому архитектору О. Монферрану, недавно до этого приехавшему из Франции в Россию. Чтобы показать своё мастерство, Монферран сделал 24 рисунка зданий различных архитектурных стилей, которые Бетанкур и представил Александру I. Императору рисунки понравились, и вскоре был подписан указ о назначении Монферрана «императорским архитектором». Одновременно ему поручалась подготовка проекта перестройки Исаакиевского собора с условием сохранить алтарную часть существующего собора.</p>";

		private const string StreetNameEn = "Saint Isaac's Townsquare";
		private const string HeaderEn = @"Saint Isaac's Cathedral";
		private const string TeaserEn = @"Largest Russian Orthodox cathedral (sobor) in the city. It is dedicated to Saint Isaac of Dalmatia, a patron saint of Peter the Great, who had been born on the feast day of that saint.";
		private const string BodyEn = @"<h3 style='font-style: normal; font-variant: normal; letter-spacing: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; box-sizing: border-box; font-family: 'Open Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif; font-weight: 300; line-height: 1.1; color: rgb(34, 34, 34); margin-top: 21px; margin-bottom: 10.5px; font-size: 26px; background-color: rgb(255, 255, 255);'><span style='box-sizing: border-box; line-height: 1.428571429;'>History</span></h3><div style='font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; box-sizing: border-box; color: rgb(34, 34, 34); font-family: 'Open Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 15px; line-height: 21.428571701049805px; background-color: rgb(255, 255, 255);'>The church on St Isaac's Square was ordered by Tsar Alexander I, to replace an earlier Rinaldiesquestructure, and was the fourth consecutive church standing at this place.[1] A specially appointed commission examined several designs, including that of the French-born architect Auguste de Montferrand (1786–1858), who had studied in the atelier of Napoleon's designer, Charles Percier. Montferrand's design was criticised by some members of the commission for the dry and allegedly boring rhythm of its four identical pedimented octastyle porticos. It was also suggested that despite gigantic dimensions, the edifice would look squat and not very impressive. The emperor, who favoured the ponderous Empire style of architecture, had to step in and solve the dispute in Montferrand's favour.</div><div style='font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; box-sizing: border-box; color: rgb(34, 34, 34); font-family: 'Open Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 15px; line-height: 21.428571701049805px; background-color: rgb(255, 255, 255);'>The cathedral took 40 years to construct, under Montferrand's direction, from 1818 to 1858.[1] Under the Sovietgovernment, the building was stripped of religious trappings. In 1931, it was turned into the Museum of the History of Religion and Atheism, the dove sculpture was removed, and replaced by a Foucault pendulum.[1][2]On April 12, 1931, the first public demonstration of the Foucault pendulum was held to visualize Copernicus’s theory. In 1937, the museum was transformed into the museum of the Cathedral, and former collections were transferred to the Museum of the History of Religion (located in the Kazan Cathedral).[3]</div><div style='font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; box-sizing: border-box; color: rgb(34, 34, 34); font-family: 'Open Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 15px; line-height: 21.428571701049805px; background-color: rgb(255, 255, 255);'>During World War II, the dome was painted over in gray to avoid attracting attention from enemy aircraft. On its top, in the skylight, a geodesical intersection point was placed, with the objective of aiding in the location of enemy cannon.</div><div style='font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; box-sizing: border-box; color: rgb(34, 34, 34); font-family: 'Open Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 15px; line-height: 21.428571701049805px; background-color: rgb(255, 255, 255);'>With the fall of communism, the museum was removed and regular worship activity has resumed in the cathedral, but only in the left-hand side chapel. The main body of the cathedral is used for services on feast days only.</div><div style='font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; box-sizing: border-box; color: rgb(34, 34, 34); font-family: 'Open Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 15px; line-height: 21.428571701049805px; background-color: rgb(255, 255, 255);'>As of 2012, the church is still a museum.</div><h3 style='font-style: normal; font-variant: normal; letter-spacing: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; box-sizing: border-box; font-family: 'Open Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif; font-weight: 300; line-height: 1.1; color: rgb(34, 34, 34); margin-top: 21px; margin-bottom: 10.5px; font-size: 26px; background-color: rgb(255, 255, 255);'>Exterior</h3><div style='font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; box-sizing: border-box; color: rgb(34, 34, 34); font-family: 'Open Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 15px; line-height: 21.428571701049805px; background-color: rgb(255, 255, 255);'>The neoclassical exterior expresses the traditional Russian-Byzantine formula of a Greek-cross ground plan with a large central dome and four subsidiary domes. It is similar to Andrea Palladio's Villa La Rotonda, with a full dome on a high drum substituted for the Villa's low central saucer dome. The design of the cathedral in general and the dome in particular later influenced the design of the Wisconsin State Capitol in Madison, Wisconsin[4] and the Cathedral in Helsinki.</div><div style='font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; box-sizing: border-box; color: rgb(34, 34, 34); font-family: 'Open Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 15px; line-height: 21.428571701049805px; background-color: rgb(255, 255, 255);'>The exterior is faced with gray and pink stone, and features a total of 112 red granite columns with Corinthian capitals, each hewn and erected as a single block: 48 at ground level, 24 on the rotunda of the uppermost dome, 8 on each of four side domes, and 2 framing each of four windows. The rotunda is encircled by a walkway accessible to tourists. 24 statues stand on the roof, and another 24 on top of the rotunda.</div><div style='font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; box-sizing: border-box; color: rgb(34, 34, 34); font-family: 'Open Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 15px; line-height: 21.428571701049805px; background-color: rgb(255, 255, 255);'>Dome</div><div style='font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; box-sizing: border-box; color: rgb(34, 34, 34); font-family: 'Open Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 15px; line-height: 21.428571701049805px; background-color: rgb(255, 255, 255);'><br style='box-sizing: border-box;'></div><div style='font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; box-sizing: border-box; color: rgb(34, 34, 34); font-family: 'Open Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 15px; line-height: 21.428571701049805px; background-color: rgb(255, 255, 255);'>Montferrand's design of the dome is based on a supporting cast ironstructure, and was only the third dome to be constructed this way.</div><div style='font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; box-sizing: border-box; color: rgb(34, 34, 34); font-family: 'Open Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 15px; line-height: 21.428571701049805px; background-color: rgb(255, 255, 255);'>The cathedral's main dome rises 101.5 metres (333 ft) and is plated with pure gold. The dome is decorated with twelve statues of angels by Josef Hermann.[5] These angels were likely the first large sculptures produced by the then novel process of electrotyping, which was an alternative to traditional bronze casting of sculptures.[6] Montferrand's design of the dome is based on a supporting cast iron structure. It was the third historical instance of cast iron cupolaafter the Leaning Tower of Nevyansk (1732) and Mainz Cathedral (1826).[7]</div><div style='font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; box-sizing: border-box; color: rgb(34, 34, 34); font-family: 'Open Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 15px; line-height: 21.428571701049805px; background-color: rgb(255, 255, 255);'>Interior</div><h3 style='font-style: normal; font-variant: normal; letter-spacing: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; box-sizing: border-box; font-family: 'Open Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif; font-weight: 300; line-height: 1.1; color: rgb(34, 34, 34); margin-top: 21px; margin-bottom: 10.5px; font-size: 26px; background-color: rgb(255, 255, 255);'>St. Isaac's Cathedral Interior.</h3><div style='font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; box-sizing: border-box; color: rgb(34, 34, 34); font-family: 'Open Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 15px; line-height: 21.428571701049805px; background-color: rgb(255, 255, 255);'>The cathedral's bronze doors are covered in reliefs, patterned after the celebrated doors of the Battistero di San Giovanni in Florence, designed by Lorenzo Ghiberti. Suspended underneath the peak of the dome is a sculpted dove representing the Holy Spirit. Internal features such ascolumns, pilasters, floor, and statue of Montferrand are composed of multicolored granites and marbles gathered from all parts of Russia. Theiconostasis is framed by eight columns of semiprecious stone: six ofmalachite and two smaller ones of lazurite. The four pediments are also richly sculpted.</div><div style='font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; box-sizing: border-box; color: rgb(34, 34, 34); font-family: 'Open Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 15px; line-height: 21.428571701049805px; background-color: rgb(255, 255, 255);'>The interior was originally decorated with scores of paintings by Karl Bryullov and other great Russian masters of the day. When these paintings began to deteriorate due to the cold, damp conditions inside the cathedral, Montferrand ordered them to be painstakingly reproduced as mosaics, a technique introduced in Russia by Mikhail Lomonosov. This work was never completed.</div><h3 style='font-style: normal; font-variant: normal; letter-spacing: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; box-sizing: border-box; font-family: 'Open Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif; font-weight: 300; line-height: 1.1; color: rgb(34, 34, 34); margin-top: 21px; margin-bottom: 10.5px; font-size: 26px; background-color: rgb(255, 255, 255);'>Technologies</h3><div style='font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; box-sizing: border-box; color: rgb(34, 34, 34); font-family: 'Open Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 15px; line-height: 21.428571701049805px; background-color: rgb(255, 255, 255);'>A model of the wooden framework used to erect the columns of St. Isaac's Cathedral is on display inside.</div><div style='font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; box-sizing: border-box; color: rgb(34, 34, 34); font-family: 'Open Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 15px; line-height: 21.428571701049805px; background-color: rgb(255, 255, 255);'>William Handyside and other engineers used a number of technological innovations in the construction of the building.[8] The portico columns were raised with the use of large wooden frameworks before the walls were erected. The building rests on 10,000 tree trunks[1] that were sunk by a large number of workers into the marshy banks upon which the cathedral is situated. The dome was gilded by a technique similar to spraypainting; the solution used included toxic mercury, the vapors of which caused the deaths of sixty workers.[9][10]). The dozen gilded statues of angels, each six metres tall, facing each other across the interior of the rotunda, were constructed usinggalvanoplastic technology,[1] making them only millimeters thick and very lightweight. St. Isaac's Cathedral represents the first use of this technique in architecture.</div><div style='font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; box-sizing: border-box; color: rgb(34, 34, 34); font-family: 'Open Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 15px; line-height: 21.428571701049805px; background-color: rgb(255, 255, 255);'>The meticulous and painstakingly detailed work on constructing the St. Isaac's Cathedral took 40 years to complete, and left an expression in the Finnish language, rakentaa kuin Iisakin kirkkoa ('to build like St. Isaac's Church'), for lengthy and never-ending megaprojects.</div><p></p>";
	}
}