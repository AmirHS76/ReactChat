import React from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import Cookies from 'js-cookie';

const Header: React.FC = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const token = Cookies.get('token');

    const handleLogout = () => {
        Cookies.remove('token');
        navigate('/login');
    };

    const handleBack = () => {
        navigate(-1);
    };

    const isLoginOrMainPage = location.pathname === '/login' || location.pathname === '/main';

    return (
        <header>
            {token && (
                <div>
                    {!isLoginOrMainPage && <button onClick={handleBack}>Back</button>}
                    <button onClick={handleLogout}>Logout</button>
                </div>
            )}
        </header>
    );
};

export default Header;
