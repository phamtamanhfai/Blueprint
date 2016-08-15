$(document).ready(function () {
    $('.detail').click(function () {
        var id = $(this).attr('data-id')
        $.post('/Home/FeedbackDetail', { id: id }, function (data) {
            $('.modal-form').addClass('show-up').html(data).addClass('bounceIn')
        })
    })

    $(document).on('click', '.modal-form-close', function () {
        $('.modal-form').addClass('flipOutX')
        setTimeout(function () {
            $('.modal-form').removeClass('show-up').removeClass('bounceIn').removeClass('flipOutX')
        }, 1000)
    })
})