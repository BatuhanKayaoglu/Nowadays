document.getElementById('companiesButton').addEventListener('click', function (event) {
    event.preventDefault();
    var submenu = document.getElementById('companiesSubmenu');
    if (submenu.style.display === 'none' || submenu.style.display === '') {
        submenu.style.display = 'block';
    } else {
        submenu.style.display = 'none';
    }
});
