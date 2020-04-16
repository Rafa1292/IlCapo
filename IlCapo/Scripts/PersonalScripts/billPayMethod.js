function updateSubTotal() {
    let elements = document.getElementsByName("totalPrice");
    let price = 0;
    let subtotal = document.getElementById("subtotal");
    for (var i = 0; i < elements.length; i++) {
        price += parseInt(elements[i].innerHTML);
    }

    subtotal.innerHTML = `${price}`;
    updateTax();
    
    updateTotal();
}

function updateTotal() {
    let subtotal = parseInt(document.getElementById("subtotal").innerHTML);
    let impuestos = parseInt(document.getElementById("impuestos").innerHTML);
    let descuento = parseInt(document.getElementById("descuento").innerHTML);
    let total = document.getElementById("total");
    let price = subtotal + impuestos - descuento;
    total.innerHTML = `${price}`;

}

function updateTax() {
    let toGo = document.getElementById("toGo").value;
    let taxes = document.getElementsByClassName("tax");
    let tax = document.getElementById("impuestos");

    let taxAmount = 0;

    for (var i = 0; i < taxes.length; i++) {
        taxAmount += parseInt(taxes[i].value);
    }

    tax.innerHTML = taxAmount;
}