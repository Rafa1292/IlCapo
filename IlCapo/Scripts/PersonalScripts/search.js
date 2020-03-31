var products;

window.onload = function () {
    $.ajax({
        type: "GET",
        url: "Products/GetProducts",
        cache: false
    })
        .then(function (data) {
            products = JSON.parse(data);
        })
        .fail(function (data) {
            alert('Imposible obtener lista de articulos');

        })

    return false;
}

function search() {
    var text = document.getElementById("search").value.toLowerCase();
    var results = document.getElementById("search-result");
    results.innerHTML = "";
    if (text != "") {

        for (let product of products) {
            let name = product.Name.toLowerCase();
            if (name.indexOf(text) !== -1) {
                results.innerHTML += `<div class="col-md-12 my-2 p-3 rounded  select-product text-center justify-content-center">
                                ${product.Name}
                            <br />
                            <small>${product.SubCategory}</small>
                            </div>`
            }
        }
    }

}