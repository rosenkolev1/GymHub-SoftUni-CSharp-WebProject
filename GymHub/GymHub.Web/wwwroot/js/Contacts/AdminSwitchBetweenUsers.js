function AdminSwitchBetweenUsersInit(){
    let switchesBetweenUsers = Array.from(document.querySelectorAll('.switch-between-users'));

    switchesBetweenUsers.forEach(switchBetweenUsers => {
        switchBetweenUsers.addEventListener('click', e => {
            let switchUserId = switchBetweenUsers.querySelector('.switch-user-id').value;

            window.location.search = `?targetUserId=${switchUserId}`;
        })
    })
}

AdminSwitchBetweenUsersInit();