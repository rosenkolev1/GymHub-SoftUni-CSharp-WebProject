function ShowRepliesButton() {
    let replyButtons = Array.from(document.querySelectorAll(".product-comment-replies-button"));

    replyButtons.forEach(btn => {
        btn.addEventListener("click", e => {
            let parentDivElement = btn.parentElement.closest('.product-commentWithChildren-container').querySelector(".product-comment-child-container");

            parentDivElement.toggleAttribute('hidden');

            if (parentDivElement.hasAttribute("hidden")) {
                let repliesCount = btn.textContent.split('(')[1].split(')')[0];
                btn.textContent = `Show replies(${repliesCount})`;
            }
            else {
                let repliesCount = btn.textContent.split('(')[1].split(')')[0];
                btn.textContent = `Hide replies(${repliesCount})`;
            }
        })
    })

    //If replies are automatically loaded on page refresh, substitute the "Show replies(${repliesCount})" with "Hide replies(${repliesCount})"
    $(document)
        .ready(x => {
            replyButtons.forEach(btn => {
                let parentDivElement = btn.parentElement.closest('.product-commentWithChildren-container').querySelector(".product-comment-child-container");

                if (parentDivElement.hasAttribute("hidden")) {
                    let repliesCount = btn.textContent.split('(')[1].split(')')[0];
                    btn.textContent = `Show replies(${repliesCount})`;
                }
                else {
                    let repliesCount = btn.textContent.split('(')[1].split(')')[0];
                    btn.textContent = `Hide replies(${repliesCount})`;
                }
            })
        })
}

//ShowRepliesButton();