// cart.js - управление корзиной
class CartManager {
    constructor() {
        this.currentUser = null;
        this.cart = [];
        this.isSyncing = false;
        
        // Проверяем доступность Firebase
        this.checkFirebaseAvailability();
        this.setupAuthListener();
    }

    // Проверка доступности Firebase
    checkFirebaseAvailability() {
        if (!window.firebase) {
            console.error('Firebase not loaded');
            return;
        }
        
        if (!window.database) {
            console.error('Firebase Database not available - check if firebase-database.js is loaded');
        } else {
            console.log('Firebase Database is available');
        }
    }

    // Получить ключ для корзины текущего пользователя
    getCartKey() {
        if (this.currentUser) {
            return `coffeeCart_${this.currentUser.uid}`;
        } else {
            return 'coffeeCart_guest';
        }
    }

    // Получить корзину из localStorage
    getCartFromStorage() {
        const cartKey = this.getCartKey();
        const cart = localStorage.getItem(cartKey);
        return cart ? JSON.parse(cart) : [];
    }

    // Сохранить корзину в localStorage
    saveCartToStorage() {
        const cartKey = this.getCartKey();
        localStorage.setItem(cartKey, JSON.stringify(this.cart));
    }

    // Добавить товар в корзину
    async addToCart(productId, product) {
        const existingItem = this.cart.find(item => item.id === productId);

        if (existingItem) {
            existingItem.quantity += 1;
        } else {
            this.cart.push({
                id: productId,
                name: product.name,
                price: product.price,
                image: product.image,
                quantity: 1,
                weight: product.weight || 250
            });
        }

        this.saveCartToStorage();
        if (this.currentUser) {
            await this.saveCartToFirebase();
        }
        this.updateCartUI();
        this.showCartNotification(product.name);
    }

    // Удалить товар из корзины
    async removeFromCart(productId) {
        console.log('Удаляем товар:', productId);
        this.cart = this.cart.filter(item => item.id !== productId);
        this.saveCartToStorage();
        if (this.currentUser) {
            await this.saveCartToFirebase();
        }
        this.updateCartUI();
    }

    // Изменить количество товара
    async updateQuantity(productId, newQuantity) {
        if (newQuantity <= 0) {
            await this.removeFromCart(productId);
            return;
        }

        const item = this.cart.find(item => item.id === productId);
        if (item) {
            item.quantity = newQuantity;
            this.saveCartToStorage();
            if (this.currentUser) {
                await this.saveCartToFirebase();
            }
            this.updateCartUI();
        }
    }

    // Очистить корзину
    async clearCart() {
        this.cart = [];
        this.saveCartToStorage();
        if (this.currentUser) {
            await this.saveCartToFirebase();
        }
        this.updateCartUI();
    }

    // Показать уведомление о добавлении в корзину
    showCartNotification(productName) {
        const notification = document.createElement('div');
        notification.className = 'cart-notification';
        notification.innerHTML = `
            <i class="fas fa-check-circle"></i>
            <span>${productName} добавлен в корзину</span>
        `;
        notification.style.cssText = `
            position: fixed;
            top: 20px;
            right: 20px;
            background: linear-gradient(135deg, #4CAF50, #45a049);
            color: white;
            padding: 15px 20px;
            border-radius: 5px;
            z-index: 10000;
            font-weight: bold;
            box-shadow: 0 4px 6px rgba(0,0,0,0.1);
        `;

        document.body.appendChild(notification);

        setTimeout(() => {
            if (notification.parentNode) {
                notification.parentNode.removeChild(notification);
            }
        }, 3000);
    }

    // Обновить UI корзины
    updateCartUI() {
        this.updateCartCount();
        this.updateCartPage();
    }

    // Обновить счетчик товаров в header
    updateCartCount() {
        const cartCount = document.querySelector('.cart-count');
        const cartLink = document.getElementById('cart-link');
        const totalItems = this.cart.reduce((sum, item) => sum + item.quantity, 0);

        if (cartCount) {
            cartCount.textContent = totalItems;
        }

        if (cartLink) {
            if (totalItems > 0) {
                cartLink.style.display = 'block';
            } else {
                cartLink.style.display = 'none';
            }
        }
    }

