// src/components/WelcomePage.js
import React, { useEffect, useState } from 'react';
import { getMovies } from '../services/api';

function WelcomePage() {
    const [movies, setMovies] = useState([]);

    useEffect(() => {
        async function fetchMovies() {
            const movieData = await getMovies();
            setMovies(movieData);
        }
        fetchMovies();
    }, []);

    return (
        <div>
            <h1>Welcome to CineBuzz</h1>
            <div className="movie-list">
                {movies.map(movie => (
                    <div key={movie.movieId} className="movie-card">
                        <h2>{movie.title}</h2>
                        <p>Genre: {movie.genres.join(', ')}</p>
                        <p>{movie.description}</p>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default WelcomePage;
