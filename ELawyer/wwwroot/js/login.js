// Toggle active style on "Remember Me" label when checkbox state changes

// Select the "Remember Me" label and its corresponding checkbox input
const rememberLabel = document.querySelector(".login-manually .remember-forget label");
const rememberCheckbox = document.querySelector(".login-manually .remember-forget label .checkbox-input");

// Add an event listener to the checkbox to handle state changes
rememberCheckbox.addEventListener("change", () => {
    toggleClassName(rememberLabel, "active");
})

// Toggles a class name on the given HTML element.
function toggleClassName(element, className) {
    element.classList.toggle(className)
}