guide.mapService = (function ($) {
	return {
		addMapToPage: function (coordinate1, coordinate2, header,  wrapperSelector, mapDivId) {
			
			ymaps.ready(initMap);
			var map,
				myPlacemark;

			var coordinates = "";
			if (coordinate1 !== '' && coordinate2 !== '') {
				coordinates += coordinate1 + ', ' + coordinate2;
			}

			if (coordinates != '') {

				function initMap() {
					$("#" + mapDivId).width($(wrapperSelector).width());

					map = new ymaps.Map("map", {
						center: [coordinate1, coordinate2],
						zoom: 17 //,
						//	behaviors: ['default', 'scrollZoom']
					});

					myPlacemark = new ymaps.Placemark([coordinate1, coordinate2], { content: header, balloonContent: header });
					map.controls.add(new ymaps.control.ZoomControl());
					map.controls.add('typeSelector');
					map.geoObjects.add(myPlacemark);
				}

				function windowResized() {
					map.destroy();
					initMap();
				}

				var resizeTimeout;
				window.onresize = function() {
					clearTimeout(resizeTimeout);
					resizeTimeout = setTimeout(windowResized, 100);
				};
			} else {
				$(wrapperSelector).hide();
			}
		}
	
	};
})(window.jQuery);
