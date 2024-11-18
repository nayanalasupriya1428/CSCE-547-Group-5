// src/components/CartPage.js
import React, { useEffect, useState } from 'react';
import { getCart } from '../services/api';

function CartPage() {
    const [cart, setCart] = useState(null);
    const cartId = 1;

    useEffect(() => {
        async function fetchCart() {
            const cartData = await getCart(cartId);
            setCart(cartData);
        }
        fetchCart();
    }, []);

    const calculateTotal = () => {
        return cart.items.reduce((total, item) => total + item.quantity * item.ticket.price, 0);
    };

    return cart ? (
        <div>
            <h1>Your Cart</h1>
            <ul>
                {cart.items.map((item) => (
                    <li key={item.cartItemId}>
                        {item.ticket.title} - {item.quantity} x ${item.ticket.price}
                    </li>
                ))}
            </ul>
            <p>Total: ${calculateTotal()}</p>
        </div>
    ) : (
        <p>Loading...</p>
    );
}

export default CartPage;
