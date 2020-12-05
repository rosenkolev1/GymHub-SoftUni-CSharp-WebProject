function RemoveProductInit(){
    let removeButton = document.querySelector(".product-remove-button");

    //Page popup
    var popup = document.querySelector(".product-remove-dimPage-container");

    //Popup close button
    let popupCloseButtons = Array.from(document.querySelectorAll(".product-close-button-popup"));
    popupCloseButtons.forEach(popupCloseButton => {
        popupCloseButton.addEventListener('click', e => {
            popup.toggleAttribute("hidden");
        })
    })

    var productRemoveForm = document.querySelector('.product-remove-form');

    //On clicking the remove button, will send post request
    let popupRemoveButton = popup.querySelector(".product-remove-button-popup");
    popupRemoveButton.addEventListener('click', sendRemovePostRequest(productRemoveForm));

    //Remove-product button handler
    removeButton.addEventListener('click', e => {
        e.preventDefault();
        e.stopPropagation();

        popup.toggleAttribute("hidden");

        return false;
    })

    function sendRemovePostRequest(productRemoveForm) {
        return (e) => {
            productRemoveForm.requestSubmit();  
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

//RemoveProductInit();