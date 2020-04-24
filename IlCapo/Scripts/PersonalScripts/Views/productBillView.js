function getProductView(product) {
    let taxService = getServiceTax(product);
    let taxSale = getSaleTax(product);
    var view =
        `<div class="col-12 bill-item d-flex flex-wrap text-white text-center border-white" id="${product.Id}" name="BillProducts">
            <div class="col-3 p-0">
                ${product.Name}
            </div>
            <div class="col-2 p-0" id="quantity${product.Id}">
                1
            </div>
            <div class="col-1 p-0 product-price">
                ${product.Price}
                <input class="d-none" name="taxService" id="taxService${product.Id}" value="${taxService}">
                <input class="d-none" name="taxSale" id="taxSale${product.Id}" value="${taxSale}">
            </div>
            <div class="col-2 p-0 billable" id="totalPrice${product.Id}" name="totalPrice">
            ${product.Price}
            </div>
            <div class="col-3 p-0">
                <input class="form-control rounded" type="text">
            </div>
            <div class="col-12 d-flex flex-wrap text-white justify-content-center font-weight-light" id="sidesContainer${product.Id}">

            </div>
          </div>`;

    let frag = document.createDocumentFragment();
    frag = document.createRange().createContextualFragment(view);

    return frag;

}




