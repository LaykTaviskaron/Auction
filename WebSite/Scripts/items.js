function getFeatures() {
    var ids = [];
    $.each($(".categories input[type='checkbox']:checked"), function () {
        ids.push($(this).val());
    });
    $.ajax({
        type: "POST",
        url: '/Items/GetFeatures',
        data: JSON.stringify({ categoryIds: ids }),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).always(function (viewHTML) {
        $(".target").removeClass("hidden");
        $(".target").html(viewHTML.responseText);
    });
}

function filter() {
    var categories = [], features = [];
    $.each($(".categories input[type='checkbox']:checked"), function () {
        categories.push($(this).val());
    });
    $.each($(".features input[type='checkbox']:checked"), function () {
        var value = $(this)[0].nextSibling.nodeValue;
        features.push(value.replace(/[^a-z0-9]/gi, ''));
    });

    $(".target").removeClass("hidden");

    $(".hottest").addClass("hidden");

    $.ajax({
        type: "POST",
        url: '/Items/Filter',
        data: JSON.stringify({ Categories: categories, Features: features }),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).always(function (viewHTML) {
        $(".item-target").html(viewHTML.responseText);
        $.each($(".timer"), function () {
            timer($(this));
        });
    });
}

function clearFilter() {
    $.each($(".categories input[type='checkbox']:checked"), function () {
        $(this).prop("checked", false);
    });

    $(".target").addClass("hidden");

    $.ajax({
        type: "POST",
        url: '/Items/Filter',
        data: JSON.stringify({ Categories: [], Features: [] }),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).always(function (viewHTML) {
        $(".item-target").html(viewHTML.responseText);
        $.each($(".timer"), function () {
            timer($(this));
        });
    });

}

function timer(element) {
    var from = element.attr('time');
    var date = from.split(' ')[0];
    var time = from.split(' ')[1];
    var to = new Date();
    to.setFullYear(date.split('.')[2], date.split('.')[1] - 1, date.split('.')[0]);
    to.setHours(time.split(':')[0]);
    to.setMinutes(time.split(':')[1]);
    var countDownDate = to.getTime();
    var x = setInterval(function () {
        var now = new Date().getTime();
        var distance = countDownDate - now;
        var days = Math.floor(distance / (1000 * 60 * 60 * 24));
        var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        var seconds = Math.floor((distance % (1000 * 60)) / 1000);

        // Display the result in the element with id="demo"
        element.text(days + "d " + hours + "h "
        + minutes + "m " + seconds + "s ");

        // If the count down is finished, write some text
        if (distance < 0) {
            clearInterval(x);
            element.text("EXPIRED");
        }
    }, 1000);
}

function confirmPayment(itemId) {
    $.ajax({
        type: "POST",
        url: '/Items/ConfirmPayment',
        data: JSON.stringify({
            itemId: itemId
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).always(function (viewHTML) {
        $(".payment-btn").addClass('disabled');
    });
}

$(document).ready(function () {
    $.each($(".timer"), function () {
        timer($(this));
    });
});

