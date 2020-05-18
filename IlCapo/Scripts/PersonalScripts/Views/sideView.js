function getSideView(side, productId, quantityValue) {
    let view =
        `
            <small class="mx-2" id="sideLabel${productId}${quantityValue}${side.Id}">
            <input type="number" value="${side.Id}" class="d-none" id="sideInput${productId}${quantityValue}${side.Id}" />
             ${side.Name} 
            </small>
        `;

    let frag = document.createDocumentFragment();
    frag = document.createRange().createContextualFragment(view);

    return frag;
}