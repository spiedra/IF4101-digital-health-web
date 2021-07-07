$(document).ready(function () {
    createListenerBtnSearch();
});

function createListenerBtnSearch() {
    $("#btn_search").click(function () {
        var tbodyTable = $('#tbodySearchAppointment')

        $.ajax({
            url: '',
            type: 'post',
            data: {
                "PaitentCardId": $(this).closest('tr').children('td.productId').text()
            },
            dataType: 'json',
            success: function (response) {
                tbodyTable.empty();
                response.forEach(element => {
                    tbodyTable.append($('<tr>')
                        .append($('<td class="" scope="row">"').append(element['NAME']))
                        .append($('<td>').append(element['PRICE']))
                        .append($('<td>').append(element['DISCOUNTED_PRICE']))
                        .append($('<td>').append(element['START_DATE']))
                        .append($('<td>').append(element['END_DATE']))
                    )
                });
            }
        });
    });
}
