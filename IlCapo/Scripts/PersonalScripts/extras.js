function showExtrasContainer(id) {
    var container = document.getElementById(`extrasContainer${id}`);
    container.style.opacity = "1";
    container.style.display = "flex";

}

function hideExtrasContainer(id) {
    var container = document.getElementById(`extrasContainer${id}`);
    container.style.display = "none";
}

function getExtrasView(quantity, productId) {
    let checks = "";
    let id = `extraContainer${productId}${quantity}`;
    for (var i = 0; i < extras.length; i++) {
        checks += `  <div class=" col-12 d-flex justify-content-start p-0 align-items-center">
                        <input type="checkbox" value="${extras[i].Price}" name="extras" onclick="amountsManager()" id="extraCheck${productId}${quantity}${extras[i].Id}" >
                        <label class="mx-2" for="extraCheck${productId}${quantity}${extras[i].Id}">${extras[i].Name} ¢${extras[i].Price}</label>
                    </div>`;
    }

    let content = `  <div class="d-flex col-3  flex-wrap p-0" id="${id}">
                        <div class="col-12 flex-wrap d-flex justify-content-center p-0">
                           ${checks}
                        </div>
                     </div>`;

    let frag = document.createDocumentFragment();
    frag = document.createRange().createContextualFragment(content);

    return frag;
}

function addExtra(quantity,productId) {
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