$(document).ready(function () {
    if (location.href.indexOf("Khoa-hoc/") != -1) {
        $('div.fb-comments').attr('data-href', location.href);
        (function (d, s, id) {

            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) return;
            js = d.createElement(s); js.id = id;
            js.src = "//connect.facebook.net/vi_VN/sdk.js#xfbml=1&version=v2.5&appId=868674296578792";
            fjs.parentNode.insertBefore(js, fjs);

        }(document, 'script', 'facebook-jssdk'));

        var html = $('.content').html()
        var newHtml = html.split('+').join('<img src="/Images/correct.png" class="bullet"/>')
        $('.content').html(newHtml)
    } 

    $.each($('.description'), function (i, o) {
        switch (i % 3) {
            case 0:
                $('.description').eq(i).css('background-color', 'red');
                $('.course-container-home').find('a').eq(i).css('background-color', 'red');
                break;
            case 1:
                $('.description').eq(i).css('background-color', '#0058a9');
                $('.course-container-home').find('a').eq(i).css('background-color', '#0058a9');
                break;
            case 2:
                $('.description').eq(i).css('background-color', '#0298bd');
                $('.course-container-home').find('a').eq(i).css('background-color', '#0298bd');
                break;
        }

        switch (i) {
            case 0:
            case 1:
            case 2:
                $('.description').eq(i).css('height', '251px')
                if (window.innerWidth < 991) {
                    $('.col-my2-3').eq(i).find('img').css('height', '251px')
                }
                break;
            case 3:
            case 4:
            case 5:
                $('.description').eq(i).css('height', '286px')
                if (window.innerWidth < 991) {
                    $('.col-my2-3').eq(i).find('img').css('height', '286px')
                }
                break;
            case 6:
            case 7:
            case 8:
                $('.description').eq(i).css('height', '336px')
                if (window.innerWidth < 991) {
                    $('.col-my2-3').eq(i).find('img').css('height', '336px')
                }
                break;
        }
    })
})