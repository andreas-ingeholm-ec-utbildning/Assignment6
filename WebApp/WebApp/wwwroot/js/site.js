function toggleMenu(elementSelector) {

    try {

        const toggleBtn = document.querySelector(elementSelector)
        toggleBtn.addEventListener('click', function () {

            const element = document.querySelector(toggleBtn.getAttribute('data-target'))
            const isOpenClass = element.classList.contains('open-menu');

            element.classList.toggle('open-menu', !isOpenClass)
            toggleBtn.classList.toggle('btn-outline-dark', !isOpenClass)
            toggleBtn.classList.toggle('btn-toggle-white', !isOpenClass)

        });

    } catch { }

}

toggleMenu('[data-option="toggle"]');

const validateText = (event) => {

    if (event.target.value.length >= 2)
        document.querySelector(`[data-valmsg-for="${event.target.id}"]`).innerHTML = "";
    else
        document.querySelector(`[data-valmsg-for="${event.target.id}"]`).innerHTML = "Must be at least 1 character long.";

};

const validateEmail = (event) => {

    const regex = /^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/;

    if (regex.test(event.target.value))
        document.querySelector(`[data-valmsg-for="${event.target.id}"]`).innerHTML = "";
    else
        document.querySelector(`[data-valmsg-for="${event.target.id}"]`).innerHTML = "Must be valid email address.";

};


const validatePassword = (event) => {

    const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;

    if (regex.test(event.target.value))
        document.querySelector(`[data-valmsg-for="${event.target.id}"]`).innerHTML = "";
    else
        document.querySelector(`[data-valmsg-for="${event.target.id}"]`).innerHTML = "Password must be at least 8 characters long, and must contain:\n\n* 1 lowercase character\n* 1 uppercase character\n* 1 number character\n* 1 special character";

};

const validatePhoneNumber = (event) => {

    const regex = /^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$/;

    if (event.target.value == "" || regex.test(event.target.value))
        document.querySelector(`[data-valmsg-for="${event.target.id}"]`).innerHTML = "";
    else
        document.querySelector(`[data-valmsg-for="${event.target.id}"]`).innerHTML = "Must be a valid phone number";

};
