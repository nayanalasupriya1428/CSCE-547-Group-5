// src/components/CheckoutPage.js
import React, { useState } from 'react';
import { checkout } from '../services/api';

function CheckoutPage({ cart }) {
    const [cardDetails, setCardDetails] = useState({
        cardNumber: '',
        expirationDate: '',
        cardholderName: '',
        cvc: ''
    });

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setCardDetails({ ...cardDetails, [name]: value });
    };

    const handleCheckout = async () => {
        try {
            await checkout(cart.cartId, cardDetails);
            alert('Payment successful!');
        } catch (error) {
            alert('Payment failed');
        }
    };

    return (
        <div>
            <h1>Checkout</h1>
            <p>Total: ${cart.total}</p>
            <input
                type="text"
                name="cardNumber"
                placeholder="Card Number"
                value={cardDetails.cardNumber}
                onChange={handleInputChange}
            />
            <input
                type="text"
                name="expirationDate"
                placeholder="Expiration Date (MM/YY)"
                value={cardDetails.expirationDate}
                onChange={handleInputChange}
            />
            <input
                type="text"
                name="cardholderName"
                placeholder="Cardholder Name"
                value={cardDetails.cardholderName}
                onChange={handleInputChange}
            />
            <input
                type="text"
                name="cvc"
                placeholder="CVC"
                value={cardDetails.cvc}
                onChange={handleInputChange}
            />
            <button onClick={handleCheckout}>Complete Payment</button>
        </div>
    );
}

export default CheckoutPage;
