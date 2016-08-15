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
            $('#frm_Change').submit();
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

        if ($('#txtOldPassword').val() == '') {
            $('.callout span').css("width", "0px").text('Ồ, thiếu mật khẩu cũ !').animate({ "width": "100%" }, 1000)
            $('#txtOldPassword').focus();
            $('#txtOldPassword').addClass('field_error');
            return false;
        } else {
            $('#txtOldPassword').removeClass('field_error');

        }

        if ($('#txtNewPassword').val() == '') {
            $('.callout span').css("width", "0px").text('Chưa nhập mật khẩu mới !').animate({ "width": "100%" }, 1000)
            $('#txtNewPassword').focus();
            $('#txtNewPassword').addClass('field_error');
            return false;
        } else {
            $('#txtNewPassword').removeClass('field_error');

        }

        var check = true;

        $.ajax({
            url: '/Admin/CheckPass',
            data: { username: $('#txtUsername').val(), password: $('#txtOldPassword').val() },
            success: function (data) {
                if (data == '0') {
                    $('.callout span').css("width", "0px").text('Tên đăng nhập hoặc mật khẩu không đúng.').animate({ "width": "100%" }, 1000)
                    check = false;
                }
            },
            async: false
        })

        return check;
    }
})