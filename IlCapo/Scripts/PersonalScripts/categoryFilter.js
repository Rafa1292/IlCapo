function selectCategory(id) {
    $.ajax({
        type: "GET",
        url: "ProductCategories/GetSubCategories",
        data: { productCategoryId: id },
        cache: false
    })
        .then(function (data) {
            darwSubCategories(data);
        })
        .fail(function (data) {
            alert('No');

        })
}

function darwSubCategories(data) {

    var SubCategoriesContainer = document.getElementById("subCategoriesList");
    subCategoriesList = JSON.parse(data);
    var content = "";
    for (var i = 0; i < subCategoriesList.length; i++) {
        content += 
            `<div class="col-md-12 my-2 p-1 rounded nebulosa-btn category-product text-center justify-content-center" onclick="selectSubCategory(${subCategoriesList[i].Id})">
            ${subCategoriesList[i].Name}
             </div>`
    }

    SubCategoriesContainer.innerHTML = content;
}