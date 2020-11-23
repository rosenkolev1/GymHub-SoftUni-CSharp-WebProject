function SetActiveFragment() {
    //Set the default fragment if it is null on page reload
    let defaultFragment = "Info";

    let pageFragment = window.location.hash.substring(1);

    let myTabUl = document.querySelector("#myTab");

    let myTabUlListItems = Array.from(myTabUl.getElementsByTagName("li"));

    //Set default fragment
    if (pageFragment === "") {
        pageFragment = defaultFragment
    }

    myTabUlListItems.forEach(listItem => {
        let listItemFragment = listItem.querySelector("a").getAttribute("href").split("#").pop();
        if (listItemFragment === pageFragment) {
            listItem.classList.add("active");

            let tabDiv = myTabUl.parentElement.querySelector("#myTabContent").querySelector(`#${pageFragment}`);

            tabDiv.classList.add("in");
            tabDiv.classList.add("active");
        }

        listItem.addEventListener('click', e => {
            window.location.replace(window.location.protocol + '//' + window.location.host + window.location.pathname + window.location.search + `#${listItemFragment}`);
        })
    })
}

function SetFormDataPageFragment() {
    //Set hidden input value to current page fragment
    var hiddenFragmentInputs = document.querySelectorAll('.post-pageFragment');
    hiddenFragmentInputs.forEach(hiddenFragmentInput => {
        hiddenFragmentInput.value = window.location.hash.substring(1);
    })
}

$(document)
    .ready(SetActiveFragment());

$(document.querySelectorAll('form'))
    .submit(function (event) {
        SetFormDataPageFragment();
    });