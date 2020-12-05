function ProductRemoveCommentPopupInit() {

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

            //If user is admin and his comment is own, then don't show the textarea
            let commentBelongsToCurrentUser = removeButton.closest('.product-comment-container').classList.contains('product-comment-currentUser');
            if (commentBelongsToCurrentUser == true) {
                let justificationContainer = popup.querySelector('.product-comment-remove-justification-container');
                if (justificationContainer !== null) {
                    justificationContainer.setAttribute('hidden', null);
                }
            }
            else {
                let justificationContainer = popup.querySelector('.product-comment-remove-justification-container');
                justificationContainer.removeAttribute('hidden');
            }

            let justificationTextArea = popup.querySelector('.product-comment-remove-justification-textarea');
            popupRemoveButton.addEventListener("click", sendRemovePostRequest(commentRemoveForm, justificationTextArea));

            return false;
        })
    })

    function sendRemovePostRequest(commentRemoveForm, justificationTextArea) {
        return (e) => {
            if (justificationTextArea !== null && justificationTextArea.closest('.product-comment-remove-justification-container').hasAttribute('hidden') === false) {
                justificationTextArea.toggleAttribute('hidden');
                commentRemoveForm.appendChild(justificationTextArea);
            }

            //Add comment page
            SetFormDataCommentPage();

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

//ProductRemoveCommentPopupInit();