function updateSubTotal() {
    let elements = document.getElementsByName("totalPrice");
    let price = 0;
    let subtotal = document.getElementsByName("subtotal");

    for (var i = 0; i < elements.length; i++) {
        if (elements[i].parentNode.classList.contains("billable")) {
            price += parseInt(elements[i].innerHTML);
        }
    }

    for (var i = 0; i < subtotal.length; i++) {

        subtotal[i].innerHTML = `${price}`;
    }
}

function updateExtras() {
    let billExtras = document.getElementsByName("extraFinalPrice");
    let extrasAmount = 0;
    let extraLabel = document.getElementsByName("extras");

    for (var i = 0; i < billExtras.length; i++) {
        if (billExtras[i].parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.classList.contains("billable")) {
            extrasAmount += parseInt(billExtras[i].value);
        }
    }

    for (var i = 0; i < extraLabel.length; i++) {

        extraLabel[i].innerHTML = extrasAmount;
    }
}

function updateTax() {
    let toGo = document.getElementById("toGo");
    let taxeService = document.getElementsByName("taxService");
    let taxeSale = document.getElementsByName("taxSale");
    let tax = document.getElementsByName("impuestos");
    let taxAmount = 0;

    for (var i = 0; i < taxeSale.length; i++) {
        taxAmount += parseInt(taxeSale[i].value);
    }

    if (toGo.value === "False") {
        for (var i = 0; i < taxeService.length; i++) {
            taxAmount += parseInt(taxeService[i].value);
        }
    }

    for (var i = 0; i < tax.length; i++) {

        tax[i].innerHTML = taxAmount;
    }
}

function updateTotal() {
    let subtotal = parseInt(document.getElementById("subtotal").innerHTML);
    let impuestos = parseInt(document.getElementById("impuestos").innerHTML);
    let descuento = parseInt(document.getElementById("descuento").innerHTML);
    let extra = parseInt(document.getElementById("extras").innerHTML);
    let total = document.getElementsByName("total");
    let price = subtotal + extra + impuestos - descuento;

    for (var i = 0; i < total.length; i++) {

        total[i].innerHTML = `${price}`;
    }

}

function createDiscountView() {
    let resultContainer = document.createElement("div");
    let discountHeader = document.createElement("div");
    let discountTitle = document.createElement("h5");
    let titleStyle = document.createElement("strong");
    let discountlegend = document.createElement("small");
    let discountContainer = document.createElement("div");
    let discountAsk = document.createElement("h6");
    let discountInput = document.createElement("input");
    let buttonContainer = document.createElement("div");
    let discountButton = document.createElement("button");

    resultContainer.classList.add("col-12", "flex-wrap", "d-flex", "justify-content-center");
    discountHeader.classList.add("col-12", "text-center", "justify-content-center", "mb-5");
    discountContainer.classList.add("col-12", "d-flex", "flex-wrap", "justify-content-center", "text-center");
    discountAsk.classList.add("col-12", "mb-3");
    discountInput.classList.add("col-2", "rounded", "mb-3");
    buttonContainer.classList.add("col-12", "d-flex", "justify-content-center");
    discountButton.classList.add("btn", "btn-success");
    discountButton.setAttribute("onclick", "applyDiscount()");
    discountInput.setAttribute("id", "discountAmount");

    titleStyle.innerHTML = "Aplicar descuento";
    discountlegend.innerHTML = "Estas a punto de aplicar un descuento a la factura actual, recuerda que debes contar con los permisos necesarios"
    discountAsk.innerHTML = "Ingresa el monto de descuento";
    discountButton.innerHTML = "Aplicar";

    discountTitle.appendChild(titleStyle);
    discountHeader.appendChild(discountTitle);
    discountHeader.appendChild(discountlegend);

    discountContainer.appendChild(discountAsk);
    discountContainer.appendChild(discountInput);

    buttonContainer.appendChild(discountButton);

    resultContainer.appendChild(discountHeader);
    resultContainer.appendChild(discountContainer);
    resultContainer.appendChild(buttonContainer);

    return resultContainer;
}

function getDiscountView() {
    
    let view = createDiscountView();
    let viewContainer = document.getElementById("modalBody");
    viewContainer.innerHTML = "";
    viewContainer.appendChild(view);
    $('#billModalAux').modal('show');
}

function applyDiscount() {
    let discountInput = document.getElementById("discount");
    let discountPercentage = parseInt(document.getElementById("discountAmount").value);
    let subtotal = parseInt(document.getElementById("subtotal").innerHTML);
    let impuestos = parseInt(document.getElementById("impuestos").innerHTML);
    let extra = parseInt(document.getElementById("extras").innerHTML);
    discountInput.value = discountPercentage;
    let discount = document.getElementsByName("descuento");
    let discountAmount = discountPercentage * (subtotal + impuestos + extra) / 100;

    for (var i = 0; i < discount.length; i++) {

        discount[i].innerHTML = discountAmount;
    }
    amountsManager(discountPercentage);
    $('#billModalAux').modal('hide');

}

