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

    $.ajax({
        type: "POST",
        url: '/Items/Filter',
        data: JSON.stringify({ Categories: categories, Features: features }),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).always(function (viewHTML) {
        $(".item-target").html(viewHTML.responseText);
    });
}
