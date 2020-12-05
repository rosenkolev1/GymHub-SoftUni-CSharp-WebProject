function ProductCommentsOrderingInit(){
    let orderingSelect = document.querySelector('.product-comments-ordering-select');

    $(orderingSelect)
        .change(e => {
            let queryString = window.location.search;
            if (queryString.includes('&commentsOrderingOption=') == true) {
                let urlParams = new URLSearchParams(queryString);
                let newQueryString = Array.from(urlParams).reduce((acc, x) => {
                    let queryParam = `${x[0]}=`;
                    if (x[0] == 'commentsOrderingOption') {
                        queryParam += `${e.target.value}&`;
                    }
                    else {
                        queryParam += `${x[1]}&`;
                    }

                    return acc + queryParam;
                }, '?').slice(0, -1);

                window.location.replace(window.location.protocol + '//' + window.location.host + window.location.pathname + newQueryString + window.location.hash);
            }
            else {
                window.location.replace(window.location.protocol + '//' + window.location.host + window.location.pathname + window.location.search + `&commentsOrderingOption=${e.target.value}` + window.location.hash);
            }
        })
}

function SetUrlCommentsOrderingOption() {
    function SetUrlCommentsOrderingOptionExecute(form) {
        //Insert hidden input value with current comments ordering option value
        let pageUrlParams = new URLSearchParams(window.location.search);
        let commentsOrderingOption = pageUrlParams.get('commentsOrderingOption');
        if (commentsOrderingOption !== null) {
            let commentsOrderingOptionInput = `<input hidden name="commentsOrderingOption" value="${commentsOrderingOption}"/>`
            form.insertAdjacentHTML('afterbegin', commentsOrderingOptionInput);
        }
    }

    $(document.querySelectorAll('form'))
        .submit(function (event) {
            SetUrlCommentsOrderingOptionExecute(event.target);
        });
}