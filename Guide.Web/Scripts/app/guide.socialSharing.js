guide.socialSharing = (function ($) {
	var getImageAbsoluteUrl = function (relativeUrl) {
		var arr = window.location.href.split("/");
		return arr[0] + "//" + arr[2] + "/" + relativeUrl;
	};

	return {
		getVkUrl: function(title, description, image) {
			var arr = window.location.href.split("/");
			var imageUrl = arr[0] + "//" + arr[2] + "/" + image;
			return 'http://vkontakte.ru/share.php?url=' + document.URL
				+ '&title=' + title
				+ '&description=' + description
				+ '&image=' + getImageAbsoluteUrl(image);
		},
		getFbUrl: function(url) {
			return "https://www.facebook.com/sharer/sharer.php?u=" + url;
		},
		getTwUrl: function(url, text) {
			return 'http://twitter.com/share?url=' + url + '&text=' + text;
		}
};
})(window.jQuery);
