function showAlert(message, type, timeout = 3000) {
    var alertHtml = '<div class="alert alert-' + type + ' fixed-top" role="alert" style="display:none;">' +
        message +
        '<button type="button" class="close" data-dismiss="alert" aria-label="Close">' +
        '<span aria-hidden="true">×</span>' +
        '</button>' +
        '</div>';

    $('body').prepend(alertHtml); // Add the new alert to the top of the body

    // Show the alert with a slide-down animation
    $('.alert').first().slideDown();

    // Hide the alert after the given timeout
    setTimeout(() => {
        $(".alert").first().slideUp(function() {
            $(this).remove();
        });
    }, timeout);
}


function showConfirmDialog(message, onConfirm) {
    let confirmDialog = $(`<div class="modal fade" tabindex="-1" role="dialog">
                              <div class="modal-dialog" role="document">
                                <div class="modal-content">
                                  <div class="modal-header">
                                    <h5 class="modal-title">Xác nhận</h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                      <span aria-hidden="true">&times;</span>
                                    </button>
                                  </div>
                                  <div class="modal-body">
                                    <p>${message}</p>
                                  </div>
                                  <div class="modal-footer">
                                    <button type="button" class="btn btn-primary" data-dismiss="modal" id="confirmButton">Đồng ý</button>
                                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Hủy</button>
                                  </div>
                                </div>
                              </div>
                            </div>`);

    confirmDialog.modal('show');
    confirmDialog.find('#confirmButton').on('click', () => {
        console.log("ok");
        onConfirm();
        confirmDialog.modal('hide');
    });
}