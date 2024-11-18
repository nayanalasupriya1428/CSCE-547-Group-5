// src/services/api.js
import axios from 'axios';

const API_BASE_URL = 'http://localhost:5231/api'; // Adjust according to your backend setup

export const getMovies = async () => {
    const response = await axios.get(`${API_BASE_URL}/Movies`);
    return response.data;
};

export const addToCart = (cartId, ticketId, quantity) => {
    return axios.post(`${API_BASE_URL}/Carts/AddTicketToCart`, { cartId, ticketId, quantity });
};

export const getCart = (cartId) => {
    return axios.get(`${API_BASE_URL}/Carts/${cartId}`);
};

export const checkout = (cartId, cardDetails) => {
    return axios.post(`${API_BASE_URL}/PaymentRequests/ProcessPayment`, {
        cartId,
        cardNumber: cardDetails.cardNumber,
        expirationDate: cardDetails.expirationDate,
        cardholderName: cardDetails.cardholderName,
        cvc: cardDetails.cvc
    });
};
