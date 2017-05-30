$(document)
    .ready(function() {
        $(function() {
            $('#datetimepicker').datetimepicker({
                format: 'YYYY-MM-DD HH:mm'
            });
        });
    });

function getFeatures() {
    var id = $("#categoryId").val();
    $.ajax({
        type: "GET",
        url: '/Items/GetFeatures',
        data: { categoryId: id },
        success: function (viewHTML) {
            $(".target").html(viewHTML);
        },
        error: function (errorData) { console.log(errorData); }
    });
}

function getImage() {
    $.ajax({
        type: "GET",
        url: '/Items/GetImage',
        success: function (data) {
            $("#item-image").attr("src", data);
        },
        error: function (errorData) { console.log(errorData); }
    });
}

function uploadImage() {
    var input = document.getElementById('file-btn'),
        data = new FormData();
    data.append("ItemImage", input.files[0]);
    $.ajax({
        type: "POST",
        url: '/Items/ImageUpload',
        data: data,
        dataType: 'json',
        contentType: false,
        processData: false,
        allways: getImage
    }).always(getImage);
}

function setSelectedFeature(featureId) {
    var selectedValue = $("#feature-" + featureId).val();
    $.ajax({
        type: "POST",
        url: '/Items/SetFeature',
        data: { featureId: featureId, slectedValue: selectedValue }
    });
}

getImage();
getFeatures();
