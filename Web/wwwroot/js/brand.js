(function (brand, $) {
    brand.refreshBrandPartial = () => {
        feather.replace();

        $('.js-delete-link').on('click', function (e) {
            brand.deleteLink(e);
        });
        $('.js-delete-photo').on('click', function (e) {
            brand.deletePhoto(e);
        });
    };

    brand.deleteLink = (e) => {
        $.ajax({
            type: "POST",
            url: e.currentTarget.attributes['data-url'].value,
            data: {
                linkId: e.currentTarget.attributes['id'].value
            },
            success: function (result) {
                $('section .brand-links').html(result);
                brand.refreshBrandPartial();
            },
            error: function (result) {
                console.warn(result);
            }
        });
    };

    brand.deletePhoto = (e) => {
        $.ajax({
            type: "POST",
            url: e.currentTarget.attributes['data-url'].value,
            data: {
                linkId: e.currentTarget.attributes['id'].value
            },
            success: function (result) {
                $('section .brand-photos').html(result);
                brand.refreshBrandPartial();
            },
            error: function (result) {
                console.warn(result);
            }
        });
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
                $('section .brand-links').html(result);
                brand.refreshBrandLinksPartial();
            },
            error: function (result) {
                console.warn(result);
            }
        });
    }

    $(document).ready(() => {
        brand.refreshBrandLinksPartial();
    });
})(window.brand = window.brand || {}, jQuery);