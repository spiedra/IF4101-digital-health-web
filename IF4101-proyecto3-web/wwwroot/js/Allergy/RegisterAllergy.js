$(document).ready(function () {
    RegisterAllergy();
});

function RegisterAllergy() {
    $("#btnRegisterAllergy").click(function () {
        var form = $(this).closest("form");
        if (form.valid()) {
            $.ajax({
                url: "/Allergy/RegisterAllergy",
                type: 'post',
                data: {
                    "IdCard": $('#patientIdCard').val(),
                    "AllergyType": $('#s_allergy_type').val(),
                    "Description": $('#ta_description').val(),
                    "DiagnosticDate": $('#it_DiagnosticDate').val()
                },
                dataType: 'text',
                success: function (response) {
                    //document.getElementById("formVaccination").reset();
                    if (response == "1") {
                        createModalResponse("Patient allergy successful added!");
                    } else if (response == "-1") {
                        createModalResponse("Patient was not found");
                    } else if (response == "0") {
                        createModalResponse("Patient allergy already registered");
                    }
                }
            });
        }
    });
}
