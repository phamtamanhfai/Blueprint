$(document).ready(function () {

    $("#image").change(function (e) {
        var input = this;
        var filename = e.target.value.split('\\').pop();

        var reader = new FileReader();
        reader.onload = function (e) {
            $('.preview').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    })

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
            $('#frm_Courses').submit();
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

        if ($('#txtSubject').val() == '') {
            $('#txtSubject').focus();
            $('#txtSubject').addClass('field_error');
            $('.validate-message').text('Bạn chưa nhập đối tượng.');
            return false;
        } else {
            $('#txtSubject').removeClass('field_error');

        }

        if ($('#txtDuration').val() == '') {
            $('#txtDuration').focus();
            $('#txtDuration').addClass('field_error');
            $('.validate-message').text('Bạn chưa nhập thời lượng.');
            return false;
        } else {
            $('#txtDuration').removeClass('field_error');

        }

        if ($('#txtContent').val() == '') {
            $('#txtContent').focus();
            $('#txtContent').addClass('field_error');
            $('.validate-message').text('Bạn chưa nhập nội dung.');
            return false;
        } else {
            $('#txtContent').removeClass('field_error');

        }
        return true;
    }
})