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
            window.location.hash = `#${listItemFragment}`;
        })
    })
}

function SetFormDataPageFragment() {
    //Set hidden input value to current page fragment
    document.querySelector('#addReview-pageFragment').value = window.location.hash.substring(1);
}

$(document)
    .ready(SetActiveFragment());

$(document.querySelector('#add-review-form'))
    .submit(function (event) {
        SetFormDataPageFragment();
    });