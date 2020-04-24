function getSidesView(product, drawSideContainerBool, selectedSides, quantity) {
    if (drawSideContainerBool) {
        drawSideContainer(product);
    }
    setOriginalPositionModal();
    let view = createSidesView(product, selectedSides, quantity);
    let viewContainer = document.getElementById("modalBody");
    viewContainer.innerHTML = "";
    viewContainer.appendChild(view);
    $('#billModalAux').modal('show');   
}

function createSidesView(product, selectedSides, quantity) {
    let element = document.createElement("div");
    element.classList.add("col-12", "d-flex", "flex-wrap");
    let sidesTitle = document.createElement("h4");
    sidesTitle.classList.add("col-12", "text-center", "text-success");
    sidesTitle.innerHTML = "Acompañamientos";
    let sidesAsk = document.createElement("h6");
    sidesAsk.classList.add( "col-md-12", "font-weight-bold", "text-center", "p-4");
    sidesAsk.innerHTML = "Seleccione los acompañamientos solicitados";
    element.appendChild(sidesTitle);
    element.appendChild(sidesAsk);

    // condicionar cantidad de acompañamientos

    for (var i = 0; i < sides.length; i++) {
        let sideClass = "notSelected";
        if (selectedSides != undefined) {
            sideClass = selectedSides.includes(sides[i].Id) ? "selected" : "notSelected";
        }
        let btnContainer = document.createElement("div");
        btnContainer.classList.add("d-flex", "flex-wrap", "justify-content-center", "col-12", "p-1");
        let button = document.createElement("button");
        button.setAttribute("type", "button");
        button.setAttribute("id", `side-${sides[i].Id}`);
        button.setAttribute("onclick", `clickSide(${product.Id}, ${sides[i].Id}, ${quantity})`);
        button.classList.add("btn", "btn-outline-success", "col-4", sideClass);
        button.innerHTML = sides[i].Name;
        btnContainer.appendChild(button);
        element.appendChild(btnContainer);
    }

    return element;
}

function clickSide(productId, sideId, quantity) {

    let quantityValue = 0;

    if (quantity == 0) {
        let quantityId = `quantity${productId}`;
        quantityValue = parseInt(document.getElementById(quantityId).innerHTML);
    }
    else {
        quantityValue = quantity;
    }

    let product = selectProduct(productId);
    let exists = switchClass(sideId);

    if (!exists) {
        if (verifyQuantity(product, quantityValue)) {
            drawSideLabel(product, quantityValue, sideId);
        }
        else {
            alert(`Maximo ${product.SidesQuantity} acompañamientos`);
            switchClass(sideId);
        }
    }
    else {
        deleteSideLabel(product, quantityValue, sideId);
    }
}

function drawSideContainer(product) {
    let quantityId = `quantity${product.Id}`;
    let quantityValue = parseInt(document.getElementById(quantityId).innerHTML);
    let sideLabelContainerId = 1;
    let sidesContainer = document.getElementById(`sidesContainer${product.Id}`);

    if (quantityValue > 1) {
        sideLabelContainerId = quantityValue;
    }

    var content = `<div class="d-flex p-2 mx-1 my-2 col-3 pr-4 rounded border flex-wrap text-center background-light position-relative" style="min-height: 40px;" id="sideLabelContainer${product.Id}${sideLabelContainerId}"><i class="fas fa-pencil-alt side-edit" onclick="editSides(${product.Id}${sideLabelContainerId},${product.Id},${quantityValue})"></i></div>`;
    let frag = document.createDocumentFragment();
    frag = document.createRange().createContextualFragment(content);
    sidesContainer.appendChild(frag);
}

function verifyQuantity(product, quantityValue) {
    let sideLabelContainerId = `sideLabelContainer${product.Id}${quantityValue}`;
    let sideLabelContainer = document.getElementById(sideLabelContainerId);
    let selectedSidesElements = sideLabelContainer.getElementsByTagName("small");

    if (selectedSidesElements.length < product.SidesQuantity) {
        return true;
    }

    return false;
}

function drawSideLabel(product, quantityValue, sideId) {
    let sidesLabelContainer = document.getElementById(`sideLabelContainer${product.Id}${quantityValue}`);
    let side = sides.filter(x => x.Id == sideId);
    let content = getSideView(side[0], product.Id, quantityValue);
    sidesLabelContainer.prepend(content);
}

function switchClass(sideId) {
    let sideButtonId = `side-${sideId}`;
    let button = document.getElementById(sideButtonId);
    let selected = button.classList.contains("selected");

    if (selected) {
        button.classList.remove("selected");
        return true;
    }
    else {
        button.classList.add("selected");
        return false;
    }
}

function deleteSideLabel(product, quantityValue, sideId) {
    let labelId = `${product.Id}${quantityValue}${sideId}`;
    let label = document.getElementById(labelId);
    let labelContainer = label.parentNode;
    labelContainer.removeChild(label);
}

function editSides(containerId, productId, quantity) {
    containerId = `sideLabelContainer${containerId}`;
    let product = selectProduct(productId);
    let selectedSidescontainer = document.getElementById(containerId);
    let selectedSideselements = selectedSidescontainer.getElementsByTagName("small");
    let selectedSides = [];
    for (var i = 0; i < selectedSideselements.length; i++) {
        let sideId = selectedSideselements[i].id;
        sideId = sideId.substr(2, 2);
        selectedSides.push(parseInt(sideId));
    }

    getSidesView(product, false, selectedSides, quantity);

}

function deleteSideContainer(productId, currentQuantity) {
    let sideLabelContainerId = `sideLabelContainer${productId}${currentQuantity}`;
    let sideLabelContainer = document.getElementById(sideLabelContainerId);
    let sidesContainer = sideLabelContainer.parentNode;
    sidesContainer.removeChild(sideLabelContainer);
    alert("Verifique que los acompañamientos seleccionados sean los indicados!!!");
}