$(document).ready(function () {
    $('#btnSearch').click(function (e) {
        $.post('/Admin/GetListFeedback', {
            name: $('#txtName').val(),
            courseId: $("#cboCourse").val(),
            fromDate: $('#txtFromDate').val(),
            toDate: $('#txtToDate').val(),
            status: $('#cboStatus').val(),
        }, function (data) {
            $('.list-items').html(data);
        });
    })
})