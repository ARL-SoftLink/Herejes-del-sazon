document.addEventListener("DOMContentLoaded", () => {

    const buttons = document.querySelectorAll(".select-table-btn");
    const selectedId = document.getElementById("selectedTableId");
    const selectedName = document.getElementById("selectedTableName");

    buttons.forEach(button => {

        button.addEventListener("click", () => {

            document.querySelectorAll(".table-card")
                .forEach(card => card.classList.remove("selected"));

            const card = button.closest(".table-card");
            card.classList.add("selected");

            selectedId.value = button.dataset.id;
            selectedName.textContent = button.dataset.name;

            document
            .querySelector(".reservation-form")
             .scrollIntoView({

                   behavior:"smooth",

                      block:"start"

            });

        });

    });


    const form = document.getElementById("reservationForm");

    form.addEventListener("submit", function(e){

        if(selectedId.value === ""){

         e.preventDefault();

          alert("Seleccione una mesa antes de continuar.");

        }

    });

});