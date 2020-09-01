(function (contact, $) {

    contact.contactMe = (url) => {
        debugger;
        var contact =
        {
            Email: $('#contact-email').val(),
            Phone: $('#contact-phone').val(),
            Name: $('#contact-name').val(),
            Content: $('#contact-content').val(),
        }
        if (contact.Phone !== "" && (!$.isNumeric(contact.Phone) || contact.Phone.length !== 9))
        {
            alert("Niepoprawny format numeru telefonu");
            return false;
        }
        var emailRegEx = /^\w+([-+.'][^\s]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;

        if (!emailRegEx.test(contact.Email)) {
            alert("Niepoprawny format email");
            return false;
        };

        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(contact),
            contentType: "application/json",
            success: function (result) {
                alert("Thank you! We'll contact you soon.");
            },
            error: function (result) {
                console.warn(result);
            }
        });
    }

})(window.contact = window.contact || {}, jQuery);