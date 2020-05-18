function showExtrasContainer(id) {
    var container = document.getElementById(`extrasContainer${id}`);
    container.style.opacity = "1";
    container.style.display = "flex";

}

function hideExtrasContainer(id) {
    var container = document.getElementById(`extrasContainer${id}`);
    container.style.display = "none";
}

function reduceExtraQuantity(id) {

    let extraQuantityContainerId = `extraQuantity${id}`;
    let extraQuantityContainer = document.getElementById(extraQuantityContainerId);
    let extraQuantity = parseInt(extraQuantityContainer.innerHTML);
    let newQuantity = extraQuantity - 1;
    if (extraQuantity > 0) {
        extraQuantityContainer.innerHTML = newQuantity;
    }

    updateExtraFinalPrice(id, newQuantity);
    amountsManager();
}

function addExtraToBill(id) {

    let extraQuantityContainerId = `extraQuantity${id}`;
    let extraQuantityContainer = document.getElementById(extraQuantityContainerId);
    let extraQuantity = parseInt(extraQuantityContainer.innerHTML);
    let newQuantity = extraQuantity + 1;
    extraQuantityContainer.innerHTML = newQuantity;

    updateExtraFinalPrice(id, newQuantity);
    amountsManager();
}

function updateExtraFinalPrice(id, newQuantity) {

    let extraUnitPriceInput = document.getElementById(`extraUnitPrice${id}`);
    let extraUnitPrice = extraUnitPriceInput.value;

    let extraFinalPriceInput = document.getElementById(`extraFinalPrice${id}`);
    extraFinalPriceInput.value = newQuantity * extraUnitPrice;
}

function getExtrasView(quantity, productId) {
    let checks = "";
    let id = `extraContainer${productId}${quantity}`;
    for (var i = 0; i < extras.length; i++) {
        checks += `  <div class=" col-12 d-flex flex-wrap p-0 mb-2">
                        <div class="col-md-7 p-0 d-flex justify-content-start align-items-center">
                            <input type="number" value="${extras[i].Price}"  name="extraUnitPrice" class="d-none" id="extraUnitPrice${productId}${quantity}${extras[i].Id}">
                            <input type="number" value="0" name="extraFinalPrice" class="d-none" id="extraFinalPrice${productId}${quantity}${extras[i].Id}">
                            <label class="m-0" for="extraCheck${productId}${quantity}${extras[i].Id}">${extras[i].Name} ¢${extras[i].Price}</label>
                        </div>

                        <div class="col-md-5 p-0 d-flex justify-content-start align-items-center">
                            <i class="fas fa-minus value-manager red text-white" onclick="reduceExtraQuantity(${productId}${quantity}${extras[i].Id})"></i>
                            <span class="mx-2" id="extraQuantity${productId}${quantity}${extras[i].Id}">
                                0
                            </span>
                            <i class="fas fa-plus value-manager text-white" onclick="addExtraToBill(${productId}${quantity}${extras[i].Id})"></i>
                        </div>

                    </div>`;
    }

    let content = `  <div class="d-flex flex-wrap p-0 border-right border-white mr-1 mt-2" id="${id}" style="font-size: 10px; width: 31%; max-width: 50%;">
                        <div class="col-12 flex-wrap d-flex justify-content-center p-0">
                           ${checks}
                        </div>
                     </div>`;

    let frag = document.createDocumentFragment();
    frag = document.createRange().createContextualFragment(content);

    return frag;
}

function addExtra(quantity, productId) {
    let extrasContainer = document.getElementById(`extrasContainer${productId}`);
    let extraContainer = getExtrasView(quantity, productId);

    extrasContainer.appendChild(extraContainer);
}

function deleteExtra(productId, quantity) {
    let id = `extraContainer${productId}${quantity}`;
    let container = document.getElementById(id);
    let containerParent = container.parentNode;
    containerParent.removeChild(container);
}