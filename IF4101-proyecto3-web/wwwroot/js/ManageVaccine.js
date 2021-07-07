var IdCard=""

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
    $.ajax({
        url: "/Vaccine/ListPatientVaccine",
        type: 'post',
        async: false,
        data: {
            "IdCard": IdCard,
        },
        dataType: 'json',
        success: function (response) {
            console.log(response);
            tbodyTable.empty();
            response.forEach(element => {
                tbodyTable.append($('<tr id="' + element['vaccinationType'] + '">')
                    .append($('<td scope="row" class="fw-bold">"').append(element['vaccinationType']))
                    .append($('<td>').append(element['fullName']))
                    .append($('<td>').append(element['description']))
                    .append($('<td>').append(element['applicationDate']))
                    .append($('<td>').append(element['nextVaccinationDate']))
                    .append($('<td>').append($('<button data-bs-toggle="modal" onclick="PutOnUpdateModal(this);" data-bs-target="#UpdateModal" class="btn btn-secondary mt-3 btn-delete-cart"><i class="fas fa-cog fa-lg"></i></button> <button onclick="DeletePatientVaccine(this)" id="btnDeleteVac" class=" btn btn-danger mt-3 btn-delete-cart"><i class="fas fa-trash-alt fa-lg"></i></button>'))) //<a> <i class="fas fa-trash-alt fa-lg"></i> </a >'
                )
            });
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
    //values
    var child = row.parentNode.parentNode;
    var description = child.cells[2].innerText;
    var lattestVaccinedate = child.cells[3].innerText;
    var nextVaccinedate = child.cells[4].innerText;
    //modal fields

    var from1 = lattestVaccinedate.split("/");

    var day = from1[0];
    var month = from1[1];
    var year = from1[2].split(" ");
    var date;
    if (parseInt(day) < 10 && parseInt(month) < 10) {
        date = `${year[0]}-0${month}-0${day}`;
    } else if (parseInt(day) < 10) {
        date = `${year[0]}-${month}-0${day}`;
    } else if (parseInt(month) < 10) {
        date = `${year[0]}-0${month}-${day}`;
    } else {
        date = `${year[0]}-${month}-${day}`;
    }
    $('#it_applicationDate').val(date);
    //fecha2

    var from2 = nextVaccinedate.split("/");
    var date2 = "";
   
    var day2 = from2[0];
    var month2 = from2[1];
    var year2 = from2[2].split(" ");
   
    if (parseInt(day2) < 10 && parseInt(month2) < 10) {
        date2 = `${year2[0]}-0${month2}-0${day2}`;
    } else if (parseInt(day2) < 10) {
        date2 = `${year2[0]}-${month2}-0${day2}`;
    } else if (parseInt(month2) < 10) {
        date2 = `${year2[0]}-0${month2}-${day2}`;
    } else {
        date2 = `${year2[0]}-${month2}-${day2}`;
    }
    $('#it_nextApplicationDate').val(date2);
    var field_description = $('#ta_description');
    field_description.val(description);

}

function UpdatePatientVaccine() {

    $.ajax({
        url: "/Vaccine/UpdatePatientVaccine",
        type: 'PUT',
        async: false,
        data: {
            "IdCard": IdCard,
            "Description": $('#ta_description').val(),
            "ApplicationDate": $('#it_applicationDate').val(),
            "NextVaccinationDate": $('#it_nextApplicationDate').val()
        },
        dataType: 'text',
        success: function (response) {
            SearchResult();
            CloseModal();
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
