// ProtectedRoute.tsx
import React, { useEffect, useState } from 'react';
import { Navigate } from 'react-router-dom';
import Cookies from 'js-cookie';
import axios from 'axios';

interface ProtectedRouteProps {
    element: React.ReactNode;
    adminOnly?: boolean
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ element, adminOnly = false }) => {
    const [isAuthenticated, setIsAuthenticated] = useState<boolean | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [isAdmin, setIsAdmin] = useState<boolean>(false);

    useEffect(() => {
        const checkAuthentication = async () => {
            const token = Cookies.get('token');

            if (!token) {
                setIsAuthenticated(false);
                setLoading(false);
                return;
            }

            try {
                const response = await axios.get('https://localhost:7240/auth', {
                    headers: { Authorization: `Bearer ${token}` }
                });
                setIsAuthenticated(response.status === 200);
                isAdminCheck(token);
            } catch (error) {
                console.log(error)
                setIsAuthenticated(false);
            } finally {
                setLoading(false);
            }
        };

        checkAuthentication();
    }, []);
    const isAdminCheck = async (token) => {
        const response = await axios.get('https://localhost:7240/user/GetUserRole', {
            headers: { Authorization: `Bearer ${token}` }
        });
        const user = await response.data;
        if (user.role === "Admin")
            setIsAdmin(true);
        else
            setIsAdmin(false);
    }
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
