function startUp () {
    let productEditButtons = Array.from(document.querySelectorAll(".product-comment-edit-button"));
    let productEditButtonCloseTextContent = 'Stop edit';
    let productEditButtonOpenTextContent = 'Edit';

    //Get comment text field
    let commentTextContent = document.querySelector('.product-comment-textfield');


    productEditButtons.forEach(editButton => {
        editButton.addEventListener('click', e => {
            //Show and hide edit button
            let productCommentContainer = editButton.parentElement.parentElement;
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
                    //Show the edited comment with all of it's replies
                    let childrenCommentsDiv = editContainer.parentElement.parentElement.parentElement;
                    childrenCommentsDiv.removeAttribute('hidden');
                    editContainer.removeAttribute('hidden');

                    //Hide normal comment text
                    editContainer.parentElement.querySelector('.product-comment-text').setAttribute('hidden', true);

                    //Change edit button to textContent to "Stop Edit"
                    editContainer.parentElement.parentElement.querySelector('.product-comment-edit-button').textContent = productEditButtonCloseTextContent;
                }  
            })
        })

    //Dynamically change product rating
    function DynamicProductRating() {
        let editReviewContainers = Array.from(document.querySelectorAll(".product-comment-editReview-ratings-container"));

        editReviewContainers.forEach(editReviewContainer => {
            let starsElements = Array.from(editReviewContainer.parentElement.querySelectorAll(".fa"));

            let spanRating = editReviewContainer.querySelector("#span-rating");

            let reviewRatingInput = editReviewContainer.parentElement.querySelector('#Rating');

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

startUp();