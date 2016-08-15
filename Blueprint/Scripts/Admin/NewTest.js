$(document).ready(function () {
    $('.button a').click(function () {
        processPost();
    })

    $('form').keydown(function (e) {
        if (e.which == 13 || e.keyCode == 13) {
            processPost();
        }
    })

    function processPost() {
        if (isValid()) {
            $('#frm_Test').submit();
        }
    }

    function isValid() {
        if ($('#txtTitle').val() == '') {
            $('#txtTitle').focus();
            $('#txtTitle').addClass('field_error');
            $('.validate-message').text('Bạn chưa nhập tiêu đề.');
            return false;
        } else {
            $('#txtTitle').removeClass('field_error');
        }

        if ($('#txtNo').val() == '') {
            $('#txtNo').focus();
            $('#txtNo').addClass('field_error');
            $('.validate-message').text('Bạn chưa nhập số câu hỏi.');
            return false;
        } else {
            $('#txtNo').removeClass('field_error');

        }
        return true;
    }
})