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
                    .append($('<td>').append($('<button class="btn btn-secondary mt-3 btn-delete-cart"><i class="fas fa-cog fa-lg"></i></button> <button onclick="DeletePatientVaccine(this)" id="btnDeleteVac" class=" btn btn-danger mt-3 btn-delete-cart"><i class="fas fa-trash-alt fa-lg"></i></button>'))) //<a> <i class="fas fa-trash-alt fa-lg"></i> </a >'
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
    alert("hola");
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
