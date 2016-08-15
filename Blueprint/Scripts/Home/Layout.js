$(document).ready(function () {
    var Admin = $('#tmpAdmin').val(); $('#tmpAdmin').remove();
    var IsAuthenticated = $('#tmpIsAuthenticated').val(); $('#tmpIsAuthenticated').remove();

    toastr.options = {
        closeButton: true,
        progressBar: true,
        positionClass: 'toast-top-center',
        onclick: null,
        'body-output-type': 'trustedHtml'
    };

    if (Admin != 'True') {
        $('.admin').remove();
    } else {
        $('.admin').show();
    }

    if (IsAuthenticated == 'True') {
        $('.logout').show()
        $('.login').hide()
        $('.menu-container').css("width", "580px")
    } else {
        $('.login').show()
        $('.logout').hide()
        $('.menu-container').css("width", "475px")
    }

    $('.aside-menu-toggler').click(function () {
        if ($(this).hasClass('show')) {
            $('body').addClass('body-out')
            $('.aside-menu').addClass('aside-menu-in')
            $(this).removeClass('show')
        } else {
            $('body').removeClass('body-out')
            $('.aside-menu').removeClass('aside-menu-in')
            $(this).addClass('show')
        }
    });

    $('.aside-close').click(function () {
        $('body').removeClass('body-out')
        $('.aside-menu').removeClass('aside-menu-in')
        $('.aside-menu-toggler').addClass('show')
    })

    $('.plus').click(function () {
        var newtext = $(this).html() == "+" ? "-" : "+";
        $(this).html(newtext);
        $(this).parent().toggleClass('goup');
    })
})