$(document).ready(function () {
    $('#btnSearch').click(function (e) {
        $.post('/Admin/GetListUser', {
            name: $('#txtName').val(),
            role: $("#cboRole").val(),
            status: $('#cboStatus').val(),
        }, function (data) {
            $('.list-items').html(data);
        });
    })

    $('.new-account').click(function () {
        $.post('/Admin/ModalNewUser', null, function (data) {
            $('.modal-form').css({
                'top': '60px',
                'left': '50px',
                'width': '300px',
                'height': '355px',
                'display': 'block'
            }).html(data)
            $('#cboRoles').val('Admin')
            $('option[value="User"]').hide()
            $('option[value="Admin"]').show()
            $('.modal-form').addClass('show-up')
        })
    })
})