$(document).ready(function () {
    var CorrectAns = 0;
    var ActualNoQuestions = Number($('#tmpActualNoQuestions').val()); $('#tmpActualNoQuestions').remove();
    var ModelID = Number($('#tmpModelID').val()); $('#tmpModelID').remove();

    if(ActualNoQuestions == 1){
        $('#btnNext').hide()
        $('#btnFinal').show()
    }

    $('#btnNext').click(function () {

        if ($(this).hasClass('disable')) {
            return;
        }

        if (!isValid()) {
            toastr.info('Bạn chưa chọn đáp án.')
            return;
        }

        var counter = Number($('.counter').text());
        $('.blob').eq(counter - 1).removeClass('active')
        $('.blob').eq(counter).fadeIn(500).promise().done(function () {
            $('.blob').eq(counter).addClass('active').removeAttr('style');
            $('.counter').text(counter + 1);
            $('.disable').removeClass('disable');
            if (counter == (ActualNoQuestions - 1)) {
                $('#btnNext').hide()
                $('#btnFinal').show()
            };
        })
    })

    $('#btnPrev').click(function () {

        if ($(this).hasClass('disable')) {
            return;
        }

        var counter = Number($('.counter').text());
        $('.blob').eq(counter - 1).removeClass('active')
        $('.blob').eq(counter - 2).fadeIn(500).promise().done(function () {
            $('.blob').eq(counter - 2).addClass('active').removeAttr('style');
            $('.counter').text(counter - 1);
            $('#btnNext').show()
            $('#btnFinal').hide()
            if ($('.counter').text() == '1') {
                $('#btnPrev').addClass('disable');
            };
        })
    })

    $('#btnFinal').click(function(){

        if ($(this).hasClass('disable')) {
            return;
        }

        if (!isValid()) {
            toastr.info('Bạn chưa chọn đáp án.')
            return;
        }

        $(this).addClass('disable')

        $.ajax({
            url: '/Home/CheckResult',
            data: { testId: Number($('#title').attr('data-id')) },
            success: function (data) {
                var ans = data.split(',');
                $.each($('.opt:checked'), function (i, o) {
                    var $this = $(this);

                    if ($(this).attr('value') == ans[0]) {
                        CorrectAns++;
                        $this.parent().nextAll('.correct').show();
                    } else {
                        $this.parent().nextAll('.wrong').show();
                    }
                    ans.shift();
                })
            },
            async: false
        });

        $('.blob').addClass('active')
        $('.hint').show()

        $.post('/Home/YourScore', { testId: ModelID, noCorrect: CorrectAns }, function (data) {
            $('.enquiremainheading').remove()
            $('.aboutheading').remove()
            $('.button').remove()
            $('.blob').eq(0).before(data);
        })
        CorrectAns=0;
    })

    function isValid() {
        var size = $('.opt:visible:checked').size();
        return size > 0;
    }
});