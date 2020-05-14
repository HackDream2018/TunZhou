(function () {
    $('.clicks .border p').click(function () {
        if ($(this).attr('data-index') == '0') {
            $(this).attr('data-index', '1');
            $(this).find('img').attr('src', '../../images/activity/CityProper/check_bule.png')
        } else {
            $(this).attr('data-index', '0');
            $(this).find('img').attr('src', '../../images/activity/CityProper/check_boc.png');
        }
    })
    $('.clicks .right h5 p').click(function () {
        if ($(this).attr('data-index') == '0') {
            $(this).attr('data-index', '1');
            $(this).find('img').attr('src', '../../images/activity/CityProper/check_bule.png')
            $('.clicks table tr td').each(function () {
                $(this).attr('data-index', '1');
                $(this).find('img').attr('src', '../../images/activity/CityProper/check_bule.png')
            })
        } else {
            $(this).attr('data-index', '0');
            $(this).find('img').attr('src', '../../images/activity/CityProper/check_boc.png');

            $('.clicks table tr td').each(function () {
                $(this).attr('data-index', '0');
                $(this).find('img').attr('src', '../../images/activity/CityProper/check_boc.png');
            })
        }
    })

})(document, window);
