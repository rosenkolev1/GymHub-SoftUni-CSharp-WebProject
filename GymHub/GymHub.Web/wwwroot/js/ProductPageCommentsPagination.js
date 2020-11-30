function ProductPageCommentsPaginationInit(){
    let commentsPageItems = Array.from(document.querySelectorAll('.product-comment-page-item'));
    let commentPagePrevious = document.querySelector('.product-comment-page-previous');
    let commentPageNext = document.querySelector('.product-comment-page-next');
    let commentPageDots = Array.from(document.querySelectorAll('.product-comment-page-dots'));

    let numberOfCommentsPages = parseInt(commentsPageItems.slice().pop().textContent);

    //Get the query string value
    let commentsPagesQueryValue = new URLSearchParams(window.location.search).get('commentsPage');
    if (commentsPagesQueryValue === null) commentsPagesQueryValue = 1;
    else {
        commentsPagesQueryValue = parseInt(commentsPagesQueryValue);
    }

    //Disable next and previous buttons if they go outside the pages' boundaries
    setInterval(() => {
        //Change previous button appearance
        if (commentsPagesQueryValue <= 1) commentPagePrevious.classList.add('disabled');

        //Change next button appearance
        if (commentsPagesQueryValue >= numberOfCommentsPages) commentPageNext.classList.add('disabled');

    }, 100);

    // Set active page item on page load
    $(document)
        .ready(() => {
            commentsPageItems.forEach(pageItem => {
                if (parseInt(pageItem.textContent) == commentsPagesQueryValue) pageItem.classList.add('active');
            })
        })

    //Set event handler for concrete page number item
    commentsPageItems.forEach(pageItem => {
        let pageItemNumber = parseInt(pageItem.textContent);

        //Change normal page button appearance
        if (pageItemNumber === commentsPagesQueryValue) {
            pageItem.classList.add('active');
        }

        pageItem.addEventListener('click', e => {
            FillQueryStringWithCommentsPage(pageItemNumber, 0, false);
        })
    })

    //Set event handler for previous button
    commentPagePrevious.addEventListener('click', e => {
        if (commentsPagesQueryValue > 1) {
            FillQueryStringWithCommentsPage(commentsPagesQueryValue, -1, true);
        }
    });

    //Set event handler for next button
    commentPageNext.addEventListener('click', e => {
        if (commentsPagesQueryValue < numberOfCommentsPages) {
            FillQueryStringWithCommentsPage(commentsPagesQueryValue, 1, true);
        }
    });

    //Set event handle for dots button
    commentPageDots.forEach(commentPageDot => {
        commentPageDot.addEventListener('click', e => {
            let pageItemEl;
            let pageItemNumber;

            if (commentPageDot.classList.contains('pageNumber=fromNext')) {
                pageItemEl = commentPageDot.nextElementSibling;
                pageItemNumber = parseInt(pageItemEl.textContent) - 1;
            }
            else if (commentPageDot.classList.contains('pageNumber=fromPrevious')) {
                pageItemEl = commentPageDot.previousElementSibling;
                pageItemNumber = parseInt(pageItemEl.textContent) + 1;
            }

            FillQueryStringWithCommentsPage(pageItemNumber, 0, false);
        })
    })

    function FillQueryStringWithCommentsPage(intialValue, increment, willIncrement) {
        let pageUrlParams = new URLSearchParams(window.location.search);

        let newPageUrlParams = [];

        let commentPageNumber = intialValue;

        if (pageUrlParams.has('commentsPage') === true) {

            Array.from(pageUrlParams.entries()).forEach(kv => {
                let paramName = kv[0];
                let paramValue = kv[1];

                if (paramName == 'commentsPage') {
                    if (willIncrement == true) {
                        commentPageNumber = parseInt(paramValue) + increment;
                    }

                    if (commentPageNumber <= 0) {
                        commentPageNumber = 1;
                    }
                    else if (commentPageNumber > numberOfCommentsPages) {
                        commentPageNumber = numberOfCommentsPages;
                    }

                    newPageUrlParams.push(`${paramName}=${commentPageNumber}`);
                    return;
                }

                newPageUrlParams.push(`${paramName}=${paramValue}`);
            })

            window.location.search = `?${newPageUrlParams.join("&")}`;
        }
        else {
            if (commentPageNumber <= 0) commentPageNumber = 1;
            if (willIncrement === true) commentPageNumber++;
            window.location.search = window.location.search + `&commentsPage=${commentPageNumber}`;
        }
    }

    SetFormDataCommentPage();
}

function SetFormDataCommentPage() {
    function SetFormDataCommentsPageExecute(form) {
        //Insert hidden input value with current comments page value
        let pageUrlParams = new URLSearchParams(window.location.search);
        let commentsPage = pageUrlParams.get('commentsPage');
        if (commentsPage !== null) {
            let commentsPageInput = `<input hidden name="commentsPage" value="${commentsPage}"/>`
            form.insertAdjacentHTML('afterbegin', commentsPageInput);
        }
    }

    $(document.querySelectorAll('form'))
        .submit(function (event) {
            SetFormDataCommentsPageExecute(event.target);
        });
}

//ProductPageCommentsPaginationInit();