    // Обновить страницу корзины
    updateCartPage() {
        const cartItems = document.getElementById('cart-items');
        const emptyCart = document.getElementById('empty-cart');
        const cartTotalPrice = document.getElementById('cart-total-price');
        const checkoutBtn = document.getElementById('checkout-btn');

        if (!cartItems) return;

        // ВСЕГДА вычисляем общую сумму
        const totalPrice = this.cart.reduce((sum, item) => sum + (item.price * item.quantity), 0);

        if (this.cart.length === 0) {
            if (emptyCart) emptyCart.style.display = 'block';
            if (cartItems) cartItems.style.display = 'none';
            if (checkoutBtn) checkoutBtn.disabled = true;
            // ОБНОВЛЯЕМ ОБЩУЮ СУММУ НА 0 ПРИ ПУСТОЙ КОРЗИНЕ
            if (cartTotalPrice) {
                cartTotalPrice.textContent = '0';
            }
            return;
        }

        if (emptyCart) emptyCart.style.display = 'none';
        if (cartItems) cartItems.style.display = 'block';

        // Очищаем и заполняем корзину
        cartItems.innerHTML = '';

        this.cart.forEach(item => {
            const itemTotal = item.price * item.quantity;

            const cartItem = document.createElement('div');
            cartItem.className = 'cart-item';
            cartItem.innerHTML = `
                <div class="cart-item-image">
                    <img src="${item.image}" alt="${item.name}">
                </div>
                <div class="cart-item-info">
                    <h4>${item.name}</h4>
                    <p>${item.price} ₽ / ${item.weight} г</p>
                </div>
                <div class="cart-item-controls">
                    <button class="quantity-btn minus" data-id="${item.id}">-</button>
                    <span class="quantity">${item.quantity}</span>
                    <button class="quantity-btn plus" data-id="${item.id}">+</button>
                    <button class="remove-btn" data-id="${item.id}">
                        <i class="fas fa-trash"></i>
                    </button>
                </div>
                <div class="cart-item-total">
                    ${itemTotal} ₽
                </div>
            `;

            cartItems.appendChild(cartItem);
        });

        // ОБНОВЛЯЕМ ОБЩУЮ СУММУ - ГАРАНТИРОВАННО
        if (cartTotalPrice) {
            cartTotalPrice.textContent = totalPrice;
            console.log('Общая сумма обновлена:', totalPrice, '₽');
        }

        if (checkoutBtn) {
            checkoutBtn.disabled = false;
        }

        // Добавляем обработчики событий
        this.addCartEventListeners();
    }

    // Добавить обработчики событий для корзины
    addCartEventListeners() {
        // Кнопки увеличения количества
        document.querySelectorAll('.quantity-btn.plus').forEach(btn => {
            btn.addEventListener('click', async (e) => {
                const productId = e.target.closest('.quantity-btn').dataset.id;
                const item = this.cart.find(item => item.id === productId);
                if (item) {
                    await this.updateQuantity(productId, item.quantity + 1);
                }
            });
        });

        // Кнопки уменьшения количества
        document.querySelectorAll('.quantity-btn.minus').forEach(btn => {
            btn.addEventListener('click', async (e) => {
                const productId = e.target.closest('.quantity-btn').dataset.id;
                const item = this.cart.find(item => item.id === productId);
                if (item) {
                    await this.updateQuantity(productId, item.quantity - 1);
                }
            });
        });

        // Кнопки удаления
        document.querySelectorAll('.remove-btn').forEach(btn => {
            btn.addEventListener('click', async (e) => {
                const productId = e.target.closest('.remove-btn').dataset.id;
                await this.removeFromCart(productId);
            });
        });

        // Кнопка оформления заказа
        const checkoutBtn = document.getElementById('checkout-btn');
        if (checkoutBtn) {
            checkoutBtn.addEventListener('click', () => {
                this.checkout();
            });
        }
    }

    // Оформление заказа
    async checkout() {
        if (!window.auth || !window.auth.currentUser) {
            alert('Пожалуйста, войдите в систему для оформления заказа');
            window.location.href = 'login.html';
            return;
        }

        // Проверяем, доступна ли база данных
        if (!window.database) {
            alert('Ошибка подключения к базе данных');
            return;
        }

        const user = window.auth.currentUser;
        const order = {
            userId: user.uid,
            items: this.cart,
            total: this.cart.reduce((sum, item) => sum + (item.price * item.quantity), 0),
            status: 'pending',
            createdAt: new Date().toISOString(),
            userEmail: user.email,
            userName: user.displayName || user.email
        };

        try {
            // Сохраняем заказ в базу данных
            const db = window.database;
            const ordersRef = db.ref('orders');
            await ordersRef.push(order);
            
            // Очищаем корзину после успешного оформления
            await this.clearCart();
            
            alert('Заказ успешно оформлен!');
            
            // Переключаем на вкладку заказов
            this.switchTab('orders');
        } catch (error) {
            console.error('Ошибка при оформлении заказа:', error);
            alert('Ошибка при оформлении заказа: ' + error.message);
        }
    }

    // Переключение вкладок
    switchTab(tabName) {
        document.querySelectorAll('.tab-btn').forEach(btn => {
            btn.classList.remove('active');
        });
        document.querySelectorAll('.tab-content').forEach(content => {
            content.classList.remove('active');
        });

        const tabBtn = document.querySelector(`[data-tab="${tabName}"]`);
        const tabContent = document.getElementById(tabName);
        
        if (tabBtn) tabBtn.classList.add('active');
        if (tabContent) tabContent.classList.add('active');
    }

