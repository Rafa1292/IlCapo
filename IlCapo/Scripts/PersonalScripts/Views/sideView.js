function getSideView(side, productId, quantityValue) {
    let view =
        `
            <small class="mx-2" id="${productId}${quantityValue}${side.Id}">
             ${side.Name} 
            </small>
        `;

    let frag = document.createDocumentFragment();
    frag = document.createRange().createContextualFragment(view);

    return frag;
}