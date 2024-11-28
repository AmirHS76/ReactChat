import React, { useEffect, useState } from 'react';
import { Navigate } from 'react-router-dom';
import Cookies from 'js-cookie';
import { checkAuthToken, handleUnauthenticated } from './services/authService';
import { fetchUserRole } from './services/userService';
interface ProtectedRouteProps {
    element: React.ReactNode;
    adminOnly?: boolean;
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ element, adminOnly = false }) => {
    const [isAuthenticated, setIsAuthenticated] = useState<boolean | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [isAdmin, setIsAdmin] = useState<boolean>(false);

    useEffect(() => {
        const verifyAuthentication = async () => {
            const token = Cookies.get('token');

            if (!token) {
                handleUnauthenticated();
                setLoading(false);
                return;
            }

            try {
                const isAuthenticated = await checkAuthToken(token);
                setIsAuthenticated(isAuthenticated);

                if (isAuthenticated) {
                    const userRole = await fetchUserRole(token);
                    setIsAdmin(userRole === 'Admin');
                } else {
                    handleUnauthenticated();
                }
            } catch (error) {
                console.error('Error verifying authentication:', error);
                handleUnauthenticated();
            } finally {
                setLoading(false);
            }
        };

        verifyAuthentication();
    }, []);

    if (loading) {
        return <div>Loading...</div>;
    }

    if (adminOnly && !isAdmin) {
        return <Navigate to="/main" />;
    }

    return isAuthenticated ? (
        <>{element}</>
    ) : (
        <Navigate to="/login" />
    );
};

export default ProtectedRoute;
