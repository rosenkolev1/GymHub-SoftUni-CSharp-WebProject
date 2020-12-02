function ProductCommentLikeInit(){
    let likeElements = Array.from(document.querySelectorAll('.product-comment-like-button'));

    likeElements.forEach(likeElement => {
        likeElement.addEventListener('click', e => {

            let commentId = likeElement.closest('.product-comment-likes-container').querySelector('.product-comment-like-commentId').value;
            let anitForgeryToken = Array.from(likeElement.closest('.product-comment-likes-container').querySelectorAll('input'))
                .find(x => x.name == '__RequestVerificationToken').value;

            $.ajax({
                method: "POST",
                url: `/Products/LikeComment?commentId=${commentId}`,
                data: {
                    commentId: commentId,
                    __RequestVerificationToken: anitForgeryToken
                },
                success: (response) => {
                    let likesSpan = likeElement.closest('.product-comment-likes-container').querySelector('span');
                    likesSpan.textContent = response.toString();

                    likeElement.classList.toggle('far');
                    likeElement.classList.toggle('fas');
                }
            })

        })
    })
}

//ProductCommentLikeInit();