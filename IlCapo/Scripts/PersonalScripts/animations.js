
var room = document.getElementById("ordersToGo");
var containerBtn = document.getElementById("OrdersToGoBtnContainer")
var arrowsRight = document.getElementsByClassName("arrow-right");
var arrowsLeft = document.getElementsByClassName("arrow-left");



function HideOrdersToGo() {

    room.style.transform = "translateX(-38vw)";
    containerBtn.style.transform = "translateX(19vw)";
    rotate(arrowsLeft, 180);

    setTimeout(function () {

        toggleView(arrowsLeft, "none");
        toggleView(arrowsRight, "block");
        rotate(arrowsLeft, 0);

    }, 1000)

}

function ShowOrdersToGo() {

    room.style.transform = "translateX(0)";
    containerBtn.style.transform = "translateX(0)";
    rotate(arrowsRight, 180);

    setTimeout(function () {

        toggleView(arrowsRight, "none");
        toggleView(arrowsLeft, "block");
        rotate(arrowsRight, 0);

    }, 1000)
}

function rotate(array, deg) {
    var style = `rotateY(${deg.toString()}deg)`;

    for (var i = 0; i < array.length; i++) {
        array[i].style.transform = style;
    }
}


function toggleView(array, style) {
    for (var i = 0; i < array.length; i++) {
        array[i].style.display = style;
    }
}