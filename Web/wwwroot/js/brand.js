(function (brand, $) {
    brand.refreshBrandLinksPartial = () => {
        feather.replace();

        $('.js-delete').on('click', function (e) {
            brand.deleteLink(e);
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
                brand.refreshBrandLinksPartial();
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