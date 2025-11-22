// map-loader.js - Asynchronous Yandex map loader
document.addEventListener('DOMContentLoaded', function() {
    // Load Yandex map asynchronously for all pages with map containers
    const mapContainers = document.querySelectorAll('#yandex-map');

    if (mapContainers.length > 0) {
        // Create script element for Yandex Maps API
        const script = document.createElement('script');
        script.src = 'https://api-maps.yandex.ru/2.1/?apikey=YOUR_API_KEY&lang=ru_RU';
        script.async = true;
        script.defer = true;

        // When script loads, initialize maps
        script.onload = function() {
            ymaps.ready(function() {
                mapContainers.forEach(function(container) {
                    // Create map instance
                    const map = new ymaps.Map(container, {
                        center: [55.030204, 82.920430], // Coordinates for Novosibirsk, ul. Lenina, 12
                        zoom: 17,
                        controls: ['zoomControl', 'fullscreenControl']
                    });

                    // Add placemark
                    const placemark = new ymaps.Placemark([55.030204, 82.920430], {
                        hintContent: 'Дом Кофе',
                        balloonContent: 'г. Новосибирск, ул. Ленина, 12<br>Телефон: +7 (495) 123-45-67'
                    });

                    map.geoObjects.add(placemark);
                });
            });
        };

        // Handle script loading errors
        script.onerror = function() {
            console.warn('Failed to load Yandex Maps API');
            mapContainers.forEach(function(container) {
                container.innerHTML = '<p style="text-align: center; padding: 20px; color: #666;">Карта временно недоступна</p>';
            });
        };

        // Append script to document head
        document.head.appendChild(script);
    }
});
