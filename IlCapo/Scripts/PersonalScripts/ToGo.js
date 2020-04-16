//function carga() {

//    contador_s = 0;
//    contador_m = 0;

//    s = document.getElementById("segundos");
//    m = document.getElementById("minutos");


//    cronometro = setInterval(
//        function () {
//            if (contador_s == 60) {
//                contador_s = 0;
//                contador_m++;
//                m.innerHTML = contador_m;

//                if (contador_m == 60) {
//                    contador_m = 0;
//                }

//            }
//            s.innerHTML = contador_s;
//            contador_s++;
//        }
//        , 1000);
//}

//window.onload = carga();

function express(id) {
    let motorcycle = document.getElementById("motorcycle");
    let walking = document.getElementById("walking");
    let inputExpress = document.getElementById("express");
    if (id == "motorcycle") {
        inputExpress.value = true;
        motorcycle.style.backgroundColor = "#ffc107";
        walking.style.backgroundColor = "#fff";
    }
    else {
        inputExpress.value = false;
        walking.style.backgroundColor = "#ffc107";
        motorcycle.style.backgroundColor = "#fff";
    }

}