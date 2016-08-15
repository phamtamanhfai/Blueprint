$(document).ready(function () {
    $('.flexslider').flexslider({
        animation: "fade",
        animationLoop: false,
        itemMargin: 0,
        pausePlay: true,
        start: function (slider) {
            $('body').removeClass('loading');
            $('.flex-prev').text('');
            $('.flex-next').text('');
        }
    });

    $('.animate').on("animationStart webkitAnimationStart", function (event) {
        $(this).css('opacity', '1')
    });

    $(window).scroll(function (e) {
        if ($('#bounce1').isOnScreen() > 150) {
            $('#bounce1').addClass('bounceInLeft')
            setTimeout(function () {
                $('#bounce2').addClass('bounceInRight')
            }, 300)

        }

        if ($('#bounce3').isOnScreen() > 150) {
            $('#bounce3').addClass('bounceInLeft')
            setTimeout(function () {
                $('#bounce4').addClass('bounceInRight')
            }, 300)
        }

        if ($('.four').eq(0).isOnScreen() > 150) {
            $('.four').eq(0).addClass('bounceIn')
            setTimeout(function () {
                $('.four').eq(1).addClass('bounceIn')

                setTimeout(function () {
                    $('.four').eq(2).addClass('bounceIn')

                    setTimeout(function () {
                        $('.four').eq(3).addClass('bounceIn')

                        setTimeout(function () {
                            $('.quizlet').prev().addClass('bounceIn')

                            setTimeout(function () {
                                $('.quizlet').addClass('lightSpeedIn')
                                setTimeout(function () {
                                    $('.quizlet').next().addClass('flip').addClass('animated')
                                }, 300)
                            }, 300)
                        }, 300)
                    }, 300)
                }, 300)
            }, 300)
        }

        if ($('#bounce5').isOnScreen() > 150) {
            $('#bounce5').addClass('rotateInDownLeft')
            setTimeout(function () {
                $('#bounce6').addClass('rotateInDownRight')
            }, 300)
        }

        if ($('#bounce7').isOnScreen() > 150) {
            $('#bounce7').addClass('rotateInDownLeft')
            setTimeout(function () {
                $('#bounce7').next().next().addClass('rollIn')
                setTimeout(function () {
                    $('#bounce7').next().addClass('bounceIn')
                }, 500)
            }, 500)

        }

        if ($('#bounce8').isOnScreen() > 150) {
            $('#bounce8').addClass('flip').addClass('animated')
            setTimeout(function () {
                $('#bounce9').addClass('swing')
            }, 300)
        }
    })

    $.fn.isOnScreen = function () {

        var win = $(window);

        var viewport = {
            top: win.scrollTop(),
            left: win.scrollLeft()
        };
        viewport.right = viewport.left + win.width();
        viewport.bottom = viewport.top + win.height();

        var bounds = this.offset();
        bounds.right = bounds.left + this.outerWidth();
        bounds.bottom = bounds.top + this.outerHeight();
        return viewport.bottom - bounds.top
    };
});