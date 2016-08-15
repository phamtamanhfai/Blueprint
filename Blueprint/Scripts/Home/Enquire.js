$(document).ready(function () {
    var msg = $('#tmpMsg').val(); $('#tmpMsg').remove();

    if (msg != '') {
        toastr.success(msg);
    }

    $('.submit').click(function () {
        if (isValid()) {
            $('form').submit();
        }
    });

    function isValid() {
        if ($('#txtName').val() == '') {
            $('#txtName').focus();
            $('#txtName').addClass('field_error');
            $('.validate-message').text('Bạn chưa nhập Họ tên.').show();
            return false;
        } else {
            $('#txtName').removeClass('field_error');
        }

        if ($('#txtAddress').val() == '') {
            $('#txtAddress').focus();
            $('#txtAddress').addClass('field_error');
            $('.validate-message').text('Bạn chưa nhập Địa chỉ.').show();
            return false;
        } else {
            $('#txtAddress').removeClass('field_error');
        }

        if ($('#txtPhone').val() == '') {
            $('#txtPhone').focus();
            $('#txtPhone').addClass('field_error');
            $('.validate-message').text('Bạn chưa nhập Số điện thoại.').show();
            return false;
        } else {
            $('#txtPhone').removeClass('field_error');
        }

        if ($('#txtEmail').val() == '') {
            $('#txtEmail').focus();
            $('#txtEmail').addClass('field_error');
            $('.validate-message').text('Bạn chưa nhập Email.').show();
            return false;
        } else {
            $('#txtEmail').removeClass('field_error');
        }

        if ($('#txtBirthDate').val() == '') {
            $('#txtBirthDate').focus();
            $('#txtBirthDate').addClass('field_error');
            $('.validate-message').text('Bạn chưa nhập Ngày sinh.').show();
            return false;
        } else {
            $('#txtBirthDate').removeClass('field_error');
        }

        if ($('#cboCourse').val() == '-1') {
            $('#cboCourse').focus();
            $('#cboCourse').addClass('field_error');
            $('.validate-message').text('Bạn chưa chọn khóa học.').show();
            return false;
        } else {
            $('#cboCourse').removeClass('field_error');
        }

        if ($('#txaPurpose').val() == '') {
            $('#txaPurpose').focus();
            $('#txaPurpose').addClass('field_error');
            $('.validate-message').text('Bạn chưa nhập Mục đích học.').show();
            return false;
        } else {
            $('#txaPurpose').removeClass('field_error');
        }
        return true;
    }
});