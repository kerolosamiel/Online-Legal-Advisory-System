const dropDownEle = document.querySelector(".dropdown");

dropDownEle.addEventListener("click", () => {
    toggleClass(dropDownEle, "active")
});

document.addEventListener("click", (e) => {
    if (!dropDownEle.contains(e.target)) {
        dropDownEle.classList.remove("active");
    }
});

function toggleClass(element, className) {
    element.classList.toggle(className);
}