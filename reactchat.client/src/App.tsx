import React from "react";
import {
  BrowserRouter as Router,
  Route,
  Routes,
  Navigate,
} from "react-router-dom";
import Login from "./components/login/manual/Login";
import Register from "./components/register/register";
import MainPage from "./components/MainPage/js/MainPage";
import ProtectedRoute from "./ProtectedRoute";
import PrivateChat from "./components/privateChat/PrivateChat";
import Header from "./Header/Header";
import UserList from "./components/ChatHub/js/UsersList";
import UserManagementPage from "./components/Sections/AdminSections/js/UserManagement";
import AddNewUserPage from "./components/Sections/AdminSections/js/AddNewUser";
import { ToastContainer } from "react-toastify";
import GoogleCallback from "./components/login/google/GoogleCallback";
import GroupsPage from "./components/Sections/PublicSections/js/GroupsPage";
import NewGroupSection from "./components/Sections/PublicSections/js/NewGroupSection";
import GroupChat from "./components/Sections/PublicSections/js/GroupChat";
import SessionsSection from "./components/Sections/PublicSections/js/SessionsSection";
const App: React.FC = () => {
  return (
    <Router>
      <Header />
      <ToastContainer
        position="top-center"
        autoClose={5000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
      />
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/" element={<Navigate to="/login" replace />} />
        <Route
          path="/main"
          element={<ProtectedRoute element={<MainPage />} />}
        />
        <Route
          path="/privateChat/:username"
          element={<ProtectedRoute element={<PrivateChat />} />}
        />
        <Route
          path="/users"
          element={<ProtectedRoute element={<UserList />} />}
        />
        <Route path="*" element={<Navigate to="/" />} />
        <Route
          path="/userManagement"
          element={
            <ProtectedRoute element={<UserManagementPage />} adminOnly={true} />
          }
        />
        <Route
          path="/newUser"
          element={
            <ProtectedRoute element={<AddNewUserPage />} adminOnly={true} />
          }
        />
        <Route path="/google-callback" element={<GoogleCallback />} />
        <Route
          path="/Groups"
          element={<ProtectedRoute element={<GroupsPage />} />}
        />
        <Route
          path="/new-group"
          element={<ProtectedRoute element={<NewGroupSection />} />}
        />
        <Route
          path="/group-chat/:groupName"
          element={<ProtectedRoute element={<GroupChat />} />}
        />
        <Route
          path="/sessions"
          element={<ProtectedRoute element={<SessionsSection />} />}
        />
      </Routes>
    </Router>
  );
};

export default App;
