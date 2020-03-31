function InitialCash() {

    var currencies = [5, 10, 25, 50, 100, 1000, 2000, 5000, 10000, 20000, 50000];
    var acum = 0;

    for (var i in currencies) {
        var currencyQuantity = document.getElementById(currencies[i]).value != null ? document.getElementById(currencies[i]).value : 0;
        acum += (currencyQuantity * currencies[i]);
    }

    var cashInput = document.getElementById("Cash");
    cashInput.value = acum;

}

function fillTypes(id) {
    openLoader();
    $.ajax({
        type: "GET",
        url: "Pays/GetTypes",
        data: { payType: id },
        cache: false
    })
        .then(function (data) {
            setTimeout(function myfunction() {
                var dplPayNames = document.getElementById("payName");
                var payNames = JSON.parse(data);
                dplPayNames.innerHTML = drawPayTypes(payNames);
                closeLoader();

            }, 1000);


        })
        .fail(function (data) {
            alert('No');
            closeLoader();

        })

    return false;
}

function drawPayTypes(payNames) {

    var content = "<option selected disabled >Seleccione un tipo</option>";

    for (var i = 0; i < payNames.length; i++) {
        content +=
            `<option value="${payNames[i].Value}">${payNames[i].Text}</option>`;
    }

    return content;
}

function validate() {
    var amount = validateAmount("Amount");
    if (amount) {
        var concept = validateConcept();
    }

    if (amount && concept) {
        ActionController("Pays/AddPay", "#Pay");
    }
}

function validateAmount(id) {
    var element = document.getElementById(id);
    var label = document.getElementById("payLabel");
    if (!(parseInt(element.value) > 0)) {
        label.innerHTML = "Debe agregar un monto!!!";
        return false;
    }
    label.innerHTML = "";
    return true;
}

function validateConcept() {
    var element = document.getElementById("payName");
    var label = document.getElementById("payLabel");
    if (!(parseInt(element.value) != 0)) {
        label.innerHTML = "Debe agregar un tipo de pago!!!";
        return false;
    }
    label.innerHTML = "";
    return true;
}
