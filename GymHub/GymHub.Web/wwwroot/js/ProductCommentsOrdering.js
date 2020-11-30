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