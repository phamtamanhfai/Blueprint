$(document).ready(function () {

    var NoQuestions = Number($('#tmpNoQuestions').val()); $('#tmpNoQuestions').remove();
    var ModelID = Number($('#tmpModelID').val()); $('#tmpModelID').remove();
    var Href = $('#tmpHref').val(); $('#tmpHref').remove();

    $.each($('.blob'), function (i, o) {
        var size = $(this).find('.chooser-inside').size();
        $(this).find('.icon-bar').css('top', (10 + (size - 1) * 53) + 'px');
    });

    $('#btnNext').click(function () {

        if ($(this).hasClass('disable')) {
            return;
        }

        var counter = Number($('.counter').text());
        if (isValid(counter - 1)) {
            $.when(SaveData()).then(function () {
                $('.blob').eq(counter - 1).removeClass('active')
                $('.blob').eq(counter).fadeIn(500).promise().done(function () {
                    $('.blob').eq(counter).addClass('active').removeAttr('style');
                    $('.counter').text(counter + 1);
                    $('.disable').removeClass('disable');
                    if (counter == (NoQuestions - 1)) {
                        $('#btnNext').addClass('disable');
                    };
                })
            })
        }
    })

    $('#btnPrev').click(function () {

        if ($(this).hasClass('disable')) {
            return;
        }

        var counter = Number($('.counter').text());
        if (isValid(counter - 1)) {

            $.when(SaveData()).then(function () {
                $('.blob').eq(counter - 1).removeClass('active')
                $('.blob').eq(counter - 2).fadeIn(500).promise().done(function () {
                    $('.blob').eq(counter - 2).addClass('active').removeAttr('style');
                    $('.counter').text(counter - 1);
                    $('.disable').removeClass('disable');
                    if ($('.counter').text() == '1') {
                        $('#btnPrev').addClass('disable');
                    };
                })
            })
        }
    })

    $('#btnFinish').click(function () {
        var counter = Number($('.counter').text());
        if (isValid(counter - 1)) {
            $.when(SaveData()).then(function () {
                $.post('/Admin/Unlock', {
                    id: ModelID,
                    entity: "Test"
                }, function () {
                    location.href = Href
                })
            })
        }
    });

    function SaveData() {
        $.ajax({
            url: '/Admin/ModifiedQuestion',
            data: {
                Id: Number($('.question:visible').attr('data')),
                IdHeader: ModelID,
                Content: $('.question:visible').val(),
                Hint: $('.Hint:visible').val()
            },
            success: function (data) {
                $.each($('.opt:visible'), function (i, o) {
                    $.ajax({
                        url: '/Admin/ModifiedAnswer',
                        data: {
                            Id: Number($(this).attr('data')),
                            IdQuestion: Number(data),
                            Content: $(this).val(),
                            IsCorrect: $('.sel:visible').eq(i).is(':checked')
                        },
                        async: false
                    });
                });
            },
            async: false
        });
    }

    $(document).on('keydown', '.opt', function (e) {
        if (e.which == 40 || e.keyCode == 40) {
            $('.more:visible').click();
        }

        if (e.which == 38 || e.keyCode == 38) {
            $('.less:visible').click();
        }
    })

    $('.more').click(function () {
        var html = "<div class='chooser-inside'>" +
                      "<div class='div-radio'>" +
                         "<input class='sel' type='radio' name='" + $('.sel:visible').eq(0).attr('name') + "' /></div>" +
                      "<div class='div-textbox'>" +
                         "<input class='textbox opt' placeholder='Nhập câu trả lời' value='' data='-1'/></div></div>";

        $(this).parents('.rightside').find('.chooser').append(html);
        $('.icon-bar:visible').animate({ top: "+=53px" }, 500);
    });

    $('.less').click(function () {
        var size = $('.chooser-inside:visible').size();
        if (size == 1) {
            $('.validate-message').show().text('Câu hỏi phải có ít nhất 1 câu trả lời.').delay(1000).fadeOut(1000)
            return;
        }

        if ($('.opt:visible').eq(size - 1).attr('data') != '-1') {
            $('.validate-message').show().text('Câu trả lời đã tồn tại.').delay(1000).fadeOut(1000)
            return;
        }

        $('.chooser-inside:visible').eq(size - 1).remove();
        $('.icon-bar:visible').animate({ top: "-=53px" }, 500);
    })

    function isValid(j) {

        if ($('.question').eq(j).val() == '') {
            $('.question').eq(j).addClass('field_error').focus()
            $('.validate-message').show().text('Câu hỏi không thể bỏ trống.').delay(1000).fadeOut(1000)
            $('.validate-message').promise().done(function () {
                $('.field_error').removeClass('field_error');
            });
            return false;
        }

        var emptyBox = $('.blob').eq(j).find('.opt').filter(function () {
            return $(this).val() == "";
        })

        if (emptyBox.size() > 0) {
            emptyBox.addClass('field_error')
            $('.validate-message').show().text('Câu trả lời không thể bỏ trống.').delay(1000).fadeOut(1000)
            $('.validate-message').promise().done(function () {
                $('.field_error').removeClass('field_error');
            });
            return false;
        }

        if ($('.blob').eq(j).find('.sel:checked').size() == 0) {
            $('.validate-message').show().text('Chọn đáp án đúng cho câu hỏi.').delay(1000).fadeOut(1000)
            return false;
        }

        return true;
    }
});