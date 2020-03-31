function GetView(url, title) {
    openLoader();

    $.ajax({
        type: "GET",
        url: url,
        cache: false
    })
        .then(function (data) {
            setTimeout(function myfunction() {
                var modalTitle = document.getElementById("modalTitle");
                var modalBody = document.getElementById("BasicModalContent");
                modalTitle.innerHTML = title;
                modalBody.innerHTML = data;
                $('#BasicModal').modal('show');
                closeLoader();

            }, 1000);


        })
        .fail(function (data) {
            alert('No');
            closeLoader();

        })

    return false;
}

function ActionController(url, id) {


    $.ajax({
        type: "GET",
        url: url,
        data: $(id).serialize(),
        cache: false
    })
        .then(function (data) {
            setTimeout(function myfunction() {
                var modalBody = document.getElementById("BasicModalContent");
                modalBody.innerHTML = data;

            }, 1000);


        })
        .fail(function (data) {
            alert('No');

        })

    return false;
}