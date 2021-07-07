$(document).ready(function () {
    createListenerBtnSearch();
});

function createListenerBtnSearch() {
    $("#btn_search").click(function () {
        var tbodyTable = $('#tbodySearchAppointment')

        $.ajax({
            url: '/Appointment/Manage',
            type: 'post',
            data: {
                "PaitentCardId": $('#inPatientId').val()
            },
            dataType: 'json',
            success: function (response) {
                tbodyTable.empty();
                response.forEach(element => {
                    tbodyTable.append($('<tr id= "' + element['appointmentId']+'">')
                        .append($('<td scope="row">"').append(element['patientName']))
                        .append($('<td>').append(element['specialityType']))
                        .append($('<td>').append(element['healthCenter']))
                        .append($('<td>').append(element['date']))
                        .append($('<td>').append($('<button class="btn btn-secondary mb-2 me-1 btn-update"><i class="fas fa-cog fa-lg"></i></i></button><button class="btn btn-danger mb-2 btn-delete"><i class="fas fa-trash-alt fa-lg"></i></button>')))
                    )
                });
                createListenerBtnDelete();
            }
        });
    });
}

function createListenerBtnDelete() {
    $(".btn-delete").click(function () {
        var trTable = $(this).closest('tr');

        $.ajax({
            url: '/Appointment/ManageDelete',
            type: 'post',
            data: {
                "AppointmentId": trTable.attr('id')
            },
            dataType: 'json',
            success: function (response) {
                alert(response);
                trTable.remove();
            }
        });
    });
}
