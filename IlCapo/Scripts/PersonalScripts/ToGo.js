function carga() {

    contador_s = 0;
    contador_m = 0;

    s = document.getElementById("segundos");
    m = document.getElementById("minutos");


    cronometro = setInterval(
        function () {
            if (contador_s == 60) {
                contador_s = 0;
                contador_m++;
                m.innerHTML = contador_m;

                if (contador_m == 60) {
                    contador_m = 0;
                }

            }
            s.innerHTML = contador_s;
            contador_s++;
        }
        , 1000);
}

window.onload = carga();