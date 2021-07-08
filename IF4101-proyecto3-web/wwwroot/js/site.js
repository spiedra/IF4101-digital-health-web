function createModalResponse(response) {
    $('#modalResponse').remove();
    $('#pageMain').append($('<div class="modal fade" id="modalResponse" tabindex="-1" aria-labelledby="modalResponse" aria-hidden="true">')
        .append($('<div class="modal-dialog">')
        .append($('<div class="modal-content">')
        .append($('<div class="modal-header">'))
        .append($('<div class="modal-body">').append('<div class="container"><p>' + response + '</p></div>'))
        .append($('<div class="modal-footer">').append('<button type="button" class="btn btn-primary" data-bs-dismiss="modal">Ok</button>'))))
    )
    $('#modalResponse').modal("show");
}
