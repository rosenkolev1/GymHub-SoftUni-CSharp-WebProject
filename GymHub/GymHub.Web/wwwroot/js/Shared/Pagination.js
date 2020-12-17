function PaginationInit(pageQueryParamName){
    let pageItems = Array.from(document.querySelectorAll('.page-element'));
    let pagePrevious = document.querySelector('.page-element-previous');
    let pageNext = document.querySelector('.page-element-next');
    let pageDots = Array.from(document.querySelectorAll('.page-element-dots'));

    let numberOfPages = parseInt(pageItems.slice().pop().textContent);

    //Get the query string value
    let pageQueryValue = new URLSearchParams(window.location.search).get(`${pageQueryParamName}`);
    if (pageQueryValue === null) pageQueryValue = 1;
    else {
        pageQueryValue = parseInt(pageQueryValue);
    }

    //Disable next and previous buttons if they go outside the pages' boundaries
    setInterval(() => {
        //Change previous button appearance
        if (pageQueryValue <= 1) pagePrevious.classList.add('disabled');

        //Change next button appearance
        if (pageQueryValue >= numberOfPages) pageNext.classList.add('disabled');

    }, 100);

    // Set active page item on page load
    $(document)
        .ready(() => {
            pageItems.forEach(pageItem => {
                if (parseInt(pageItem.textContent) == pageQueryValue) pageItem.classList.add('active');
            })
        })

    //Set event handler for concrete page number item
    pageItems.forEach(pageItem => {
        let pageItemNumber = parseInt(pageItem.textContent);

        //Change normal page button appearance
        if (pageItemNumber === pageQueryValue) {
            pageItem.classList.add('active');
        }

        pageItem.addEventListener('click', e => {
            FillQueryStringWithPage(pageItemNumber, 0, false);
        })
    })

    //Set event handler for previous button
    pagePrevious.addEventListener('click', e => {
        if (pageQueryValue > 1) {
            FillQueryStringWithPage(pageQueryValue, -1, true);
        }
    });

    //Set event handler for next button
    pageNext.addEventListener('click', e => {
        if (pageQueryValue < numberOfPages) {
            FillQueryStringWithPage(pageQueryValue, 1, true);
        }
    });

    //Set event handle for dots button
    pageDots.forEach(pageDot => {
        pageDot.addEventListener('click', e => {
            let pageItemEl;
            let pageItemNumber;

            if (pageDot.classList.contains('pageNumber=fromNext')) {
                pageItemEl = pageDot.nextElementSibling;
                pageItemNumber = parseInt(pageItemEl.textContent) - 1;
            }
            else if (pageDot.classList.contains('pageNumber=fromPrevious')) {
                pageItemEl = pageDot.previousElementSibling;
                pageItemNumber = parseInt(pageItemEl.textContent) + 1;
            }

            FillQueryStringWithPage(pageItemNumber, 0, false);
        })
    })

    function FillQueryStringWithPage(intialValue, increment, willIncrement) {
        let pageUrlParams = new URLSearchParams(window.location.search);

        let newPageUrlParams = [];

        let pageNumber = intialValue;

        if (pageUrlParams.has(`${pageQueryParamName}`) === true) {

            Array.from(pageUrlParams.entries()).forEach(kv => {
                let paramName = kv[0];
                let paramValue = kv[1];

                if (paramName == `${pageQueryParamName}`) {
                    if (willIncrement == true) {
                        pageNumber = parseInt(paramValue) + increment;
                    }

                    if (pageNumber <= 0) {
                        pageNumber = 1;
                    }
                    else if (pageNumber > numberOfPages) {
                        pageNumber = numberOfPages;
                    }

                    newPageUrlParams.push(`${paramName}=${pageNumber}`);
                    return;
                }

                newPageUrlParams.push(`${paramName}=${paramValue}`);
            })

            window.location.search = `?${newPageUrlParams.join("&")}`;
        }
        else {
            if (pageNumber <= 0) pageNumber = 1;
            if (willIncrement === true) pageNumber++;
            window.location.search = window.location.search + `&${pageQueryParamName}=${pageNumber}`;
        }
    }

    SetFormDataPage(pageQueryParamName);
}

function SetFormDataPage(pageQueryParamName) {
    function SetFormDataPageExecute(form) {
        //Insert hidden input value with current page value
        let pageUrlParams = new URLSearchParams(window.location.search);
        let pageQueryValue = pageUrlParams.get(`${pageQueryParamName}`);
        if (pageQueryValue !== null) {
            let pageQueryValueInput = `<input hidden name="${pageQueryParamName}" value="${pageQueryValue}"/>`
            form.insertAdjacentHTML('afterbegin', pageQueryValueInput);
        }
    }

    $(document.querySelectorAll('form'))
        .submit(function (event) {
            SetFormDataPageExecute(event.target);
        });
}

//PaginationInit('commentsPage');