var renderChart = function (json) {
    var divNode = document.getElementById("timeSeriesChart");
    while (divNode.firstChild) {
        divNode.removeChild(divNode.firstChild);
    }

    new Morris.Line({
        element: "timeSeriesChart",
        data: json,
        xkey: "Date",
        ykeys: ["Value"],
        labels: ["Value"],

        fillOpacity: 0.6,
        hideHover: "auto",
        resize: false,
        smooth: true,
        lineColors: ["#5cb85c", "#428bca"],
        behaveLikeLine: true
    });
}

var comparisonChart = function (json) {
    var divNode = document.getElementById("comparisonChart");
    while (divNode.firstChild) {
        divNode.removeChild(divNode.firstChild);
    }

    new Morris.Line({
        element: "comparisonChart",
        data: json,
        xkey: "Date",
        ykeys: ["ProfitFund", "ProfitInv"],
        labels: ["ProfitFund", "ProfitInv"],

        fillOpacity: 0.6,
        hideHover: "auto",
        resize: false,
        smooth: true,
        lineColors: ["#5cb85c", "#428bca"],
        behaveLikeLine: true
    });
}


var parseJson = function (json) {
    var parseDate = function (x) { return new Date(parseInt(x.substr(6))).toISOString().slice(0, 10); };
    for (i = 0; i < json.length; i++) {
        json[i].Date = parseDate(json[i].Date);
    }

    return json;
}