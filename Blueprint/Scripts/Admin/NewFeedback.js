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

    function processPost() {
        if (isValid()) {
            $('#frm_Feedback').submit();
        }
    }

    function isValid() {
        if ($('#txtName').val() == '') {
            $('#txtName').focus();
            $('#txtName').addClass('field_error');
            $('.validate-message').text('Bạn chưa nhập tên học viên.');
            return false;
        } else {
            $('#txtName').removeClass('field_error');
        }

        if ($('#txtCareer').val() == '') {
            $('#txtCareer').focus();
            $('#txtCareer').addClass('field_error');
            $('.validate-message').text('Bạn chưa nhập nghề nghiệp.');
            return false;
        } else {
            $('#txtCareer').removeClass('field_error');

        }

        if ($('#txtContent').val() == '') {
            $('#txtContent').focus();
            $('#txtContent').addClass('field_error');
            $('.validate-message').text('Bạn chưa nhập ý kiến phản hồi.');
            return false;
        } else {
            $('#txtContent').removeClass('field_error');

        }

        if ($('#txtQuote').val() == '') {
            $('#txtQuote').focus();
            $('#txtQuote').addClass('field_error');
            $('.validate-message').text('Bạn chưa nhập trích dẫn.');
            return false;
        } else {
            $('#txtQuote').removeClass('field_error');

        }
        return true;
    }
})