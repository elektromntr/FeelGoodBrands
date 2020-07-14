(function (contact, $) {

    contact.contactMe = (url) => {
        var contact =
        {
            contact: $('#contact-input').val()
        }
        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(contact),
            contentType: "application/json",
            success: function (result) {
                alert("Kontakt " +result.contact.contact+ " wysłano poprawnie");
            },
            error: function (result) {
                console.warn(result);
            }
        });
    }

})(window.contact = window.contact || {}, jQuery);