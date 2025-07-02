// src/register/Register.tsx
import React, { useState } from "react";
import "./register.css";
import RegisterRepository from "../../Repositories/RegisterRepository";
import { Box, Paper, Stack, TextField, Typography } from "@mui/material";
const Register: React.FC = () => {
  const [username, setUsername] = useState<string>("");
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [confirmPassword, setConfirmPassword] = useState<string>("");
  const [message, setMessage] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);
  const RegisterRepo = new RegisterRepository();

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setError(null);
    setMessage(null);

    if (password !== confirmPassword) {
      setError("Passwords do not match");
      setTimeout(() => setError(null), 3000);
      return;
    }

    try {
      const response = await RegisterRepo.register(username, password, email);

      if (response.status !== 200) {
        const errorData = response.data;
        const errorString = errorData.join("\n");
        throw errorString;
      }

      const data = response.data;
      console.log("Registration successful:", data);
      setMessage("Registration successful! Redirecting to login...");

      setTimeout(() => {
        window.location.href = "/login";
      }, 2000);
    } catch (err) {
      setError("An error occurred during registration. : " + err);
      setTimeout(() => setError(null), 3000);
    }
  };

  return (
    <Stack direction="row">
      <Typography variant="h1" color="primary">
        Register
      </Typography>
      <form onSubmit={handleSubmit}>
        <Paper>
          <Box className="form-group">
            <TextField></TextField>
            <label htmlFor="username">Username</label>
            <input
              type="text"
              id="username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
            />
          </Box>
          <div className="form-group">
            <label htmlFor="email">Email</label>
            <input
              type="email"
              id="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
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
            <label htmlFor="confirmPassword">Confirm Password</label>
            <input
              type="password"
              id="confirmPassword"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              required
            />
          </div>
          <div className="button-container">
            <button type="submit" className="register-button">
              Register
            </button>
          </div>
          <div className="register-footer">
            <p>
              Already have an account? <a href="/login">Login</a>
            </p>
          </div>
        </Paper>
      </form>
    </Stack>
  );
};

export default Register;
