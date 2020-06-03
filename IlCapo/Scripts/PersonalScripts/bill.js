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

function getAddress(phone) {
    if (phone == "") {
        phone = 0;
    }
    getClientName(phone);
    $.ajax({
        type: "GET",
        url: "Addresses/GetAddress",
        data: {
            phone: parseInt(phone)
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
                let viewContainer = document.getElementById("addressessContainer");
                viewContainer.innerHTML = "";
                viewContainer.innerHTML = data;
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

function getItemsList() {
    let products = document.getElementsByName("BillProducts");
    let addedProducts = new Array();

    for (var i = 0; i < products.length; i++) {
        let id = products[i].id;
        let quantity = parseInt(document.getElementById(`quantity${products[i].id}`).innerHTML);
        let observation = document.getElementById(`observation${products[i].id}`).value;
        let extras = getExtrasByProduct(id, quantity);
        let sides = getSidesByProduct(id, quantity);
        let productObject = selectProduct(id);
        let Price = productObject.Price * quantity;

        let product = {
            ProductId: id,
            ProductQuantity: quantity,
            Observation: observation,
            Price: Price,
            Extras: extras,
            Sides: sides
        }

        addedProducts.push(product);
    }

    return addedProducts;
}

function getExtrasByProduct(productId, quantity) {

    let addedExtras = new Array();

    for (var i = 1; i <= quantity; i++) {

        for (var x = 0; x < extras.length; x++) {

            let element = document.getElementById(`extraFinalPrice${productId}${i}${extras[x].Id}`)
            let extraQuantity = parseInt(document.getElementById(`extraQuantity${productId}${i}${extras[x].Id}`).innerHTML);
            if (parseInt(element.value) > 0) {
                let extra = {
                    Id: extras[x].Id,
                    ExtraQuantity: extraQuantity,
                    Quantity: i,
                    ProductId: productId
                }

                addedExtras.push(extra);
            }
        }
    }

    return addedExtras;
}

function getSidesByProduct(productId, quantity) {

    let addedSides = new Array();

    for (var i = 1; i <= quantity; i++) {
        let sideLabelContainer = document.getElementById(`sideLabelContainer${productId}${i}`);
        let sideInputs = sideLabelContainer.getElementsByTagName("input");

        for (var x = 0; x < sideInputs.length; x++) {
            let side = {
                SideId: sideInputs[x].value,
                ProductId: productId,
                Quantity: i
            }

            addedSides.push(side);
        }
    }

    return addedSides;
}

function commandBill() {
    openLoader();
    let bill = getBillData();
    let orderNumber = document.getElementById("orderNumber").innerHTML;

    $.ajax({
        type: "Get",
        url: "Bills/CommandBill",
        data: {
            jsonData: bill,
            orderNumber: parseInt(orderNumber)
        },
        cache: false
    })
        .then(function (data) {
            alert("si");
            closeLoader();
        })
        .fail(function (data) {
            alert('No');
            closeLoader();

        })

    return false;

}

function getBillData() {
    let phone = document.getElementById("phone").value;
    if (phone == "") {
        phone = 0;
    }
    let itemsList = getItemsList();
    let toGo = document.getElementById("toGo").checked;
    let discount = document.getElementById("descuento").innerHTML;
    let tableId = document.getElementById("tableId").value;

    let expressInput = document.getElementById("express");
    let express = false;

    if (expressInput != null) {
        express = expressInput.value;
    }

    let address = "";
    let selectAddress = document.getElementById("selectAddress");
    address = selectAddress.value;

    let bill = {
        Phone: phone,
        Items: itemsList,
        ToGo: toGo,
        Express: express,
        Discount: discount,
        TableId: tableId,
        Address: address
    }

    return JSON.stringify(bill);

}