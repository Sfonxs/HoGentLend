$('button.voegtoe').click(function (e) {
    var button = $(this);
    var id = button.attr('id');

    var isInList = button.hasClass("voegtoe-active");

    $.post((isInList ? "/Verlanglijst/Remove" : "/Verlanglijst/Add"), { id: id }, function (data) {
        if (data.status === 'success') {
            if (isInList) {
                button.removeClass('voegtoe-active');
                button.find(".heart span").removeClass('glyphicon-heart');
                button.find(".heart span").addClass('glyphicon-heart-empty');
            }
            else {
                button.addClass('voegtoe-active');
                button.find(".heart span").removeClass('glyphicon-heart-empty');
                button.find(".heart span").addClass('glyphicon-heart');
            }

            var success = $("#success");
            success.html(data.message);
            success.slideDown();

            setTimeout(function () {
                success.slideUp();
            }, 3500);

        } else if (data.status === "error") {

            var error = $("#error");
            error.html(data.message);
            error.slideDown();

            setTimeout(function () {
                error.slideUp();
            }, 3500);
        }
    });
});