function makeABet(minBet, highestBet, itemId) {
    var amount = $("#user-bet").val();

    if (amount < minBet || amount < highestBet) {
        $("#user-bet").addClass("error");
        return;
    }

    $.ajax({
        type: "POST",
        url: '/Items/MakeABet',
        data: JSON.stringify({
            amount: amount,
            itemId: itemId
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).always(function (viewHTML) {
        window.location.href= "/Items/Index";
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

function confirmRecievment(itemId) {
    $.ajax({
        type: "POST",
        url: '/Bet/ConfirmReceived',
        data: JSON.stringify({
            itemId: itemId
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).always(function (viewHTML) {
        $(".received-btn").addClass('disabled');
    });

}

$(document).ready(function () {
    $.each($(".timer"), function () {
        timer($(this));
    });
});