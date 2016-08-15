$(document).ready(function () {

    $('.new-account').click(function () {
        var id = $(this).parents('tr').attr('id');
        $.post('/Admin/ModalNewUser', null, function (data) {
            $('.modal-form').html(data)
            $('#btnNewAccount').attr('student-id', id)
            $('.modal-form').addClass('show-up')
            $('#cboRoles').val('User')
            $('option[value="User"]').show()
            $('option[value="Admin"]').hide()
        })

    })

    $(document).on('click', '.Quit', function (e) {
        var id = $(this).parents('tr').attr('id')
        $.post('/Admin/GetStudent', { id: id }, function (data) {
            if (data.Status != 'Q') {
                $.post('/Admin/SetStudent', { id: id, status: 'Q' }, function (data) {
                    toastr.success('Cập nhật trạng thái thành công.')
                })
            }

            var str = '<span>Đã nghỉ</span> <br />' +
                      '<button class="Restore">Kích hoạt lại</button>';
            $(e.target).parent().html(str);
        });
    })

    $(document).on('click', '.Restore', function (e) {
        var id = $(this).parents('tr').attr('id')
        $.post('/Admin/GetStudent', { id: id }, function (data) {
            if (data.Status != 'A') {
                $.post('/Admin/SetStudent', { id: id, status: 'A' }, function (data) {
                    toastr.success('Cập nhật trạng thái thành công.')
                })
            }

            var str = '<span>Đang hoạt động</span> <br />' +
                      '<button class="Quit">Nghỉ học</button>';
            $(e.target).parent().html(str);
        });
    })
})