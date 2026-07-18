document.addEventListener("DOMContentLoaded", () => {

    const buttons = document.querySelectorAll(".menu-filters button");
    const cards = document.querySelectorAll(".food-card");
    const emptyMessage = document.getElementById("emptyMenuMessage");

    function updateEmptyMessage() {

        const visibleCards = [...cards].filter(card =>
            card.style.display !== "none"
        );

        emptyMessage.style.display =
            visibleCards.length === 0 ? "block" : "none";
    }

    function filterCards(category) {

        cards.forEach(card => {

            const show =
                category === "Todos" ||
                card.dataset.category === category;

            if (show) {

                card.classList.remove("hidden");

                setTimeout(() => {

                    card.style.display = "";

                    updateEmptyMessage();

                }, 10);

            }
            else {

                card.classList.add("hidden");

                setTimeout(() => {

                    card.style.display = "none";

                    updateEmptyMessage();

                }, 250);

            }

        });

    }

    buttons.forEach(button => {

        button.addEventListener("click", () => {

            buttons.forEach(btn =>
                btn.classList.remove("active")
            );

            button.classList.add("active");

            filterCards(button.dataset.category);

        });

    });

});