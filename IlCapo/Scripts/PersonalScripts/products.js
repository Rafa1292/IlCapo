function addProductToBill(id) {
    let product = selectProduct(id);
    verifyExistence(product);
    amountsManager();
}

function selectProduct(id) {
    let product = products.filter(x => x.Id == id);
    return product[0];
}

function verifyExistence(product) {

    let elements = document.getElementsByName("BillProducts")
    let exist = false

    for (var i = 0; i < elements.length; i++) {
        if (elements[i].id == product.Id) {
            exist = true;
            break;
        }
    }

    if (exist) {
        setQuantity(product);
    }
    else {
        drawBillProducts(product);
    }
}

function setQuantity(product) {
    let quantityId = `quantity${product.Id}`;
    let element = document.getElementById(quantityId);
    let currentQuantity = parseInt(element.innerHTML);
    let newQuantity = currentQuantity + 1;
    element.innerHTML = newQuantity;
    updateTotalPrice(product, newQuantity);  
    setTax(product, newQuantity);
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
    let newProduct = createElement(product);    
    billProductsContainer.append(newProduct);
}

function createElement(product) {

    let productContainer = document.createElement("div");
    let check = document.createElement("div");
    let inputCheck = document.createElement("input");
    let name = document.createElement("div");
    let quantity = document.createElement("div");
    let price = document.createElement("div");
    let taxService = document.createElement("input");
    let taxSale = document.createElement("input");
    let totalPrice = document.createElement("div");
    let observation = document.createElement("div");
    let inputObservation = document.createElement("input");

    productContainer.classList.add("col-12", "bill-item", "d-flex", "flex-wrap", "text-white", "text-center", "border-white");
    productContainer.setAttribute("id", product.Id);
    productContainer.setAttribute("name", "BillProducts");
    check.classList.add("col-1", "justify-content-center", "align-items-center", "bill-item-check");
    inputCheck.classList.add("form-check");
    inputCheck.setAttribute("type", "checkbox");
    name.classList.add("col-3", "p-0");
    quantity.classList.add("col-2", "p-0");
    quantity.setAttribute("id", `quantity${product.Id}`);
    price.classList.add("col-1", "p-0");
    taxService.classList.add("d-none");
    taxService.setAttribute("name", "taxService");
    taxService.setAttribute("id", `taxService${product.Id}`)
    taxSale.classList.add("d-none");
    taxSale.setAttribute("name", "taxSale");
    taxSale.setAttribute("id", `taxSale${product.Id}`);
    totalPrice.classList.add("col-2", "p-0", "billable");
    totalPrice.setAttribute("id", `totalPrice${product.Id}`);
    totalPrice.setAttribute("name", "totalPrice");
    observation.classList.add("col-3", "p-0");
    inputObservation.classList.add("form-control", "rounded");
    inputObservation.setAttribute("type", "text");

    name.innerText = product.Name;
    quantity.innerText = 1;
    price.innerText = product.Price;
    totalPrice.innerText = product.Price;
    taxService.value = getServiceTax(product);
    taxSale.value = getSaleTax(product);

    check.appendChild(inputCheck);
    price.appendChild(taxService);
    price.appendChild(taxSale);
    observation.appendChild(inputObservation);
    productContainer.appendChild(check);
    productContainer.appendChild(name);
    productContainer.appendChild(quantity);
    productContainer.appendChild(price);
    productContainer.appendChild(totalPrice);
    productContainer.appendChild(observation);

    return productContainer;
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