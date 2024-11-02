import React from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import Login from './login/Login';
import Register from "./register/register";
const App: React.FC = () => {
    return (
        <Router>
            <Routes>
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register /> }/>
                <Route path="/" element={<Navigate to="/login" replace />} /> {/* Redirect from root to /login */}
                {/* Add other routes here */}
            </Routes>
        </Router>
    );
};

export default App;
