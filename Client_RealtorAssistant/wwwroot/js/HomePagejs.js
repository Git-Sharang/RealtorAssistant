const sign_in_btn = document.querySelector("#sign-in-btn");
const sign_up_btn = document.querySelector("#sign-up-btn");
const container = document.querySelector(".container");

sign_up_btn.addEventListener("click", () => {
    container.classList.add("sign-up-mode");
});

sign_in_btn.addEventListener("click", () => {
    container.classList.remove("sign-up-mode");
});

/*
var error = $("#error").data("value");

document.getElementById('#loginInput').onclick = function () {
    if (error !== null) {
        Swal.fire({
            title: 'Error!',
            text: '$error',
            
        })
    }
}
*/

var output = '@(ViewBag.result)'

/*

$("#error").click(function () {
    console.log('@ViewBag.result');
    if (output !== null) {
        Swal.fire({
            title: 'Error!',
            text: '$error',

        })
    }
})
*/