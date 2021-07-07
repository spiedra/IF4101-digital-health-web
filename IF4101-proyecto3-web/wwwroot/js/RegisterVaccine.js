$(document).ready(function () {
    registerVaccine();
});
   

    function registerVaccine() {
        $("#btnLogIn").click(function () {
            var form = $(this).closest("form");

            if (form.valid()) {
                $.ajax({
                    url: "/LogIn/ValidateInputLogIn",
                    type: 'post',
                    data: {
                        "IdCard": $('#patientIdCard').val(),
                        "VaccinationType": $('#s_vaccinationType').val(),
                        "Description": $('#ta_description').val(),
                        "ApplicationDate": $('#it_applicationDate').val(),
                        "NextVaccinationDate": $('#it_nextApplicationDate').val()
                    },
                    dataType: 'json',
                    success: function (response) {
                        alert(response);
                    }
                });
            }
        });
    }

