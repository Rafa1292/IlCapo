

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

$(function () {
    var cronos = document.getElementsByClassName("crono");
    for (var i = 0; i < cronos.length; i++) {
        timeElapsed(cronos[i].id);
    }
    setInterval(() => {
        var cronos = document.getElementsByClassName("crono");
        for (var i = 0; i < cronos.length; i++) {
            timeElapsed(cronos[i].id);
        }
    }, 60000);
});




function timeElapsed(id) {
    var inicio = document.getElementById(id).innerHTML;
    var parts = inicio.split(":");
    var hours = (new Date()).getHours();
    var minutes = (new Date()).getMinutes();
    var elapsedHours = hours - parseInt(parts[0]);
    var elapsedMinutes = minutes - parseInt(parts[1]);
    if (elapsedMinutes < 0) {
        elapsedHours--;
        elapsedMinutes += 60;
    }

    var elapsedTime = `${elapsedHours}:${elapsedMinutes}`
    var timeId = `time-${id}`;
    var time = document.getElementById(timeId);
    time.innerHTML = elapsedTime;
}