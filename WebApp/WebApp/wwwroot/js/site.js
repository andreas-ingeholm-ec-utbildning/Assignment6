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
