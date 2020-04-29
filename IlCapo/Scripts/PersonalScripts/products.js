function addProductToBill(id) {
    let product = selectProduct(id);
    let exists = verifyExistence(product);

    if (exists) {
        let newQuantity = setQuantity(product);
        addExtra(newQuantity, id);
        updateTotalPrice(product, newQuantity);
        setTax(product, newQuantity);
    }
    else {
        drawBillProducts(product);
        addExtra(1, id);
    }

    //cambiar al actualizar create de products
    if (!product.Sides) {
        getSidesView(product, true, undefined, 0);
    }


    amountsManager();
}

function selectProduct(id) {
    let product = products.filter(x => x.Id == id);
    return product[0];
}

function verifyExistence(product) {

    let elements = document.getElementsByName("BillProducts")
    let exists = false

    for (var i = 0; i < elements.length; i++) {
        if (elements[i].id == product.Id) {
            exists = true;
            break;
        }
    }

    return exists;
}

function setQuantity(product) {
    let quantityId = `quantity${product.Id}`;
    let element = document.getElementById(quantityId);
    let currentQuantity = parseInt(element.innerHTML);
    let newQuantity = currentQuantity + 1;
    element.innerHTML = newQuantity;

    return newQuantity;
}

function setTax(product, quantity) {
    let inputTaxService = document.getElementById(`taxService${product.Id}`);
    let inputTaxSale = document.getElementById(`taxSale${product.Id}`);
    let taxService = getServiceTax(product);
    let taxSale = getSaleTax(product);
    inputTaxService.value = taxService * quantity;
    inputTaxSale.value = taxSale * quantity;
}

function updateTotalPrice(product, quantity) {
    let totalPriceId = `totalPrice${product.Id}`;
    let element = document.getElementById(totalPriceId);
    let price = parseInt(product.Price);
    let newPrice = price * quantity;
    element.innerHTML = newPrice;
}

function drawBillProducts(product) {
    var billProductsContainer = document.getElementById("billProductsContainer");
    let newProduct = getProductView(product);  
    billProductsContainer.prepend(newProduct);
}

function getSaleTax(product) {
    let tax = 0;

    for (var i = 0; i < product.Taxes.length; i++) {

        if (product.Taxes[i].Name.toLowerCase() == "impuesto de venta") {
            tax = product.Price * product.Taxes[i].Percentage / 100;
        }
    }

    return tax;
}

function getServiceTax(product) {
    let tax = 0;

    for (var i = 0; i < product.Taxes.length; i++) {

        if (product.Taxes[i].Name.toLowerCase() == "impuesto de servicio") {
            tax = product.Price * product.Taxes[i].Percentage / 100;
        }
    }

    return tax;
}

function reduceProductQuantity(productId) {
    let product = selectProduct(productId);
    let quantityId = `quantity${productId}`;
    let element = document.getElementById(quantityId);
    let currentQuantity = parseInt(element.innerHTML);
    if (currentQuantity > 1) {
        let newQuantity = currentQuantity - 1;
        element.innerHTML = newQuantity;
        updateTotalPrice(product, newQuantity);
        setTax(product, newQuantity);
        amountsManager();
        deleteSideContainer(productId, currentQuantity);
        deleteExtra(productId, currentQuantity);
    }
    else {
        alert("La cantidad minima es de 1");
    }

}

function deleteProduct(productId) {
    let productsContainer = document.getElementById("billProductsContainer");
    let products = productsContainer.getElementsByTagName("div");
    for (var i = 0; i < products.length; i++) {
        if (products[i].id == productId) {
            productsContainer.removeChild(products[i]);
        }
    }
    amountsManager();
}