function amountsManager() {
    updateSubTotal();
    updateExtras();
    updateTax();
    updateTotal();
}

function getSliceAccountView() {
    setOriginalPositionModal();
    let view = createSliceAccountView();
    let viewContainer = document.getElementById("modalBody");
    viewContainer.innerHTML = "";
    viewContainer.appendChild(view);
    $('#billModalAux').modal('show');
}

function createSliceAccountView() {
    let viewContainer = document.createElement("div");
    viewContainer.classList.add("col-md-12", "p-1", "d-flex", "flex-wrap", "justify-content-center", "text-center");

    let sliceAsk = document.createElement("label");
    sliceAsk.classList.add("mx-2", "col-md-12", "font-weight-bold");
    sliceAsk.innerHTML = "¿En cuantas partes deseas dividir la cuenta?";

    let sliceInput = document.createElement("input");
    sliceInput.classList.add("form-control", "rounded");
    sliceInput.setAttribute("type", "number");
    sliceInput.setAttribute("id", "sliceInput");

    let buttonContainer = document.createElement("div");
    buttonContainer.classList.add("col-md-12", "d-flex", "flex-wrap", "justify-content-center", "p-4");
    let button = document.createElement("button");
    button.setAttribute("type", "button");
    button.setAttribute("onclick", "SliceAccount()");
    button.innerHTML = "Aplicar";
    button.classList.add("btn", "btn-success");

    let sliceContainer = document.createElement("div");
    sliceContainer.classList.add("col-md-12", "d-flex", "flex-wrap");
    sliceContainer.setAttribute("id", "sliceAccountBody");

    buttonContainer.appendChild(button);
    viewContainer.appendChild(sliceAsk);
    viewContainer.appendChild(sliceInput);
    viewContainer.appendChild(buttonContainer);
    viewContainer.appendChild(sliceContainer);
    return viewContainer;
}

function SliceAccount() {
    let quantity = parseInt(document.getElementById("sliceInput").value);
    let price = parseInt(document.getElementById("total").innerHTML);

    openLoader();

    $.ajax({
        type: "GET",
        url: "Bills/SliceAccount",
        data: {
            quantity: quantity,
            price: price
        },
        cache: false
    })
        .then(function (data) {
            var modalBody = document.getElementById("sliceAccountBody");
            modalBody.innerHTML = data;
            closeLoader();

        })
        .fail(function (data) {
            alert('No');
            closeLoader();

        })

    return false;
}

function separateAccount() {
    let billId = parseInt(document.getElementById("orderNumber").innerHTML);

    openLoader();

    $.ajax({
        type: "GET",
        url: "Bills/SeparateAccount",
        data: {
            billId: billId,
        },
        cache: false
    })
        .then(function (data) {
            hidePaid();
            document.getElementById("bill-body").innerHTML = data;
            closeLoader();

        })
        .fail(function (data) {
            alert('No');
            closeLoader();

        })

    return false;




}

function ShowPaid() {

    if (isComanded()) {

        let paysViewContainer = document.getElementById("payViewContainer");
        paysViewContainer.style.transition = "top 0s ease, background 1s ease";
        paysViewContainer.style.top = "0";
        paysViewContainer.style.background = "rgba(0, 0, 0, .7)";
        let paysView = document.getElementById("payView");
        paysView.style.top = "25vh";
    }
    else {
        alert("comanda enviada exitosamente");
    }
}

function hidePaid() {
    let paysView = document.getElementById("payView");
    paysView.style.top = "-50vh";
    let paysViewContainer = document.getElementById("payViewContainer");
    paysViewContainer.style.transition = "top 3s ease, background 100ms ease-in";
    paysViewContainer.style.background = "transparent";
    paysViewContainer.style.top = "-100vh";
}

function productDragStart(box, e) {

    event.dataTransfer.setData("Data", box.id);
}

function dropProduct(target, e) {

    var boxId = event.dataTransfer.getData("Data");
    var products = Array.from(document.getElementsByName("BillProducts"));
    products.forEach(x => x.id == boxId ? makeProductBillable(target, x) : "");
    amountsManager();
}

function makeProductBillable(target, product) {
    target.appendChild(product);
    switchBillableClass(product);

}

function switchBillableClass(product) {
    product.classList.toggle("billable");
    product.classList.toggle("notBillable");

}

function isComanded() {
    var command = document.getElementById("command");
    let elements = document.getElementsByName("totalPrice");

    if (command == null) {
        command = true;
    }

    if (command.value == "false" && elements.length > 0) {
        commandBill();
        closeLoader();
        return false;
    }

    return true;
}

function selectPayMethod(id) { 
    var price = parseInt(document.getElementById("total").innerHTML);
    var methodInput = document.getElementById(id);
    methodInput.value = price;
}
