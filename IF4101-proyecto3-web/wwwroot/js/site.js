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
                    variable: "juanc"
                },
                dataType: 'json',
                success: function (response) {
                    alert(response);
                }
            });
        }
    });
}