    // Настройка слушателя аутентификации
    setupAuthListener() {
        if (window.auth) {
            window.auth.onAuthStateChanged((user) => {
                console.log('Auth state changed in CartManager:', user);
                if (user) {
                    this.onUserLogin(user);
                } else {
                    this.onUserLogout();
                }
            });
        } else {
            console.error('Firebase auth not available');
            // Если auth не доступен, загружаем гостевую корзину
            this.cart = this.getCartFromStorage();
            this.updateCartUI();
        }
    }

    // Обработчик входа пользователя
    async onUserLogin(user) {
        console.log('User logged in, switching cart');
        
        // Сохраняем текущую гостевую корзину
        const guestCart = [...this.cart];
        const guestCartKey = 'coffeeCart_guest';
        
        // Сохраняем гостевую корзину под отдельным ключом
        if (guestCart.length > 0) {
            localStorage.setItem(guestCartKey, JSON.stringify(guestCart));
        }

        // Переключаемся на пользователя
        this.currentUser = user;
        
        // Загружаем корзину пользователя из Firebase
        await this.loadCartFromFirebase(user.uid);
        
        // Обновляем UI
        this.updateCartUI();
    }

    // Обработчик выхода пользователя
    async onUserLogout() {
        console.log('User logged out, switching to guest cart');
        
        // Сохраняем пользовательскую корзину перед выходом
        if (this.currentUser) {
            await this.saveCartToFirebase();
        }

        // Переключаемся на гостевую корзину
        this.currentUser = null;
        this.cart = this.getCartFromStorage();
        this.updateCartUI();
    }

    // Загрузить корзину из Firebase
    async loadCartFromFirebase(userId) {
        try {
            console.log('Loading cart from Firebase for user:', userId);
            
            // Проверяем, доступна ли база данных
            if (!window.database) {
                console.error('Firebase Database not available');
                this.cart = this.getCartFromStorage();
                this.updateCartUI();
                return;
            }
            
            const db = window.database;
            const cartRef = db.ref(`carts/${userId}`);
            const snapshot = await cartRef.once('value');
            const firebaseCart = snapshot.val();

            console.log('Loaded cart from Firebase:', firebaseCart);

            if (firebaseCart && Array.isArray(firebaseCart)) {
                this.cart = firebaseCart;
            } else {
                this.cart = [];
            }
            
            this.saveCartToStorage();
            this.updateCartUI();
            
        } catch (error) {
            console.error('Ошибка загрузки корзины из Firebase:', error);
            // В случае ошибки загружаем из localStorage
            this.cart = this.getCartFromStorage();
            this.updateCartUI();
        }
    }

    // Сохранить корзину в Firebase
    async saveCartToFirebase() {
        if (!this.currentUser) {
            console.log('No user logged in, skipping Firebase save');
            return;
        }

        // Проверяем, доступна ли база данных
        if (!window.database) {
            console.error('Firebase Database not available');
            return;
        }

        try {
            const db = window.database;
            const cartRef = db.ref(`carts/${this.currentUser.uid}`);
            
            console.log('Saving cart to Firebase:', this.cart);
            
            // Используем set() для полной перезаписи корзины
            await cartRef.set(this.cart);
            
            console.log('Cart successfully saved to Firebase for user:', this.currentUser.uid);
            
        } catch (error) {
            console.error('Ошибка сохранения корзины в Firebase:', error);
            // Показываем пользователю сообщение об ошибке
            this.showFirebaseError();
        }
    }

    // Показать ошибку Firebase
    showFirebaseError() {
        const errorDiv = document.createElement('div');
        errorDiv.className = 'firebase-error';
        errorDiv.innerHTML = `
            <i class="fas fa-exclamation-triangle"></i>
            <span>Ошибка синхронизации корзины</span>
        `;
        errorDiv.style.cssText = `
            position: fixed;
            top: 20px;
            right: 20px;
            background: linear-gradient(135deg, #ff9800, #f57c00);
            color: white;
            padding: 15px 20px;
            border-radius: 5px;
            z-index: 10000;
            font-weight: bold;
            box-shadow: 0 4px 6px rgba(0,0,0,0.1);
        `;

        document.body.appendChild(errorDiv);

        setTimeout(() => {
            if (errorDiv.parentNode) {
                errorDiv.parentNode.removeChild(errorDiv);
            }
        }, 5000);
    }

    // Метод для принудительной синхронизации (можно вызвать из консоли для отладки)
    async forceSync() {
        console.log('Force syncing cart...');
        if (this.currentUser) {
            await this.saveCartToFirebase();
        }
    }
}

// Создаем глобальный экземпляр корзины
window.cartManager = new CartManager();