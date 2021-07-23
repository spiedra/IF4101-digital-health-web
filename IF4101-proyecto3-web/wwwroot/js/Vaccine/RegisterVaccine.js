$(document).ready(function () {
    RegisterVaccine();
});


function RegisterVaccine() {
    $("#btnRegisterVaccination").click(function () {
        var form = $(this).closest("form");
        if (form.valid()) {
            $.ajax({
                url: "/Vaccine/RegisterVaccination",
                type: 'post',
                data: {
                    "IdCard": $('#patientIdCard').val(),
                    "VaccinationType": $('#s_vaccinationType').val(),
                    "Description": $('#ta_description').val(),
                    "ApplicationDate": $('#it_applicationDate').val(),
                    "NextVaccinationDate": $('#it_nextApplicationDate').val()
                },
                dataType: 'text',
                success: function (response) {
                    if (response == "1") {
                        createModalResponse("Patient vaccine successful added!");
                    } else if (response == "-1") {
                        createModalResponse("Patient vaccine was not found");
                    } else if (response == "0") {
                        createModalResponse("Patient vaccination already exists");
                    }
                }
            });
        }
    });
}

