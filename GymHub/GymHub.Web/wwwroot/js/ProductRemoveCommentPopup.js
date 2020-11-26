function myFunction() {
    let removeButtons = Array.from(document.querySelectorAll(".product-comment-remove-button"));

    //Page popup
    var popup = document.querySelector(".product-comment-dimPage-container");

    //Popup close button
    let popupCloseButtons = Array.from(document.querySelectorAll(".product-comment-close-button-popup"));
    popupCloseButtons.forEach(popupCloseButton => {
        popupCloseButton.addEventListener('click', e => {
            popup.toggleAttribute("hidden");
        })
    })

    removeButtons.forEach(removeButton => {
        removeButton.addEventListener('click', e => {
            e.preventDefault();
            e.stopPropagation();

            var commentRemoveForm = removeButton.closest(".product-comment-remove-form");

            popup.toggleAttribute("hidden");

            //On clicking the remove button, will send post request
            let popupRemoveButton = popup.querySelector(".product-comment-remove-button-popup");

            popupRemoveButton.removeEventListener('click', sendRemovePostRequest);
            popupRemoveButton.addEventListener("click", sendRemovePostRequest(commentRemoveForm));

            return false;
        })
    })

    function sendRemovePostRequest(commentRemoveForm) {
        return (e) => {
            commentRemoveForm.requestSubmit();  
        }    
    }

    //If user clicks on dim background then hide the popup
    popup.addEventListener('click', e => {

        //If clicked on child, stop the toggle of the popup
        if (e.target !== event.currentTarget) {
            return;
        }

        popup.toggleAttribute("hidden");
    })
}

myFunction();