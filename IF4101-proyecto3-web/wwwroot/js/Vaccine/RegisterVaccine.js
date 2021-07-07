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
                        div = document.querySelector(".message");
                        if (response == "1") {
                            var html_text = "<div class='alert alert-success' role='alert'>" + "patient vaccine successful added!" + "</div>";
                            div.innerHTML = html_text;
                        } else if (response == "-1") {
                            var html_text = "<div class='alert alert-success' role='alert'>" + "Patient was not found" + "</div>";
                            div.innerHTML = html_text;
                        } else if (response == "0")  {
                            var html_text = "<div class='alert alert-success' role='alert'>" + "Patient vaccination already exists" + "</div>";
                            div.innerHTML = html_text;
                        }
                    }
                });
            }
        });
    }

