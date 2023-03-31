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

function updateFooterPosition(scrollHeight, innerHeight) {

    try {

        const footer = document.querySelector("footer");
        const isBodyLargerThanScreen = scrollHeight >= innerHeight + footer.scrollHeight;

        footer.classList.toggle("position-fixed-bottom", !isBodyLargerThanScreen);
        footer.classList.toggle("position-static", isBodyLargerThanScreen);

    } catch { }

}

toggleMenu('[data-option="toggle"]');
updateFooterPosition(document.body.scrollHeight, win.innerHeight);
