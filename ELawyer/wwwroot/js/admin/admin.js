const sidebarToggle = document.querySelector('.hamburger');
const adminTop = document.querySelector(".admin-topbar");
const sidebar = document.querySelector('.admin-sidebar');
const mainContent = document.querySelector('.admin-main');

if (sidebarToggle && sidebar) {
    sidebarToggle.addEventListener('click', function () {
        toggleClass(sidebar, "active");
        toggleClass(mainContent, "active");
        toggleClass(adminTop, "active");
        toggleClass(sidebarToggle, "active");
    });
}

function toggleClass(element, className) {
    element.classList.toggle(className);
}