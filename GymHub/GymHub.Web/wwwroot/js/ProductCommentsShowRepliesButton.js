let replyButtons = Array.from(document.querySelectorAll(".product-comment-replies-button"));

replyButtons.forEach(btn => {
    btn.addEventListener("click", e => {
        let parentDivElement = btn.parentElement.parentElement.parentElement.querySelector(".product-comment-child-container");
        if (parentDivElement.hasAttribute("hidden")) {
            parentDivElement.removeAttribute("hidden");
            let repliesCount = btn.textContent.split('(')[1].split(')')[0];
            btn.textContent = `Hide replies(${repliesCount})`;
        }
        else {
            parentDivElement.setAttribute("hidden", "true");
            let repliesCount = btn.textContent.split('(')[1].split(')')[0];
            btn.textContent = `Show replies(${repliesCount})`;
        }
    })
})