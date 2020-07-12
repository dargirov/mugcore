﻿$(document).ready(function () {

    $('.gallery-img').colorbox({
        rel: 'gallery-img', transition: 'fade', photo: true, close: 'Затвори', speed: 200,
        onLoad: function () {
            $('#cboxClose').hide();
            $('#cboxPrevious').remove();
            $('#cboxNext').remove();
            $('#cboxSlideshow').remove();
            $('#cboxCurrent').remove();
        },
        onComplete: function () {
            $('#cboxClose').show();
        }
    });

    var scene = Scene;
    var cart = Cart;

    function init(color) {
        scene.init({ canvasId: 'canvas' });

        var mug1 = Mug;
        mug1.init('mugPreview', { x: 0, height: 2, z: 0 }, scene.getScene(), color);
        mug1.create();

        cart.init({ mug: mug1 });

        var previewImages = $('#product-details').data('preview-images').split(',');
        for (var i in previewImages) {
            if (previewImages[i].length > 0) {
                mug1.addImage({ name: previewImages[i], url: '/DownloadProductImage/' + previewImages[i], width: 2362, height: 1004, dpi: 300 });
            }
        }
    }

    bindColorDropdownClick(scene, init);

    if (!scene.isSupported()) {
        $('#canvas-container').remove();
        cart.init({});
    } else {
        init('std');
    }

    $('#show-form-btn').on('click', orderCreateBtnClick);
    function orderCreateBtnClick(e) {
        e.preventDefault();

        var $orderDataContainer = $('#product-details-order').find('.hidden').first();
        $orderDataContainer.removeClass('hidden');
        $(this).remove();
        $('#create-order-btn').removeClass('hidden');
    }

});