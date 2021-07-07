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
                        "DoctorCode": $('#s_vaccinationType').val(),
                        "Password": $('#inPassword').val()
                    },
                    dataType: 'json',
                    success: function (response) {
                        alert(response);
                    }
                });
            }
        });
    }

