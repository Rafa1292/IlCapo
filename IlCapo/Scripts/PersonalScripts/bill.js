function getBill(tableId, toGo) {
    openLoader();

    $.ajax({
        type: "GET",
        url: "Bills/Create",
        data: {
            tableId: tableId,
            toGo: toGo
        },
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

function getAddress(phone) {
    if (phone == "") {
        phone = 0;
    }
    getClientName(phone);
    $.ajax({
        type: "GET",
        url: "Addresses/GetAddress",
        data: {
            phone: parseInt(phone),
        },
        cache: false
    })
        .then(function (data) {
            let viewContainer = document.getElementById("addressessContainer");
            viewContainer.innerHTML = "";
            viewContainer.innerHTML = data;

        })
        .fail(function (data) {
            alert('imposible obtener direcciones');

        })

    return false;
}

function createBill() {
    let phone = parseInt(document.getElementById("phone").innerHTML);
    let itemsList = getItemsList();
    let toGo = document.getElementById("toGo").checked;
    let discount = document.getElementById("descuento").innerHTML;
    let express = document.getElementById("express").value;
    let tableId = document.getElementById("tableId").value;
}

function addAddress(id) {

    if (id == 0) {

        let container = document.getElementById("addressessContainer");
        content = `    
                <label class="mx-2 col-md-6">Direcciones</label>
                <input type="text" id="newAddress" onchange="saveAddress()" class="form-control rounded" />`;

        container.innerHTML = content;
    }
}

function getClientName(phone) {
    $.ajax({
        type: "GET",
        url: "Clients/GetClient",
        data: {
            phone: parseInt(phone),
        },
        cache: false
    })
        .then(function (data) {
            let billName = document.getElementById("name");
            billName.value = data;

        })
        .fail(function (data) {
            alert('imposible obtener cliente');

        })

    return false;
}

function saveAddress() {
    let newAddress = document.getElementById("newAddress").value;
    let phone = parseInt(document.getElementById("phone").value);
    let name = document.getElementById("name").value;

    $.ajax({
        type: "GET",
        url: "Addresses/NewAddress",
        data: {
            phone: phone,
            name: name,
            address: newAddress
        },
        cache: false
    })
        .then(function (data) {
            if (data) {
                alert("Direccion añadida");
            }
            else {
                alert("Fallo al añadir direccion");
            }

        })
        .fail(function (data) {
            alert('imposible añadir direcciones en este momento');

        })

    return false;

}