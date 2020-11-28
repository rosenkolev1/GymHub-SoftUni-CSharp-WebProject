function EditButtonsInit () {
    let productEditButtons = Array.from(document.querySelectorAll(".product-comment-edit-button"));
    let productEditButtonCloseTextContent = 'Stop edit';
    let productEditButtonOpenTextContent = 'Edit';

    //Get comment text field
    let commentTextContent = document.querySelector('.product-comment-textfield');


    productEditButtons.forEach(editButton => {
        editButton.addEventListener('click', e => {
            //Show and hide edit button
            let productCommentContainer = editButton.closest('.product-comment-container');
            let editContainer = productCommentContainer.querySelector('.product-comment-edit-container');
            let commentText = productCommentContainer.querySelector('.product-comment-text');

            if (editContainer.hasAttribute('hidden')) {
                //Edit button shows up
                commentText.setAttribute("hidden", true);
                editContainer.removeAttribute('hidden');
                editButton.textContent = productEditButtonCloseTextContent;
            }
            else {
                //Hide edit
                commentText.removeAttribute("hidden");
                editContainer.setAttribute('hidden', true);
                editButton.textContent = productEditButtonOpenTextContent;
            }
        })
    })

    //If edit comment has returned an error then have the edited comment open when the page loads
    $(document)
        .ready(function () {
            let editContainers = Array.from(document.querySelectorAll('.product-comment-edit-container'));

            editContainers.forEach(editContainer => {
                //Get all validation list items
                let validationListItems = Array.from(editContainer.querySelectorAll('.field-validation-error'));

                if (validationListItems.length > 0) {
                    //Show the edited comment with all of it's replies if it has any
                    let childrenCommentsDiv = editContainer.closest('.product-comment-child-container');
                    if (childrenCommentsDiv !== null) {
                        //Show replies
                        childrenCommentsDiv.removeAttribute('hidden');

                        //Change 'Show replies()' to 'Hide replies()'
                        let parentWithChildren = editContainer.closest('.product-commentWithChildren-container');
                        let repliesButton = parentWithChildren.querySelector('.product-comment-parent-container').querySelector('.product-comment-replies-button');
                        let repliesCount = repliesButton.textContent.split('(')[1].split(')')[0];
                        repliesButton.textContent = `Hide replies(${repliesCount})`;
                    }

                    editContainer.removeAttribute('hidden');

                    //Hide normal comment text
                    editContainer.closest('.product-comment-textfield').querySelector('.product-comment-text').setAttribute('hidden', true);

                    //Change edit button to textContent to "Stop Edit"
                    editContainer.closest('.product-comment-container').querySelector('.product-comment-edit-button').textContent = productEditButtonCloseTextContent;
                }  
            })
        })

    //Dynamically change product rating
    function DynamicProductRating() {
        let editReviewContainers = Array.from(document.querySelectorAll(".product-comment-editReview-ratings-container"));

        editReviewContainers.forEach(editReviewContainer => {
            let starsElements = Array.from(editReviewContainer.closest('.product-comment-edit-container').querySelectorAll(".fa"));

            let spanRating = editReviewContainer.querySelector("#span-rating");

            let reviewRatingInput = editReviewContainer.closest('.product-comment-edit-container').querySelector('#Rating');

            let starsContainer = editReviewContainer.querySelector(".product-comment-addReview-ratings-container");

            function matchReviewRatingInputWithSpanRating() {
                let ratingValue = reviewRatingInput.value;
                spanRating.textContent = `${ratingValue}/10`;
            }

            window.addEventListener('load', e => {
                matchReviewRatingInputWithSpanRating();
            })

            starsElements.forEach(starElement => {
                starElement.addEventListener("click", e => {
                    let starRatingValue = parseInt(starElement.id.split('-')[2]);
                    reviewRatingInput.value = starRatingValue;

                    //Change review UI
                    matchReviewRatingInputWithSpanRating();
                })

                starElement.addEventListener('mouseover', e => {
                    let starRatingValue = parseInt(starElement.id.split('-')[2]);
                    changeStarsBasedOnRating(starRatingValue);
                })
            })

            starsContainer.addEventListener('mouseleave', e => {
                let starRatingValue = reviewRatingInput.value;
                changeStarsBasedOnRating(starRatingValue);
            })

            function changeStarsBasedOnRating(starRatingValue) {
                starsElements.filter(starEl => parseInt(starEl.id.split('-')[2]) <= starRatingValue)
                    .forEach(x => {
                        x.classList.replace("fa-star-o", "fa-star");
                    })
                starsElements.filter(starEl => parseInt(starEl.id.split('-')[2]) > starRatingValue)
                    .forEach(x => {
                        x.classList.replace("fa-star", "fa-star-o");
                    })
            }
        })
    }

    DynamicProductRating();
}

//EditButtonsInit();

//let likeButtons = Array.from(document.querySelectorAll(".likeButton"));

//likeButtons.forEach(likeButton => {
//    likeButton.addEventListener('click', e => {
//        //Product container
//        let productContainer = likeButton.closest(".product-container");

//        //Product hiddent input with productId
//        let productIdInput = productContainer.querySelector(".input-productId");

//        $.ajax({
//            method: "POST",
//            data: {
//                ProductId: productIdInput.value
//            },
//            url: `Products/Like`
//        })
//    })
//})