function filterFrames() {
    $.ajax({
        type: "POST",
        url: '@Url.Content("~/Home/Filter")',
        data: {
            dateFrom: $("#DateFrom").val(),
            dateTo: $("#DateTo").val()
        },
        success: function (result) {
            $("#frame-content").html(result);
        }
    });
}