function updateSubTotal() {
    let elements = document.getElementsByName("totalPrice");
    let price = 0;
    let subtotal = document.getElementById("subtotal");

    for (var i = 0; i < elements.length; i++) {
        price += parseInt(elements[i].innerHTML);
    }

    subtotal.innerHTML = `${price}`;
}

function updateTax() {
    let toGo = document.getElementById("toGo");
    let taxeService = document.getElementsByName("taxService");
    let taxeSale = document.getElementsByName("taxSale");
    let tax = document.getElementById("impuestos");
    let taxAmount = 0;

    for (var i = 0; i < taxeSale.length; i++) {
        taxAmount += parseInt(taxeSale[i].value);
    }

    if (toGo.value === "False") {
        for (var i = 0; i < taxeService.length; i++) {
            taxAmount += parseInt(taxeService[i].value);
        }
    }

    tax.innerHTML = taxAmount;
}

function updateTotal() {
    let subtotal = parseInt(document.getElementById("subtotal").innerHTML);
    let impuestos = parseInt(document.getElementById("impuestos").innerHTML);
    let descuento = parseInt(document.getElementById("descuento").innerHTML);
    let total = document.getElementById("total");
    let price = subtotal + impuestos - descuento;
    total.innerHTML = `${price}`;

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
    let discountPercentage = parseInt(document.getElementById("discountAmount").value);
    let subtotal = parseInt(document.getElementById("subtotal").innerHTML);
    let impuestos = parseInt(document.getElementById("impuestos").innerHTML);

    let discount = document.getElementById("descuento");
    let discountAmount = discountPercentage * (subtotal + impuestos) / 100;
    discount.innerHTML = discountAmount;
    amountsManager(discountPercentage);
    $('#billModalAux').modal('hide');

}

function amountsManager() {
    updateSubTotal();
    updateTax();
    updateTotal();
}

