import React from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import Login from './login/Login';
import Register from "./register/register";
import MainPage from "./MainPage/js/MainPage"
import ProtectedRoute from './ProtectedRoute';
import Chat from './ChatHub/js/Chat';
import Header from './Header/Header'
const App: React.FC = () => {
    return (
        <Router>
            <Header />
            <Routes>
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register /> }/>
                <Route path="/" element={<Navigate to="/login" replace />} />
                <Route path="/main" element={<ProtectedRoute element={<MainPage />} />} />
                <Route path="/chat" element={<ProtectedRoute element={<Chat /> } /> } />
            </Routes>
        </Router>
    );
};

export default App;
