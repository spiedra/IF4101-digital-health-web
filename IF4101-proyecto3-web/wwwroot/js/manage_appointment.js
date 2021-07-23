var appointmentId;

$(document).ready(function () {
    createListenerBtnSearch();
    createListenerBtnConfirm();
    createListenerBtnConfirmUpdate();
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
                console.log(response)
                if (response != null) {
                    tbodyTable.empty();
                    response.forEach(element => {
                        tbodyTable.append($('<tr id= "' + element['appointmentId'] + '">')
                            .append($('<td scope="row">"').append(element['patientName']))
                            .append($('<td id="specialityType' + element['appointmentId'] + '" class="specilType">').append(element['specialityType']))
                            .append($('<td id="healthCenter' + element['appointmentId'] + '" class="healthCenterName">').append(element['healthCenter']))
                            .append($('<td id="description' + element['appointmentId'] + '">').append(element['description']))
                            .append($('<td id="date' + element['appointmentId'] + '" class="appointmentDate">').append(moment(new Date(element['date'])).format('MMMM Do YYYY, h:mm:ss a')))
                            .append($('<td>').append(
                                $('<button class="btn btn-secondary mb-2 me-1 btn-update" data-bs-toggle="modal" data-bs-target="#updateAppointment"><i class="fas fa-cog fa-lg"></i></i></button>'
                                    + '<button class= "btn btn-primary mb-2 btn-add me-1" data-bs-target="#addDescriptionModal"><i class="fas fa-comment-medical fa-lg"></i></button >'
                                    + '<button class= "btn btn-danger mb-2 btn-delete"> <i class="fas fa-trash-alt fa-lg"></i></button >')))
                        )
                    });
                } else {
                    $('#searchModal').modal("hide");
                    createModalResponse("Please fill in the blanks");
                }
                createListenerBtnUpdate();
                createListenerBtnDelete();
                createListenerBtnAddDiagnostic();
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
                createModalResponse(response);
                trTable.remove();
            }
        });
    });
}

function createListenerBtnUpdate() {
    $(".btn-update").click(function () {
        appointmentId = $(this).closest('tr').attr('id');
        var selectHealthCenters = $('#selectHealthCenters');
        var selectSpecialityTypes = $('#selectSpecialityTypes');
        var currentHealthCenter = $(this).closest('tr').children('td.healthCenterName').text();
        var currentSpecialityType = $(this).closest('tr').children('td.specilType').text();

        setCurrentDatetime($(this).closest('tr').children('td.appointmentDate').text());
        $.ajax({
            url: '/Appointment/GetListMedicalCenters',
            type: 'get',
            dataType: 'json',
            success: function (response) {
                selectHealthCenters.empty();
                response.forEach(element => {
                    if (element == currentHealthCenter) {
                        selectHealthCenters.append('<option class="select_opcion" selected>' + element + '</option>');
                    } else {
                        selectHealthCenters.append('<option class="select_opcion">' + element + '</option>');
                    }
                });
            }
        });

        $.ajax({
            url: '/Appointment/GetListSpecialtyTypes',
            type: 'get',
            dataType: 'json',
            success: function (response) {
                selectSpecialityTypes.empty();
                response.forEach(element => {
                    if (element == currentSpecialityType) {
                        selectSpecialityTypes.append('<option class="select_opcion" selected>' + element + '</option>');
                    } else {
                        selectSpecialityTypes.append('<option class="select_opcion">' + element + '</option>');
                    }
                });
            }
        });
    });
}

function setCurrentDatetime(dateString) {
    var d = moment(dateString, "MMMM Do YYYY, h:mm a").toDate();
    $('#inDate').val(moment(new Date(d.toISOString())).format('YYYY-MM-DDThh:mm:ss.SSS'));
}

function createListenerBtnAddDiagnostic() {
    $(".btn-add").click(function () {
        appointmentId = $(this).closest('tr').attr('id');
        $.ajax({
            url: '/Appointment/ManageDate',
            type: 'post',
            data: {
                "AppointmentId": appointmentId,
            },
            dataType: 'json',
            success: function (response) {
                if (response == -1) {
                    $('#addDescriptionModal').modal("show");
                } else {
                    createModalResponse("Pending appointment");
                }
            }
        });
    });
}

function createListenerBtnConfirm() {
    $("#btn_confirm").click(function () {
        var DiagnosticDescription = $('#inDescription').val();
        $.ajax({
            url: '/Appointment/ManageDiagnostic',
            type: 'post',
            data: {
                "AppointmentId": appointmentId,
                "DiagnosticDescription": DiagnosticDescription
            },
            dataType: 'json',
            success: function (response) {
                $('#addDescriptionModal').modal("hide");
                createModalResponse(response);
                $('#description' + appointmentId).text(DiagnosticDescription);
            }
        });
    });
}

function createListenerBtnConfirmUpdate() {
    $("#btn_confirmUpdate").click(function () {
        var healthCenterSelected = $('#selectHealthCenters').val();
        var specialityTypeSelected = $('#selectSpecialityTypes').val();
        var appointmentDate = $('#inDate').val();
        $.ajax({
            url: '/Appointment/ManageUpdate',
            type: 'post',
            data: {
                "AppointmentId": appointmentId,
                "HealthCenter": healthCenterSelected,
                "SpecialityType": specialityTypeSelected,
                "Date": appointmentDate
            },
            dataType: 'json',
            success: function (response) {
                $('#updateAppointment').modal("hide");
                createModalResponse(response); 
                $('#healthCenter' + appointmentId).text(healthCenterSelected);
                $('#specialityType' + appointmentId).text(specialityTypeSelected);
                $('#date' + appointmentId).text(moment(new Date(appointmentDate)).format('MMMM Do YYYY, h:mm:ss a'));
            }
        });
    });
}

