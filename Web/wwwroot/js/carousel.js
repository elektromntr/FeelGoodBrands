(function (carousel, $) {
    carousel.addToCarousel = (e) => {
	    var guid = e.currentTarget.attributes['data-guid'].value;
	    $.ajax({
            type: "POST",
            url: e.currentTarget.attributes['data-url'].value,
            data: {
                guid: guid
            },
            success: function (result) {
	            window.location.reload();
            },
            error: function (result) {
                console.warn(result);
            }
        });
    }

    carousel.changeBrandOrder = (e) => {
        var moveUp = e.currentTarget.attributes['data-moveup'].value;
        var guid = e.currentTarget.attributes['data-guid'].value;
        $.ajax({
	        type: "POST",
            url: e.currentTarget.attributes['data-url'].value,
	        data: {
		        guid: guid,
		        moveUp: moveUp
	        },
	        success: function (result) {
                window.location.reload();
	        },
	        error: function (result) {
		        console.warn(result);
	        }
        });
    }

    $(document).ready(() => {
        $('.js-carousel-switch').on('click', function (e) {
            carousel.addToCarousel(e);
        });
        $('.js-change-order').on('click', function (e) {
            carousel.changeBrandOrder(e);
        });
    });
})(window.brand = window.brand || {}, jQuery);