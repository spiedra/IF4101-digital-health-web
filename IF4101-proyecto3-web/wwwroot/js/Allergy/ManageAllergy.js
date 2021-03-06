var IdCard = ""
var OldAllergy = ""
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
    var tbodyTable = $('#tdbody_manage_allergy');
    document.getElementById('h_patientId').style.display = 'none';
    $.ajax({
        url: "/Allergy/ListPatientAllergies",
        type: 'GET',
        async: false,
        data: {
            "IdCard": IdCard
        },
        dataType: 'json',
        success: function (response) {
            if (response != null) {
                var patientName;
                tbodyTable.empty();
                response.forEach(element => {
                    patientName = element['fullName'];
                    tbodyTable.append($('<tr id="' + element['allergyType'] + '">')
                        .append($('<td scope="row" class="fw-bold">"').append(element['allergyType']))
                        .append($('<td>').append(element['description']))
                        .append($('<td>').append(element['diagnosticDate']))
                        .append($('<td>').append($('<button data-bs-toggle="modal" onclick="PutOnUpdateModal(this);" data-bs-target="#UpdateModal" class="btn btn-secondary mt-3 me-1"><i class="fas fa-cog fa-lg"></i></button>' +
                            '<button onclick = "DeletePatientAllergy(this)" class= "btn btn-danger mt-3 me-1" ><i class="fas fa-trash-alt fa-lg"></i></button >' +
                            '<button onclick="PutAllergy(this)" data-bs-toggle="modal" data-bs-target="#MedicamentModal" class="btn btn-success mt-3 me-1"><i class="fas fa-pills"></i></button>')))
                    )
                });
                if (typeof patientName === 'undefined') {
                    createModalResponse("Allergies was not found for patient: " + IdCard);
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

function DeletePatientAllergy(row) {
    var child = row.parentNode.parentNode;
    var AllergyType = child.cells[0].innerText;
    $.ajax({
        url: "/Allergy/DeletePatientAllergy",
        type: 'DELETE',
        async: false,
        data: {
            "IdCard": IdCard,
            "AllergyType": AllergyType
        },
        dataType: 'text',
        success: function (response) {
            createModalResponse("Successfully eliminated");
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
    OldAllergy = child.cells[0].innerText;
    var diagnosticdate = child.cells[2].innerText;
    $("#s_allergyType option[value=0]").html(child.cells[0].innerText);
    $('#s_allergyType > option[value="0"]').attr('value', child.cells[0].innerText);
    setCurrentDatetime1(diagnosticdate);
    var field_description = $('#ta_description');
    field_description.val(description);
}

function setCurrentDatetime1(dateString) {
    dateString = dateString.replace(/\//g, '-');
    const date = moment(dateString, 'DD-MM-YYYY').format('MM-DD-YYYY');
    $('#it_diagnosticDate').val(moment(new Date(date)).format('YYYY-MM-DD'));
}

function UpdatePatientAllergy() {

    $.ajax({
        url: "/Allergy/UpdatePatientAllergy",
        type: 'PUT',
        async: false,
        data: {
            "IdCard": IdCard,
            "OldAllergyType": OldAllergy,
            "AllergyType": $("#s_allergyType").val(),
            "Description": $('#ta_description').val(),
            "DiagnosticDate": $('#it_diagnosticDate').val()
        },
        dataType: 'text',
        success: function (response) {
            $("#UpdateModal").modal('hide');
            if (response == "1") {
                createModalResponse("Patient allergy information successful changed!");
            } else if (response == "-1") {
                createModalResponse("Error. Patient allergy type already exists");
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

function ListPatientMedicaments() {

    var DOMMedicamentList = document.getElementById('ul_medicaments');
    DOMMedicamentList.textContent = '';
    $.ajax({
        url: "/Allergy/ListPatientMedicaments",
        type: 'GET',
        async: false,
        data: {
            "IdCard": IdCard,
            "AllergyType": OldAllergy
        },
        dataType: 'json',
        success: function (response) {
            response.forEach(element => {
                AddMedicamentToList(element);
            });
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
}


function AddPatientMedicament() {

    $.ajax({
        url: "/Allergy/AddPatientMedicament",
        type: 'post',
        async: false,
        data: {
            "IdCard": IdCard,
            "MedicineType": $("#s_MedicamentType").val(),
            "AllergyType": OldAllergy
        },
        dataType: 'text',
        success: function (response) {
            ListPatientMedicaments();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
}

function RemovePatientMedicament(event) {
    const MedicamentType = event.target.dataset.item;

    $.ajax({
        url: "/Allergy/RemovePatientMedicament",
        type: 'DELETE',
        async: false,
        data: {
            "IdCard": IdCard,
            "MedicineType": MedicamentType,
            "AllergyType": OldAllergy
        },
        dataType: 'text',
        success: function (response) {
            ListPatientMedicaments();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
}

function AddMedicamentToList(medicament) {

    var DOMMedicamentList = document.getElementById('ul_medicaments');
    const med = document.createElement('li');
    med.classList.add('list-group-item', 'text-left', 'me-1');
    med.textContent = `${medicament}`;
    med.style.marginbottom = '1em';
    //
    const btn = document.createElement('button');
    btn.classList.add('btn', 'btn-danger', 'mx-4');
    btn.textContent = 'X';
    btn.width = 2;
    btn.height = 2;
    btn.style.marginLeft = '1rem';
    btn.dataset.item = medicament;
    btn.addEventListener('click', RemovePatientMedicament);
    med.appendChild(btn);
    DOMMedicamentList.appendChild(med);
}

function PutAllergy(row) {
    var title = document.getElementById('h_manageMedicaments');
    var child = row.parentNode.parentNode;
    OldAllergy = child.cells[0].innerText;
    ListPatientMedicaments();
    title.textContent = 'Manage medicaments for ' + OldAllergy + ' allergy';
}



