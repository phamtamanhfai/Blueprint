$(document).ready(function () {

    $('.modal-form-close').click(function () {
        ResetForm()
    })

    $('#btnNewAccount').click(function () {
        if ($('#txtUserName').val() == '') {
            $('.modal-validate-message').addClass('reveal');
            $('#txtUserName').addClass('field_error').focus();
            return;
        }
        else {
            $.post('/Admin/AddNewAccount', {
                userName: $('#txtUserName').val(),
                role: $('#cboRoles').val(),
                studentId: Number($('#btnNewAccount').attr('student-id'))
            }, function (data) {
                toastr.success("Tạo tài khoản thành công")
                if (window.location.href.indexOf("Client") > 0) {
                    var str = '<span>Đang hoạt động</span> <br />' +
                                '<button class="Quit">Nghỉ học</button>';
                    $('#' + $('#btnNewAccount').attr('student-id')).find('td').eq(1).html(str)
                }
                ResetForm();
            })
        }
    });

    function ResetForm() {
        $('.validate-message').removeClass('reveal');
        $('#txtUserName').removeClass('field_error');
        $('.modal-form').removeClass('show-up')
        $('#txtUserName').val('')
        $('#btnNewAccount').attr('student-id', 0)
    }
})