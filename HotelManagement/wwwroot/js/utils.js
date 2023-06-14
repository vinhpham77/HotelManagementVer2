function showAlert(message, type, timeout = 2000) {
    // Create the alert HTML using Bootstrap 4 classes
    var alertHtml = '<div class="alert alert-' + type + ' alert-dismissible fade show position-fixed" style="top: 20px; right: 20px; width: 300px; z-index: 9999; box-shadow: 0px 0px 5px rgba(0,0,0,0.3); margin-bottom: 10px;" role="alert">' +
        message +
        '<button type="button" class="close" data-dismiss="alert" aria-label="Close">' +
        '<span aria-hidden="true">×</span>' +
        '</button>' +
        '</div>';

    // Add the new alert to the top of the body
    var newAlert = $(alertHtml).prependTo('body');

    // Adjust the position of the new alert
    adjustAlertsPosition();

    // Hide the alert after the given timeout
    setTimeout(() => {
        newAlert.alert('close');
    }, timeout);

    // Adjust the position of the alerts when an alert is closed
    newAlert.on('closed.bs.alert', function () {
        adjustAlertsPosition();
    });
}

function adjustAlertsPosition() {
    $('.alert').each(function (index) {
        $(this).css('top', (20 + index * 50) + 'px');
    });
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

function getNumberValue(inputId) {
    // Lấy giá trị từ input
    const inputValue = $(inputId).val();

    // Loại bỏ dấu phẩy và chuyển đổi thành số
    return Number(inputValue.replace(/,/g, ''));
}