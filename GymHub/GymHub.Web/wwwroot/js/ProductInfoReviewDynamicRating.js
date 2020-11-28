function DynamicRating() {
    let reviewForm = document.querySelector(".product-review-form");

    let starsElements = Array.from(reviewForm.closest('.product-comment-addReview-container').querySelectorAll(".fa"));

    let spanRating = reviewForm.closest('.product-comment-addReview-container').querySelector("#span-rating");

    let reviewRatingInput = reviewForm.querySelector('#InputModel_Rating');

    let starsContainer = reviewForm.closest('.product-comment-addReview-container').querySelector(".product-comment-addReview-ratings-container");

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
}

DynamicRating();