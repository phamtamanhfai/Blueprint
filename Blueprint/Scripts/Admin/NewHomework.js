$(document).ready(function () {

    $(document).on('change', '.upload', function (e) {
        var input = this;

        var html = "";
        $.each(e.target.files, function (i, o) {
            var name = this.name;
            html += "<span class='node'>" + name + "<span class='del' data-order='" + $('.node').length + "'></span></span>"
            if (i == e.target.files.length - 1) {
                $("#divFile").append(html);
            }
        })
        PreNewUpload($('.upload').length + 1);
    })


    $(document).on('click', '.del', function () {
        $(this).parent().hide();
        $('#delfile').val($('#delfile').val() + "," + $(this).attr('data-order'))
    })

    $('#frm_Homework').submit(function () {
        $('.upload').last().remove();

    })

    function PreNewUpload(intCount) {
        var html = "<input type='file' class='upload' id='file" + intCount + "' name='file" + intCount + "' multiple='multiple'/>";
        $('#files').append(html)
        $('#add-more').attr('for', 'file' + intCount);
    }

    $('.button a').click(function () {
        processPost();
    })

    function processPost() {
        if (isValid()) {
            $('#frm_Homework').submit();
        }
    }

    function isValid() {
        if ($('#divFile').find(".node:visible").length == 0) {
            $('#divFile').addClass('field_error');
            $('.validate-message').text('Bạn phải chọn ít nhất 1 file.');
            return false;
        } else {
            $('#divFile').removeClass('field_error');
        }

        if ($('#txtTitle').val() == '') {
            $('#txtTitle').focus();
            $('#txtTitle').addClass('field_error');
            $('.validate-message').text('Bạn chưa nhập tiêu đề');
            return false;
        } else {
            $('#txtTitle').removeClass('field_error');
        }

        return true;
    }
})