function SearchUsersInit() {
    let searchButton = document.querySelector('.search-button');
    let searchText = document.querySelector('.users-search');

    searchButton.addEventListener('click', e => {
        if (searchText.value) {
            let locationParams = new URLSearchParams(window.location.search);

            let newQueryString = "?";
            let containsSeachParam = false;

            Array.from(locationParams.entries()).forEach(locationParam => {
                if (locationParam[0] == "userSearch") {
                    newQueryString += `${locationParam[0]}=${searchText.value}&`;
                    containsSeachParam = true;
                }
                else newQueryString += `${locationParam[0]}=${locationParam[1]}&`;
            })

            if (containsSeachParam == false) newQueryString += `userSearch=${searchText.value}`;

            window.location.search = newQueryString;
        }
    })
}

SearchUsersInit();