$(document).ready(function () {

    var msg = $('#tmpMsg').val(); $('#tmpMsg').remove();

    if (msg) {
        $('.callout span').css("width", "0px").text(msg).animate({ "width": "100%" }, 1000)
    }

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
            $('#frm_Login').submit();
        }
    }

    function isValid() {
        if ($('#txtUsername').val() == '') {
            $('.callout span').css("width", "0px").text('Ủa, thiếu tên đăng nhập !').animate({ "width": "100%" }, 1000)
            $('#txtUsername').focus();
            $('#txtUsername').addClass('field_error');
            return false;
        } else {
            $('#txtUsername').removeClass('field_error');
        }

        if ($('#txtPassword').val() == '') {
            $('.callout span').css("width", "0px").text('Ồ, thiếu mật khẩu rồi !').animate({ "width": "100%" }, 1000)
            $('#txtPassword').focus();
            $('#txtPassword').addClass('field_error');
            return false;
        } else {
            $('#txtPassword').removeClass('field_error');

        }
        return true;
    }
})
