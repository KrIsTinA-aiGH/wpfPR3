// js/main.js
document.addEventListener('DOMContentLoaded', function() {
    console.log('Main script loaded');

    // Сразу обновляем UI из localStorage
    updateAuthUIFromStorage();

    // Затем подписываемся на изменения аутентификации
    if (window.auth) {
        window.auth.onAuthStateChanged((user) => {
            console.log('Auth state changed in main:', user);
            updateAuthUI(user);
        });
    }

    // Функция для плавной прокрутки к якорю
    function smoothScroll(target) {
        const element = document.querySelector(target);
        if (element) {
            element.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            });
        }
    }

    // Обработчик кликов по ссылкам навигации
    const navLinks = document.querySelectorAll('nav a[href^="#"]');

    navLinks.forEach(link => {
        link.addEventListener('click', function(e) {
            e.preventDefault();
            const target = this.getAttribute('href');
            smoothScroll(target);

            // Закрыть мобильное меню после клика на ссылку
            const nav = document.querySelector('nav');
            const hamburger = document.querySelector('.hamburger');
            if (nav.classList.contains('nav-open')) {
                nav.classList.remove('nav-open');
                hamburger.classList.remove('active');
            }
        });
    });

    // Гамбургер меню
    const hamburger = document.querySelector('.hamburger');
    const nav = document.querySelector('nav');

    if (hamburger && nav) {
        hamburger.addEventListener('click', function() {
            nav.classList.toggle('nav-open');
            hamburger.classList.toggle('active');
        });
    }
});
