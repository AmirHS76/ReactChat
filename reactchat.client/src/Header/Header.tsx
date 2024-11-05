import React from 'react';
import { useNavigate } from 'react-router-dom';
import Cookies from 'js-cookie';

const Header: React.FC = () => {
    const navigate = useNavigate();
    const token = Cookies.get('token');

    const handleLogout = () => {
        Cookies.remove('token');
        navigate('/login');
    };

    return (
        <header>
            {token && (
                <button onClick={handleLogout}>Logout</button>
            )}
        </header>
    );
};

export default Header;
