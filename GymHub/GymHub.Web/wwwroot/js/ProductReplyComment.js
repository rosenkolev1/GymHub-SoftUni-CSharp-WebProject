function startUp() {
    let productReplyButtons = Array.from(document.querySelectorAll(".product-comment-reply-button"));
    let productReplyButtonCloseTextContent = 'Stop reply';
    let productReplyButtonOpenTextContent = 'Reply';

    //Get comment text field
    let commentTextContent = document.querySelector('.product-comment-textfield');


    productReplyButtons.forEach(replyButton => {
        replyButton.addEventListener('click', e => {
            //Show and hide edit button
            let replyContainer = replyButton.parentElement.querySelector('.product-comment-reply-container');
            let commentText = replyButton.parentElement.querySelector('.product-comment-text');

            if (replyContainer.hasAttribute('hidden')) {
                //Edit button shows up
                //commentText.setAttribute("hidden", true);
                replyContainer.removeAttribute('hidden');
                replyButton.textContent = productReplyButtonCloseTextContent;
            }
            else {
                //Hide edit
                //commentText.removeAttribute("hidden");
                replyContainer.setAttribute('hidden', true);
                replyButton.textContent = productReplyButtonOpenTextContent;
            }
        })
    })

    //If reply comment has returned an error then have the reply comment open when the page loads
    $(document)
        .ready(function () {
            let editContainers = Array.from(document.querySelectorAll('.product-comment-reply-container'));

            editContainers.forEach(editContainer => {
                //Get all validation list items
                let validationListItems = Array.from(editContainer.querySelectorAll('.field-validation-error'));

                if (validationListItems.length > 0) {
                    //Show the edited comment with all of it's replies
                    let childrenCommentsDiv = editContainer.parentElement.parentElement.parentElement;
                    childrenCommentsDiv.removeAttribute('hidden');
                    editContainer.removeAttribute('hidden');

                    //Change reply button to textContent to "Stop reply"
                    editContainer.parentElement.parentElement.querySelector('.product-comment-reply-button').textContent = productReplyButtonCloseTextContent;
                }  
            })
        })
}

startUp();