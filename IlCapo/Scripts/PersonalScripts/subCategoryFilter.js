function selectSubCategory(id) {
    $.ajax({
        type: "GET",
        url: "ProductSubCategories/GetProducts",
        data: { productSubCategoryId: id },
        cache: false
    })
        .then(function (data) {
            darwProducts(data);
        })
        .fail(function (data) {
            alert('No');

        })
}

function darwProducts(data) {

    var ProductsContainer = document.getElementById("search-result");
    productsList = JSON.parse(data);
    var content = "";
    for (var i = 0; i < productsList.length; i++) {
        content +=
            `<div class="col-md-12 my-2 p-3 rounded nebulosa-btn  select-product text-center justify-content-center" 
            onclick="addProductToBill(${productsList[i].Id})">
             ${productsList[i].Name}
            </br>
             <small>${productsList[i].SubCategory}</small>
             </div>`
    }

    ProductsContainer.innerHTML = content;
}