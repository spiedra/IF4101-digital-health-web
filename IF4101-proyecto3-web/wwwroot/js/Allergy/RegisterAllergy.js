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
                    document.getElementById("formVaccination").reset();
                    div = document.querySelector(".message");
                    if (response == "1") {
                        var html_text = "<div class='alert alert-success' role='alert'>" + "patient allergy successful added!" + "</div>";
                        div.innerHTML = html_text;
                    } else if (response == "-1") {
                        var html_text = "<div class='alert alert-success' role='alert'>" + "Patient was not found" + "</div>";
                        div.innerHTML = html_text;
                    } else if (response == "0") {
                        var html_text = "<div class='alert alert-success' role='alert'>" + "Patient allergy already registered" + "</div>";
                        div.innerHTML = html_text;
                    }
                }
            });
        }
    });
}
