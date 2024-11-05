import React from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import Login from './login/Login';
import Register from "./register/register";
import MainPage from "./MainPage/js/MainPage"
import ProtectedRoute from './ProtectedRoute';
import PrivateChat from './ChatHub/js/PrivateChat';
import Header from './Header/Header'
import UserList from './ChatHub/js/UsersList'
const App: React.FC = () => {
    return (
        <Router>
            <Header />
            <Routes>
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register /> }/>
                <Route path="/" element={<Navigate to="/login" replace />} />
                <Route path="/main" element={<ProtectedRoute element={<MainPage />} />} />
                <Route path="/privateChat/:username" element={<ProtectedRoute element={<PrivateChat />} />} />
                <Route path="/users" element={<ProtectedRoute element={<UserList />} /> } />
            </Routes>
        </Router>
    );
};

export default App;
