function startUp() {
    let productReplyButtons = Array.from(document.querySelectorAll(".product-comment-reply-button"));
    let productReplyButtonCloseTextContent = 'Stop reply';
    let productReplyButtonOpenTextContent = 'Reply';


    productReplyButtons.forEach(replyButton => {
        replyButton.addEventListener('click', e => {
            //Whole comment container
            let commentTextfield = replyButton.parentElement.querySelector('.product-comment-textfield');

            //Show and hide edit button
            let replyContainer = commentTextfield.querySelector('.product-comment-reply-container');
            let commentText = commentTextfield.querySelector('.product-comment-text');

            //Hide or show reply editor if it has been loaded
            if (replyContainer !== null) {
                if (replyContainer.hasAttribute('hidden')) {
                    //Show reply editor
                    replyContainer.removeAttribute('hidden');
                    replyButton.textContent = productReplyButtonCloseTextContent;
                }
                else {
                    //Hide reply editor
                    replyContainer.setAttribute('hidden', true);
                    replyButton.textContent = productReplyButtonOpenTextContent;
                }
            }
            //Load reply editor if it hasn't been loaded
            else {
                let replyProductId = commentTextfield.querySelector('#ReplyProductId').textContent;
                let replyCommentId = commentTextfield.querySelector('#ReplyCommentId').textContent;
                let replyCommentCounter = parseInt(commentTextfield.querySelector('#ReplyCommentCounter').textContent);
                let requestData = {
                    ProductId: replyProductId,
                    ParentCommentId: replyCommentId,
                    CommentCounter: replyCommentCounter
                };

                $.ajax({
                    url: '/Products/LoadReplyToComment',
                    data: requestData,
                    method: 'GET',
                    success: function (result) {
                        commentTextfield.insertAdjacentHTML('beforeend', result);

                        //Add url page fragment to form data on submit for the reply form
                        SetFormDataPageFragment();

                        //Change 'Reply' button to 'Stop reply'
                        replyButton.textContent = productReplyButtonCloseTextContent;
                    },
                    error: function (response) {
                        document.write(response.responseText);
                    }
                });

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