$(function () {
    $('.date-picker').datepicker({ dateFormat: 'yy-mm-dd' });
})


var formatDate = function (date) {
    return new Date(date).toISOString().slice(0, 10);
}