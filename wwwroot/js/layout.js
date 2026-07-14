document.addEventListener("DOMContentLoaded", () => {

    inicializarNavbar();

});

function inicializarNavbar() {

    const navbar = document.getElementById("mainNavbar");

    if (!navbar) return;

    actualizarEstadoNavbar(navbar);

    window.addEventListener("scroll", () => {

        actualizarEstadoNavbar(navbar);

    });

}

function actualizarEstadoNavbar(navbar) {

    navbar.classList.toggle("scrolled", window.scrollY > 50);

}