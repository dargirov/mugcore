$(function () {

    $('.order-note-edit').on('click', orderNoteEditClick);
    function orderNoteEditClick(e) {
        e.preventDefault();
        $(this).parent().find('.order-note-container').slideDown();
    }

});