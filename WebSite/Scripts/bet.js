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
        console.log(viewHTML);
    });
}