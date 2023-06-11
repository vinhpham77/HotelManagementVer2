$(document).ready(function () {
    $('#viewEdit').click(function () {
        $.ajax({
            url: '/rent/Edit',
            type: 'GEt',
            success: function (result) {
                $('rightLayout').cshtml(result);
            }
        })
    })
})