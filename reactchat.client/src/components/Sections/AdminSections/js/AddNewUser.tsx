import React, { useState } from "react";
import "../css/AddNewUser.css";
import userModel from "../../../../types/users";
import UserRepository from "../../../../Repositories/UserRepository";
const AddNewUser: React.FC = () => {
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [role, setRole] = useState("RegularUser");
  const [error, setError] = useState("");
  const repo = new UserRepository();
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const newUser: userModel = { id: 0, username, email, password, role };

    try {
      const response = await repo.addUser(newUser);
      if (response.status === 200) {
        alert("User added successfully!");
        setUsername("");
        setEmail("");
        setPassword("");
        setRole("RegularUser");
      }
    } catch {
      setError("Error adding user. Please try again later.");
    }
  };

  return (
    <div className="add-new-user">
      <h2>Add New User</h2>
      <form onSubmit={handleSubmit} className="user-form">
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
          <label htmlFor="email">Email</label>
          <input
            type="email"
            id="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
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
        <div className="form-group">
          <label htmlFor="role">Role</label>
          <select
            id="role"
            value={role}
            onChange={(e) => setRole(e.target.value)}
          >
            <option value="Admin">Admin</option>
            <option value="RegularUser">Regular User</option>
            <option value="Guest">Guest</option>
          </select>
        </div>
        {error && <p className="error-message">{error}</p>}
        <button type="submit" className="submit-button">
          Add User
        </button>
      </form>
    </div>
  );
};

export default AddNewUser;
