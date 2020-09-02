(function ($) {
    "use strict";

    var changeLanguage = function(e) {
        $.ajax({
            type: "POST",
            url: e.currentTarget.attributes['data-url'].value,
            data: {
                language: e.currentTarget.attributes['data-language'].value
            },
            success: function() {
                console.warn("Session language has been changed");
                location.reload();
            },
            error: function(result) {
                console.warn("Session language hasn't been changed:" + result);
            }
        });
    };

    $('.js-language-change').click(function (e) {
        changeLanguage(e);
    });
})(jQuery);