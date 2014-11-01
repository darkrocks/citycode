guide.imageService = (function ($) {
	return {
		prepareArticleImagesToPopup: function () {
			var images = $('img.iright, img.ileft');

			$.each(images, function (ind, item) {
				var image = $(item).clone();
				var src = $(image).attr('src');
				var alt = $(image).attr('alt');
				var a = $('<a></a>');
				a.attr('href', src);
				a.attr('title', alt);
				a.attr('data-gallery', '');
				a.html(image);

				$(item).replaceWith(a);
			});
		}
	};
})(window.jQuery);
