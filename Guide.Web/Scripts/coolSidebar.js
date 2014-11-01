// sidebar hacks
$(function () {
	
	var sidebarNavOriginalHeight = 50;
	$.each($('#sidebar-nav').children(), function (index, element) {
		sidebarNavOriginalHeight += $(element).height();
	});
	function setWrappersHeights() {
		var maxHeight = $(window).height();
		var bodyHeight = $('.container.body-content').height() + 49; // + navbar height
		if (bodyHeight > maxHeight) {
			maxHeight = bodyHeight;
		}
		if (sidebarNavOriginalHeight > maxHeight) {
			maxHeight = sidebarNavOriginalHeight;
		}

		$("#page-content-wrapper").height(maxHeight);
		$("#sidebar-nav").height(maxHeight);
	}

	setWrappersHeights();
	window.addEventListener('orientationchange', function () {
		setWrappersHeights();
	});

	$(window).resize(function () {
		setWrappersHeights();
	});
});