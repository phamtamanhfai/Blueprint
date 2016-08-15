$(document).ready(function () {
    $('.status').click(function () {
        var $this = $(this);
        var $tr = $(this).parents('tr');
        if ($(this).hasClass('lock')) {
            $.post('/Admin/Lock', { id: Number($(this).attr('data')), entity: "Feedback" }, function (data) {
                $this.find('img').attr('src', '/Images/open_lock.jpg').attr('title', 'Mở');
                $this.removeClass('lock').addClass('unlock');
                $tr.find('td').eq(6).text('Khóa');
            });
        }
        else if ($(this).hasClass('unlock')) {
            $.post('/Admin/Unlock', { id: Number($(this).attr('data')), entity: "Feedback" }, function (data) {
                $this.find('img').attr('src', '/Images/lock.jpg').attr('title', 'Khóa');
                $this.removeClass('unlock').addClass('lock');
                $tr.find('td').eq(6).text('Mở');
            });
        }
    })
})