var IdCard = ""
var OldAllergy=""
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
    var table = document.getElementById('tb_allergies');
    table.style.display = 'none';
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
            console.log(response);
            var patientName;
            tbodyTable.empty();
            response.forEach(element => {
                patientName = element['fullName'];
                tbodyTable.append($('<tr id="' + element['allergyType'] + '">')
                    .append($('<td scope="row" class="fw-bold">"').append(element['allergyType']))
                    .append($('<td>').append(element['description']))
                    .append($('<td>').append(element['diagnosticDate']))
                    .append($('<td>').append($('<button data-bs-toggle="modal" onclick="PutOnUpdateModal(this);" data-bs-target="#UpdateModal" class="btn btn-secondary mt-3 btn-delete-cart"><i class="fas fa-cog fa-lg"></i></button>'+
                        '<button onclick = "DeletePatientAllergy(this)" class= "btn btn-danger mt-3 btn-delete-cart" ><i class="fas fa-trash-alt fa-lg"></i></button >' +
                        '<button data-bs-toggle="modal" data-bs-target="#MedicamentModal" class="btn btn-danger mt-3"><i class="fas fa-pills"></i></button>')))
                )  
            });
            if (typeof patientName === 'undefined') {
                document.getElementById('h_notice').textContent = "Allergies was not found for patient: " + IdCard;
            } else {
                table.style.display = 'inline-block';
                document.getElementById('h_patientId').style.display = 'block';
                document.getElementById('h_patientId').textContent = "Patient: " + patientName;
                document.getElementById('h_notice').textContent = "";
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
            alert("borrado");
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
    //modal fields

    var from1 = diagnosticdate.split("/");

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
    $('#it_diagnosticDate').val(date);
    //
    var field_description = $('#ta_description');
    field_description.val(description);
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
            alert(response);
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

function AddPatientMedicament() {
    var DOMMedicamentList = document.getElementById('ul_medicaments');
    const med = document.createElement('li');
    med.classList.add('list-group-item', 'text-left', 'mx-1');
    med.textContent = `${$("#s_MedicamentType").val()}`;
    med.style.marginbottom = '1em';
    //
    const btn = document.createElement('button');
    btn.classList.add('btn', 'btn-danger', 'mx-4');
    btn.textContent = 'X';
    btn.width = 2;
    btn.height = 2;
    btn.style.marginLeft = '1rem';
    //btn.dataset.item = item.id;
    //btn.addEventListener('click', quitar_articulo);
    med.appendChild(btn);
    DOMMedicamentList.appendChild(med);
}
