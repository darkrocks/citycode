guide.foursquareCommentsService = (function ($) {
	return {
		render: function (oauthToken, foursquareid, list1Selector, list2Selector, elementToHideSelector) {
			if (foursquareid) {
				var foursquareAPI = "https://api.foursquare.com/v2/venues/" + foursquareid + "?oauth_token=" + oauthToken + "&v=20140401";

				$.getJSON(foursquareAPI)
					.done(function (data) {
						var count = data.response.venue.tips.groups[0].items.length;
						$.each(data.response.venue.tips.groups[0].items, function (i, item) {
							var li = $("<li></li>").attr("class", "media");

							var a = $("<a></a>").attr("class", "pull-left")
								.attr("target", "_blank")
								.attr("href", "https://foursquare.com/user/" + item.user.id);
							var img = $("<img></img>").attr("class", "media-object").attr("src", item.user.photo.prefix + '64x64' + item.user.photo.suffix).appendTo(a);

							var div = $("<div></div>").attr("class", "media-body");
							var name = item.user.firstName;
							if (item.user.lastName) {
								name += ' ' + item.user.lastName;
							}
							var h4 = $("<h4></h4>").attr("class", "media-heading").attr('style', 'float:left;margin-right:10px').html(name).appendTo(div);

							var date = new Date(item.createdAt * 1000);
							var curr_date = date.getDate();
							var curr_month = date.getMonth() + 1; //Months are zero based
							var curr_year = date.getFullYear();
							var span = $("<i></i>").attr('style', 'float:left;').html(curr_date + "-" + curr_month + "-" + curr_year).appendTo(div);

							div.html(div.html() + '<div style ="clear:both;" />' + '<div>' + item.text + '</div>');


							a.appendTo(li);
							if (item.photo && item.photourl) {
								var a2 = $("<a></a>").attr("target", "_blank")
								.attr("href", item.photourl);
								var img2 = $("<img></img>").attr("class", "media-object").attr("style", "float: right;").attr("src", item.photo.prefix + '96x96' + item.photo.suffix).appendTo(a2);
								a2.appendTo(li);
							}
							div.appendTo(li);
							if (i < (count / 2)) {
								li.appendTo($(list1Selector));
							} else {
								li.appendTo($(list2Selector));
							}
						});
					});
			} else {
				$(elementToHideSelector).hide();
			}
		}
	};
})(window.jQuery);
