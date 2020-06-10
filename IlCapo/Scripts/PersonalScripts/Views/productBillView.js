function getProductView(product) {
    let taxService = getServiceTax(product);
    let taxSale = getSaleTax(product);
    var view =
        `<div class="col-12 bill-item billable d-flex flex-wrap text-white text-center border-white" id="${product.Id}" name="BillProducts">
         <i class="far fa-times-circle  text-danger position-absolute" onclick="deleteProduct(${product.Id})" style="z-index: 10"></i>
            <div class="col-3 p-0">
                ${product.Name}
            </div>
            <div class="col-3 p-0 justify-content-center" >
                <i class="fas fa-minus value-manager red" onclick="reduceProductQuantity(${product.Id})"></i>
                <span class="mx-2" id="quantity${product.Id}">
                    1
                </span>
                <i class="fas fa-plus value-manager" onclick="addProductToBill(${product.Id})"></i>
            </div>
            <div class="col-1 p-0">
                ${product.Price}
                <input class="d-none" name="taxService" id="taxService${product.Id}" value="${taxService}">
                <input class="d-none" name="taxSale" id="taxSale${product.Id}" value="${taxSale}">
            </div>
            <div class="col-2 p-0 billable" id="totalPrice${product.Id}" name="totalPrice">
            ${product.Price}
            </div>
            <div class="col-3 p-0">
                <input class="form-control rounded" id="observation${product.Id}" type="text">
            </div>
            <div class="col-12 d-flex flex-wrap text-white justify-content-center font-weight-light">
                <button type="button" class="btn justify-content-center p-0 align-content-center btn-outline-success text-center text-white d-flex col-1 border-white extra-button" onmouseover="showExtrasContainer(${product.Id})" onmouseout="hideExtrasContainer(${product.Id})">Extras</button>
                <div class="position-absolute flex-wrap extras-container" onmouseover="showExtrasContainer(${product.Id})"  onmouseout="hideExtrasContainer(${product.Id})" id="extrasContainer${product.Id}">

                </div>
                <div class="col-11 d-flex flex-wrap text-white justify-content-center font-weight-light" id="sidesContainer${product.Id}">
                </div>
            </div>

          </div>`;

    let frag = document.createDocumentFragment();
    frag = document.createRange().createContextualFragment(view);

    return frag;

}




