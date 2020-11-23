function SetActiveFragment() {
    //Set the default fragment if it is null on page reload
    let defaultFragment = "Info";

    let myTabUl = document.querySelector("#myTab");

    let myTabUlListItems = Array.from(myTabUl.getElementsByTagName("li"));

    //Get all possible fragment values from the html
    let allValidFragments = myTabUlListItems.reduce((acc, listItem) => {
        let listItemFragment = listItem.querySelector("a").getAttribute("href").split("#").pop();

        acc.push(listItemFragment);

        return acc;
    }, []);

    let pageFragment = setPageFragment();

    //Set page fragment
    function setPageFragment() {
        //Set page fragment
        let pageFragment = window.location.hash.substring(1);

        //Set default fragment 
        if (allValidFragments.includes(pageFragment) === false) {
            pageFragment = defaultFragment
            window.location.replace(window.location.protocol + '//' + window.location.host + window.location.pathname + window.location.search + `#${pageFragment}`)
        }

        return pageFragment;
    }

    myTabUlListItems.forEach(listItem => {

        //Check which list items fragment is equal to the page fragment upon loading the page
        let listItemFragment = listItem.querySelector("a").getAttribute("href").split("#").pop();

        if (listItemFragment === pageFragment) {
            listItem.classList.add("active");

            let tabDiv = myTabUl.parentElement.querySelector("#myTabContent").querySelector(`#${pageFragment}`);

            tabDiv.classList.add("in");
            tabDiv.classList.add("active");
        }

        //Change page fragment onclick of one of these list items
        listItem.addEventListener('click', e => {
            window.location.replace(window.location.protocol + '//' + window.location.host + window.location.pathname + window.location.search + `#${listItemFragment}`);
        })
    })

    //If someone changes fragment by hand, then change the page appropriately
    window.addEventListener('hashchange', e => {
        myTabUlListItems.forEach(listItem => {
            let listItemFragment = listItem.querySelector("a").getAttribute("href").split("#").pop();

            pageFragment = setPageFragment();

            //Set the appropriate page fragment list item to visible and hide the rest of the list items
            if (listItemFragment === pageFragment) {
                listItem.classList.add("active");
            }
            else {
                listItem.classList.remove("active");
            }

            //Set the appropriate page fragment content to visible and hide the rest of the content
            let tabDivs = listItem.parentElement.parentElement.querySelector("#myTabContent").querySelectorAll(`.tab-pane`);
            tabDivs.forEach(tabDiv => {
                //If the tab div is with the new page fragment -> show it : else -> hide it
                if (tabDiv.id === `${pageFragment}`) {
                    tabDiv.classList.add("in");
                    tabDiv.classList.add("active");
                }
                else {
                    tabDiv.classList.remove("in");
                    tabDiv.classList.remove("active");
                }
            })
        })
    })
}

$(document)
    .ready(SetActiveFragment());

function SetFormDataPageFragment() {
    function SetFormDataPageFragmentExecute() {
        //Set hidden input value to current page fragment
        var hiddenFragmentInputs = document.querySelectorAll('.post-pageFragment');
        hiddenFragmentInputs.forEach(hiddenFragmentInput => {
            hiddenFragmentInput.value = window.location.hash.substring(1);
        })
    }

    $(document.querySelectorAll('form'))
        .submit(function (event) {
            SetFormDataPageFragmentExecute();
        });
}

SetFormDataPageFragment();


