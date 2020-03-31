function getBill(tableId) {

    openLoader();

    $.ajax({
        type: "GET",
        url: "Bills/Create",
        data: { tableId: tableId },
        cache: false
    })
        .then(function (data) {
            var modalBody = document.getElementById("bill-body");
            modalBody.innerHTML = data;
            $('#billModal').modal('show');
            closeLoader();




        })
        .fail(function (data) {
            alert('No');
            closeLoader();

        })

    return false;

}

function separateAccount() {

    var elements = document.getElementsByClassName("bill-item-check");

    for (var check of elements) {
        check.style.display = "flex";
    }

}