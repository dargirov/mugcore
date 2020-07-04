var Notification = (function($) {

    var _trigger, _animation;

    function init(options) {
        if (options === null || typeof options !== 'object') {
            options = {};
        }

        _trigger = options.trigger || 'click';
        _animation = options.animation || 'pop';
    }

    function bind(selector) {
        $(selector).webuiPopover({ trigger: _trigger, closeable: true });
    }

    function show(options) {
        if (options === null || typeof options !== 'object') {
            options = {};
        }

        var selector = options.selector || 'body';
        var title = options.title || '';
        var content = options.content || '';
        var timeout = options.timeout || false;
        var width = options.width || 300;
        var placement = options.placement || 'top';
        var closeable = options.closeable || true;

        $(selector).webuiPopover('destroy');
        $(selector).webuiPopover(
            {
                trigger: 'manual',
                title: title,
                content: content,
                animation: _animation,
                placement: placement,
                width: width,
                multi: true,
                autoHide: timeout,
                closeable: closeable
            });
        $(selector).webuiPopover('show');
    }

    return {
        init: init,
        bind: bind,
        show: show
    }

})(jQuery);