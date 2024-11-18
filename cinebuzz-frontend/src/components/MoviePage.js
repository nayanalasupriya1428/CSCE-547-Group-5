// src/components/MoviePage.js
import React, { useState } from 'react';
import { addToCart } from '../services/api';

function MoviePage({ movie }) {
    const [quantity, setQuantity] = useState(1);
    const cartId = 1; // Placeholder for demo; replace with actual cart ID

    const handleAddToCart = async () => {
        if (quantity > 0) {
            await addToCart(cartId, movie.movieId, quantity);
            alert('Tickets added to cart');
        } else {
            alert('Quantity should be a positive number');
        }
    };

    return (
        <div>
            <h1>{movie.title}</h1>
            <p>Showtime: {movie.showtime}</p>
            <input
                type="number"
                value={quantity}
                onChange={(e) => setQuantity(Math.max(1, e.target.value))}
                min="1"
            />
            <button onClick={handleAddToCart}>Add to Cart</button>
        </div>
    );
}

export default MoviePage;
