var Cart = (function ($, Notification) {
    'use strict';

    var _mug, _skipCustomization = false, _currentStep = 1, _breakWidth = 1000;
    var MAX_IMAGE_COUNT = 3;

    function addLoader() {
        var html = '<div id="upload-loader"><div class="fa-5x"><i class="fas fa-sync fa-spin"></i></div></div>';
        $('body').append(html);
    }

    function removeLoader() {
        $('#upload-loader').remove();
    }

    function bindUpload() {
        $('#step1').find('input[type=file]').on('change', function () {
            var $self = $(this);
            var data = new FormData();
            data.append('upload_file', $self.prop('files')[0]);

            addLoader();

            $.ajax({
                type: 'POST',
                processData: false,
                contentType: false,
                data: data,
                url: $('#step1').data('url'),
                dataType: 'json',
                success: uploadFile
            });
        });
    }

    function uploadFile(data) {

        removeLoader();

        if (data.success) {
            _mug.addImage({ name: data.filename, url: '/Download/' + data.filename, width: data.width, height: data.height, dpi: data.dpi });
            $('#customization-controls-container').removeClass('hidden');
            var $controls = $($('.move-controls.hidden')[0]);
            $controls.removeClass('hidden');

            var images = MAX_IMAGE_COUNT - _mug.getImagesCount();
            if (images > 0) {
                var uploadMoreNotificationPlacement = window.innerWidth > _breakWidth ? 'top' : 'bottom';
                Notification.show({ selector: '#canvas-container-right input[type=file]', content: 'Може да качите още ' + images + ' ' + (images === 1 ? 'изображение' : 'изображения'), placement: uploadMoreNotificationPlacement, timeout: 4000, width: 270 });
            } else {
                $('#canvas-container-right input[type=file]').prop('disabled', true);
            }

            if (images === 2 && window.innerWidth > _breakWidth) {
                Notification.show({ selector: '.move-controls', content: 'Използвайте стрелките за да преместите снимката върху чашата', placement: 'left', timeout: 6000, width: 190 });
            }

            if (data.width < 1500 && data.height < 700 && data.dpi < 120) {
                var imageQualityNotificationWidth = window.innerWidth > _breakWidth ? 500 : 300;
                Notification.show({ selector: '#canvas', content: 'Каченото изображение е с резолюция ' + data.width + 'px x ' + data.height + 'px @ ' + data.dpi + 'dpi. За постигане на максимално добро качество при печат ви препоръчваме да използвате изображение с размер 2362px x 1004px @ 300dpi.', placement: 'bottom', timeout: 14000, width: imageQualityNotificationWidth });
            }

        } else {
            alert('Възникна грешка при качване на файла. Моля, опитайте с друго изображение.');
        }
    }

    function bindMovingImages() {
        $('.move').on('click', moveButtonClick);
        function moveButtonClick(e) {
            e.preventDefault();
            var imageIndex = $(this).parent().parent().data('image');
            var dir = $(this).data('direction');
            var image = _mug.getImage(imageIndex);

            switch (dir) {
                case 'up':
                    image.moveUp();
                    break;
                case 'down':
                    image.moveDown();
                    break;
                case 'left':
                    image.moveLeft();
                    break;
                case 'right':
                    image.moveRight();
                    break;
            }
        }
    }

    function bindEvents() {

        if (!_skipCustomization) {
            bindUpload();
            bindMovingImages();
        }

        // next step buttons
        $('#goto-step2').on('click', gotoStep2BtnClick);
        function gotoStep2BtnClick(e) {
            e.preventDefault();
            gotoStep2();
        }

        $('#goto-step3').on('click', gotoStep3BtnClick);
        function gotoStep3BtnClick(e) {
            e.preventDefault();
            gotoStep3();
        }

        // quantity range
        $('#quantity-range').on('change', quantityRangeChange);
        function quantityRangeChange(e) {
            var price = parseFloat($('#single-price').data('price'));
            var quantity = parseInt($(this).val());
            var deliveryFee = parseFloat($('#delivery-fee').data('fee'));

            $('#selected-quantity').html(quantity);
            $('.price-container-calc').find('h3').html((price * quantity + deliveryFee) + '.');
        }

        // go back bottons
        $('.order-back').on('click', goBackToStep);
        function goBackToStep(e) {
            e.preventDefault();
            var toStep = parseInt($(this).data('to-step'));
            switch (toStep) {
                case 1:
                    gotoStep1();
                    break;
                case 2:
                    gotoStep2();
                    break;
                case 3:
                    gotoStep3();
                    break;
            }
        }

        // final button for creating order
        var orderCreateStarted = false;
        $('#create-order-btn').on('click', createOrderBtnClick);
        function createOrderBtnClick(e) {
            e.preventDefault();
            if (orderCreateStarted) {
                return;
            }

            var url = $(this).data('url');
            var sendEmailUrl = $(this).data('send-email-url');
            var count = !_skipCustomization ? _mug.getImagesCount() : 0;
            var data = {};
            data.images = [];
            for (var i = 0; i < count; i++) {
                data.images.push(_mug.getImage(i).getInfo());
            }

            data.quantity = parseInt($('#quantity-range').val());
            //data.price = $('#single-price').data('price');
            data.paymentMethod = $('#payment-method').val();
            if ($('#product-acronym') !== null) {
                data.productAcronym = $('#product-acronym').val();
            }

            data.deliveryInfo = {};
            data.deliveryInfo.fullName = $('#names-field').val();
            data.deliveryInfo.phone = $('#phone-field').val();
            data.deliveryInfo.cityId = $('#city-dd').val();
            data.deliveryInfo.address = $('#address-field').val();
            data.deliveryInfo.comment = $('#comment-field').val();
            data.deliveryInfo.email = $('#email-field').val();
            data.deliveryInfo.courierId = $('#courier-dd').val();

            var invalidInput = false;
            if (data.deliveryInfo.fullName.length === 0) {
                $('#names-field').addClass('invalid').val('Въведи име и фамилия');
                invalidInput = true;
            }

            if (data.deliveryInfo.fullName.length > 0 && data.deliveryInfo.fullName.length < 4) {
                $('#names-field').addClass('invalid').val('Въведи валидно име и фамилия');
                invalidInput = true;
            }

            if (data.deliveryInfo.phone.length === 0) {
                $('#phone-field').addClass('invalid').val('Въведи телефон за контакт');
                invalidInput = true;
            }

            if ((data.deliveryInfo.phone.length > 0 && data.deliveryInfo.phone.length < 8)
                || (data.deliveryInfo.phone.length > 0 && data.deliveryInfo.phone.match(/\d+/g) === null)) {
                $('#phone-field').addClass('invalid').val('Въведи валиден телефон за контакт');
                invalidInput = true;
            }

            if (data.deliveryInfo.address.length === 0) {
                $('#address-field').addClass('invalid').val('Въведи адрес за доставка');
                invalidInput = true;
            }

            if (data.deliveryInfo.address.length > 0 && data.deliveryInfo.address.length < 8) {
                $('#address-field').addClass('invalid').val('Въведи валиден адрес за доставка');
                invalidInput = true;
            }

            if (!invalidInput) {
                orderCreateStarted = true;
                $(this).prepend('<i class="fas fa-sync fa-spin"></i>');
                $.ajax({ method: "POST", url: url, data: JSON.stringify(data), contentType: "application/json; charset=utf-8" })
                    .done(function (data) {
                        orderCreateStarted = false;
                        var paymentMethods = ['', 'Наложен платеж', 'Банков превод'];
                        if (data.status === 'success') {
                            $('#step4-acronym').find('span').html(data.acronym);
                            $('#step4-payment-method').html(paymentMethods[data.paymentMethod]);
                            $('#step4-address').html(data.fullName + '<br>' + data.city + ', ' + data.address + '<br>' + data.phone);
                            $('#step4-quantity').html(data.quantity);
                            $('#step4-price').html(data.price.toFixed(2) + ' лв.');
                            if (data.email !== null) {
                                $('#step4-email-dt').removeClass('hidden');
                                $('#step4-email').removeClass('hidden').html(data.email);
                            }

                            if (data.courier !== null) {
                                $('#step4-courier-dt').removeClass('hidden');
                                $('#step4-courier').removeClass('hidden').html(data.courier);
                                $('#step4-delivery-fee').removeClass('hidden');
                                var priceHtml = $('#step4-price').html();
                                $('#step4-price').html(priceHtml + ' *');
                            }

                            gotoStep4();

                            $.ajax({ method: "POST", url: sendEmailUrl, data: { acronym: data.acronym } })
                                .done(function () { });

                            // Facebook purchase event
                            fbq('track', 'Purchase', {
                                value: data.price.toFixed(2),
                                currency: 'BGN',
                            });
                        }
                    });
            }
        }

        $('body').on('focus', '.invalid', removeInvalidMessageFocus);
        function removeInvalidMessageFocus(e) {
            $(this).val('');
            $(this).removeClass('invalid');
        }
    }




    var gotoStep1 = function () {
        $('#step1').removeClass('hidden');
        $('#step2').addClass('hidden');
        $('#step3').addClass('hidden');
    }

    var gotoStep2 = function () {
        $('#step1').addClass('hidden');
        $('#step3').addClass('hidden');
        $('#step2').removeClass('hidden');
    }

    var gotoStep3 = function () {
        $('#step1').addClass('hidden');
        $('#step2').addClass('hidden');
        $('#step3').removeClass('hidden');
    }

    var gotoStep4 = function () {
        $('#step1').addClass('hidden');
        $('#step2').addClass('hidden');
        $('#step3').addClass('hidden');
        $('#step4').removeClass('hidden');
    }

    function init(options) {

        if (options.mug === undefined) {
            _skipCustomization = true;
        } else {
            _mug = options.mug;
        }

        bindEvents();
    }

    return {
        init: init
    }

})(jQuery, Notification);