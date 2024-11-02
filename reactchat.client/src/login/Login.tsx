// src/login/Login.tsx
import React, { useState } from 'react';
import './Login.css';
import axios from 'axios';
import Cookies from 'js-cookie';
import { useNavigate } from 'react-router-dom';
const Login: React.FC = () => {
    const [username, setUsername] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const [message, setMessage] = useState<string | null>(null);
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate(); // Initialize useNavigate

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        setError(null);
        setMessage(null);

        try {
            const response = await axios.post('https://localhost:7240/login/login', { 
                username,
                password
            });

            if (response.status != 200) {
                throw new Error('Invalid username or password');
            }

            const token = response.data.token;
            Cookies.set('token', token, { expires: 7, secure: true, sameSite: 'Strict' });
            console.log('Login successful');
            setMessage('Login successful! Redirecting...');

            setTimeout(() => {
                navigate('/main'); // Use navigate to go to the main page
            }, 500);
        } catch (err) {
            setError('Invalid username or password');
            console.log('Error in login : ' + err);
            setTimeout(() => setError(null), 3000);
        }
    };

    return (
        <div className="login-container">
            {message && <div className="info-message">{message}</div>}
            {error && <div className="error-message">{error}</div>}
            <h1 className="login-title">Login</h1>
            <form className="login-form" onSubmit={handleSubmit}>
                <div className="form-group">
                    <label htmlFor="username">Username</label>
                    <input
                        type="text"
                        id="username"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        required
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="password">Password</label>
                    <input
                        type="password"
                        id="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                </div>
                <div className="button-container">
                    <button type="submit" className="login-button">Login</button>
                </div>
                <div className="login-footer">
                    <p>
                        Don't have an account? <a href="/register">Sign Up</a>
                    </p>
                </div>
            </form>
        </div>
    );
};

export default Login;
