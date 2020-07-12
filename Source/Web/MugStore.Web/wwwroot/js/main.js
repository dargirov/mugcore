$(function () {

    $('#bulletin-add-btn').on('click', bulletinAddBtnClick);
    function bulletinAddBtnClick(e) {
        e.preventDefault();
        $('#subscribe-to-bulletin-form').submit();
    }

    $('#subscribe-to-bulletin-form').on('submit', bulletinFormSubmit);
    function bulletinFormSubmit(e) {
        e.preventDefault();
        var url = $(this).data('url');
        var email = $(this).find('input[type=email]').val();
        var token = $(this).find('input[name=__RequestVerificationToken]').val();

        if (email.length < 5) {
            return;
        }

        $.ajax({ method: 'POST', url: url, data: { email: email, __RequestVerificationToken: token } })
            .done(function (data) {
                if (data.success) {
                    $('#subscribe-to-bulletin-form').addClass('hidden');
                    $('#subscribe-success').removeClass('hidden');
                } else {
                    var $email = $('#subscribe-to-bulletin-form').find('input[type=email]');
                    $email.addClass('invalid');
                    if (data.message === 'Email exists') {
                        $email.val('Този адрес е регистриран');
                    }
                }
            });
    }

    $('#image-help').on('click', imageHelpClick);
    function imageHelpClick(e) {
        e.preventDefault();
        var url = $(this).data('url');
        $.colorbox({ href: url, opacity: 1, transition: "none", className: 'image-help-colorbox', close: 'Затвори', width: 900, height: 620 });
    }

    Notification.init();
    Notification.bind('.popover');

    $('#color-dropdown > a').on('click', colorDropDownClick);
    function colorDropDownClick(e) {
        e.preventDefault();
        var enabled = $(this).data('enabled');
        if (enabled !== true) {
            return;
        }

        var $ul = $('#color-dropdown > ul');
        var visible = $ul.css('display') === 'block';
        if (visible) {
            $(this).find('.fas').removeClass('fa-caret-up').addClass('fa-caret-down');
            $ul.slideUp();
        } else {
            $ul.slideDown();
            $(this).find('.fas').removeClass('fa-caret-down').addClass('fa-caret-up');
        }
    }

    bindColorDropdownClick = function(scene, init) {
        $('#color-dropdown ul a').on('click', function (e) {
            e.preventDefault();
            var color = $(this).data('color');
            var price = $(this).data('price').toString();
            var priceMsrp = $(this).data('price-msrp').toString();
            var colorText = $(this).text().trim();
            $('#color-dropdown span:first-of-type').text('Цвят: ' + colorText);
            $('#color-dropdown > a').data('color', color).click();
            scene.dispose();
            init(color);

            var $priceInCanvas = $('#canvas-container-right');
            var $priceInProduct = $('#product-details-order');
            if ($priceInCanvas.length === 1) {
                var priceParts = price.split('.');
                var decrease = (price - priceMsrp) / priceMsrp * 100;
                $priceInCanvas.find('.price-container-canvas h3').html(priceParts[0] + '.');
                $priceInCanvas.find('.price-container-canvas span:first-of-type').html(priceParts.length === 2 ? priceParts[1] : '00');
                $priceInCanvas.find('#canvas-price-container-msrp span').html(priceMsrp + ' лв.');
                $priceInCanvas.find('#canvas-price-container-msrp i').html('-' + Math.abs(Math.round(decrease)) + '%');
                $priceInCanvas.find('#single-price').data('price', price).html(price + ' лв.');
                $priceInCanvas.find('.price-container-calc h3').html(priceParts[0] + '.');
                $priceInCanvas.find('.price-container-calc span:first-of-type').html(priceParts.length === 2 ? priceParts[1] : '00');
            } else if ($priceInProduct.length === 1) {
                $priceInProduct.find('#product-price span:first-of-type').html(price);
            }
        });
    }

});