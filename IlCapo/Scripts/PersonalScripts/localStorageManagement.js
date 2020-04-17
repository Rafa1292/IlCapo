function sendToStorage(key, id) {
    let input = document.getElementById(id);
    localStorage.setItem(key, input.value);

}

function getFromStorage(key, id) {
    let value = localStorage.getItem(key);
    let itemShow = document.getElementById(id);
    itemShow.value = value;
}