// ProtectedRoute.tsx
import React, { useEffect, useState } from 'react';
import { Navigate } from 'react-router-dom';
import Cookies from 'js-cookie';
import axios from 'axios';

interface ProtectedRouteProps {
    element: React.ReactNode; // The component to render if authenticated
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ element }) => {
    const [isAuthenticated, setIsAuthenticated] = useState<boolean | null>(null); // State to hold authentication status
    const [loading, setLoading] = useState<boolean>(true); // State to manage loading

    useEffect(() => {
        const checkAuthentication = async () => {
            const token = Cookies.get('token'); // Get the token from cookies

            if (!token) {
                setIsAuthenticated(false); // No token means not authenticated
                setLoading(false);
                return;
            }

            try {
                // Send a request to the backend to verify the token
                const response = await axios.get('https://localhost:7240/auth', {
                    headers: { Authorization: `Bearer ${token}` }
                });
                setIsAuthenticated(response.status === 200); // Set authenticated status based on response
            } catch (error) {
                setIsAuthenticated(false); // If there's an error, set to not authenticated
            } finally {
                setLoading(false); // Set loading to false after the check is complete
            }
        };

        checkAuthentication(); // Call the authentication check
    }, []); // Empty dependency array to run once on mount

    // While loading, you can return a loading indicator or null
    if (loading) {
        return <div>Loading...</div>; // Replace with a spinner or loading message as needed
    }

    // If authenticated, render the protected element; otherwise, redirect to login
    return isAuthenticated ? (
        <>{element}</>
    ) : (
        <Navigate to="/login" />
    );
};

export default ProtectedRoute;
