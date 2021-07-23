var IdCard = ""
var OldVaccine = ""
$(document).ready(function () {
    IdCard = "";
    GoSearch();
});

function GoSearch() {
    $("#btnSearch").click(function () {
        IdCard = $('#patientIdCard').val();
        SearchResult();
    });
}

function SearchResult() {
    var tbodyTable = $('#tdbody_manage_vaccine');
    var table = document.getElementById('tb_vaccine');
    document.getElementById('h_patientId').style.display = 'none';
    $.ajax({
        url: "/Vaccine/ListPatientVaccine",
        type: 'GET',
        async: false,
        data: {
            "IdCard": IdCard,
        },
        dataType: 'json',
        success: function (response) {
            if (response != null) {
                tbodyTable.empty();
                var patientName;
                response.forEach(element => {
                    patientName = element['fullName'];
                    tbodyTable.append($('<tr id="' + element['vaccinationType'] + '">')
                        .append($('<td scope="row" class="fw-bold">"').append(element['vaccinationType']))
                        .append($('<td>').append(element['description']))
                        .append($('<td>').append(element['applicationDate']))
                        .append($('<td>').append(element['nextVaccinationDate']))
                        .append($('<td>').append($('<button data-bs-toggle="modal" onclick="PutOnUpdateModal(this);" data-bs-target="#UpdateModal" class="btn btn-secondary mt-3 btn-delete-cart"><i class="fas fa-cog fa-lg"></i></button> <button onclick="DeletePatientVaccine(this)" id="btnDeleteVac" class=" btn btn-danger mt-3 btn-delete-cart"><i class="fas fa-trash-alt fa-lg"></i></button>'))) //<a> <i class="fas fa-trash-alt fa-lg"></i> </a >'
                    )
                });
                if (typeof patientName === 'undefined') {
                    createModalResponse("Vaccination was not found for patient: " + IdCard);
                } else {
                    document.getElementById('h_patientId').style.display = 'block';
                    document.getElementById('h_patientId').textContent = "Patient: " + patientName;
                    document.getElementById('h_notice').textContent = "";
                }
            } else {
                createModalResponse("Please fill in the blanks");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
}

function DeletePatientVaccine(row) {
    var child = row.parentNode.parentNode;
    var vaccinationType = child.cells[0].innerText;
    $.ajax({
        url: "/Vaccine/DeletePatientVaccine",
        type: 'DELETE',
        async: false,
        data: {
            "IdCard": IdCard,
            "VaccinationType": vaccinationType
        },
        dataType: 'text',
        success: function (response) {
            SearchResult();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
}

function PutOnUpdateModal(row) {
    //on select

    //values
    var child = row.parentNode.parentNode;
    var description = child.cells[1].innerText;
    var lattestVaccinedate = child.cells[2].innerText;
    var nextVaccinedate = child.cells[3].innerText;
    OldVaccine = child.cells[0].innerText;
    $("#s_vaccinationType option[value=0]").html(child.cells[0].innerText);
    $('#s_vaccinationType > option[value="0"]').attr('value', child.cells[0].innerText);
    //modal fields

    lattestVaccinedate = lattestVaccinedate.replace(/\//g, '-');
    const date2 = moment(lattestVaccinedate, 'DD-MM-YYYY').format('MM-DD-YYYY');
    $('#it_applicationDate').val(moment(new Date(date2)).format('YYYY-MM-DD'));

    //fecha2
    nextVaccinedate = nextVaccinedate.replace(/\//g, '-');
    const date1 = moment(nextVaccinedate, 'DD-MM-YYYY').format('MM-DD-YYYY');
    $('#it_nextApplicationDate').val(date1);

    var field_description = $('#ta_description');
    field_description.val(description);
}

function setCurrentDatetime2(dateString) {
    dateString = dateString.replace(/\//g, '-');
    const date = moment(dateString, 'DD-MM-YYYY').format('MM-DD-YYYY');
    $('#it_diagnosticDate').val(moment(new Date(date)).format('YYYY-MM-DD'));
}

function UpdatePatientVaccine() {
    $.ajax({
        url: "/Vaccine/UpdatePatientVaccine",
        type: 'PUT',
        async: false,
        data: {
            "IdCard": IdCard,
            "OldVaccineType": OldVaccine,
            "VaccineType": $("#s_vaccinationType").val(),
            "Description": $('#ta_description').val(),
            "ApplicationDate": $('#it_applicationDate').val(),
            "NextVaccinationDate": $('#it_nextApplicationDate').val()
        },
        dataType: 'text',
        success: function (response) {
            $("#UpdateModal").modal('hide');
            if (response == "1") {
                createModalResponse("Patient vaccine information successful changed!");
            } else if (response == "-1") {
                createModalResponse("Error. Patient vaccine type already exists");
            }
            SearchResult();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
}
function CloseModal() {
    $("#UpdateModal").modal('hide');
    $('body').removeClass('modal-open');
    $('.modal-backdrop').remove();
}
