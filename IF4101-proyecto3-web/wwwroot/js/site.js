$(document).ready(function () {
    createListenerBtnLogIn();
});

function createListenerBtnLogIn() {
    $("#btnLogIn").click(function () {
        var form = $(this).closest("form");

        if (form.valid()) {
            $.ajax({
                url: "/LogIn/ValidateInputLogIn",
                type: 'POST',
                data: {
                    IdCard: "",
                    DoctorCode: "",
                    Password: ""
                },
                dataType: 'json',
                success: function (response) {
                    alert(response);
                }
            });
        }
    });
}
