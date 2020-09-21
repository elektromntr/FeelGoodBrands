(function (brand, $) {
    brand.refreshBrandPartial = () => {
        feather.replace();
        $('.js-delete-link').on('click', function (e) {
            brand.deleteLink(e);
        });
        $('.js-delete-photo').on('click', function (e) {
            brand.deletePhoto(e);
        });
        console.log('refreshBrandPartial');
    };

    brand.deleteLink = (e) => {
        var c = confirm("Chcesz usunąć link?");
        if (c === true) {
            $.ajax({
                type: "POST",
                url: e.currentTarget.attributes['data-url'].value,
                data: {
                    linkId: e.currentTarget.attributes['id'].value
                },
                success: function(result) {
                    $('#brand-links').html(result);
                    brand.refreshBrandPartial();
                },
                error: function(result) {
                    console.warn(result);
                }
            });
        } else {
            return false;
        }
    };

    brand.deletePhoto = (e) => {
        var c = confirm("Chcesz usunąć zdjęcie?");
        if (c === true) {
            $.ajax({
                type: "POST",
                url: e.currentTarget.attributes['data-url'].value,
                data: {
                    photoId: e.currentTarget.attributes['id'].value
                },
                success: function(result) {
                    debugger;
                    $('.brand-photos').html(result);
                    brand.refreshBrandPartial();
                },
                error: function(result) {
                    console.warn(result);
                }
            });
        } else {
            return false;
        }
    };

    brand.addBrandMedia = (url) => {
        var mediaType = $('#mediatype').val(),
            link = $('#mediaurl').val(),
            brandId = $('#brandId').val()
        $.ajax({
            type: "POST",
            url: url,
            data: {
                mediaType: mediaType,
                link: link,
                brandId: brandId
            },
            success: function (result) {
                $('#brand-links').html(result);
                brand.refreshBrandPartial();
            },
            error: function (result) {
                console.warn(result);
            }
        });
    }

    brand.changeBrandOrder = (e) => {
        var moveUp = e.currentTarget.attributes['data-moveup'].value;
        var guid = e.currentTarget.attributes['data-brandguid'].value;
        $.ajax({
	        type: "POST",
            url: e.currentTarget.attributes['data-url'].value,
	        data: {
		        brandGuid: guid,
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
        brand.refreshBrandPartial();
        $('.js-change-order').on('click', function (e) {
	        brand.changeBrandOrder(e);
        });
    });
})(window.brand = window.brand || {}, jQuery);