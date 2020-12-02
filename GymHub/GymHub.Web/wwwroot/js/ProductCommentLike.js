function ProductCommentLikeInit(){
    let likeElements = Array.from(document.querySelectorAll('.product-comment-like-button'));

    likeElements.forEach(likeElement => {
        likeElement.addEventListener('click', e => {

            let commentId = likeElement.closest('.product-comment-likes-container').querySelector('input').value;
            let anitForgeryToken;

            $.ajax({
                method: "POST",
                url: `/Products/LikeComment?commentId=${commentId}`,
                data: {
                    commentId: commentId,

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