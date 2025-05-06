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

// First ensure jQuery validation is loaded
function initValidation() {
    if (typeof jQuery === 'undefined' || typeof $.validator === 'undefined') {
        setTimeout(initValidation, 100);
        return;
    }

    // Disable default email validation
    $.validator.methods.email = function () {
        return true;
    };

    // Custom validation method
    $.validator.addMethod("loginformat", function (value, element) {
        if (this.optional(element)) return true;

        // Email pattern
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        // Username pattern (3-20 chars, letters/numbers/underscores/dots)
        const usernameRegex = /^[a-zA-Z0-9_\.\-]{3,20}$/;

        return emailRegex.test(value) || usernameRegex.test(value);
    });

    // Apply to our field with retry logic
    function applyValidation() {
        var $input = $("input[name='Input.LoginIdentifier']");
        if ($input.length) {
            $input.rules("add", {
                loginformat: true,
                messages: {
                    loginformat: "Please enter a valid username or email"
                }
            });
        } else {
            setTimeout(applyValidation, 100);
        }
    }

    // Initialize validation
    var $form = $("form#account");
    $form.removeData("validator");
    $form.removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse($form);

    // Apply custom validation
    setTimeout(applyValidation, 200);
}

// Start initialization when DOM is ready
document.addEventListener('DOMContentLoaded', function () {
    initValidation();
});

// Firefox-specific fix for event listeners
if (navigator.userAgent.toLowerCase().indexOf('firefox') > -1) {
    $(window).on('load', function () {
        initValidation();
    });
}