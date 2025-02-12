import React, { useState, useEffect } from "react";
import "./Login.css";
import Cookies from "js-cookie";
import { useNavigate } from "react-router-dom";
import { LoginRepository } from "../../../Repositories/LoginRepository";

interface LoginResponse {
  token: string;
  refreshToken: string;
}

const Login: React.FC = () => {
  const [username, setUsername] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [message, setMessage] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();
  const repo = new LoginRepository();

  useEffect(() => {
    const token = Cookies.get("token");
    if (token) {
      navigate("/main");
    }
  }, [navigate]);

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setError(null);
    setMessage(null);

    try {
      const response = await repo.login(username, password);

      if (response.status !== 200) {
        throw new Error("Invalid username or password");
      }
      const usernameToken = username;
      Cookies.set("username", usernameToken, {
        secure: true,
        sameSite: "Strict",
      });
      const { token, refreshToken } = response.data as LoginResponse;
      Cookies.set("token", token, {
        expires: 1,
        secure: true,
        sameSite: "Strict",
      });
      Cookies.set("refreshToken", refreshToken, {
        expires: 2,
        secure: true,
        sameSite: "Strict",
      });
      console.log("Login successful");
      setMessage("Login successful! Redirecting...");

      setTimeout(() => {
        navigate("/main");
      }, 500);
    } catch (err) {
      setError("Invalid username or password");
      console.log("Error in login: " + err);
      setTimeout(() => setError(null), 3000);
    }
  };

  const handleGoogleLogin = () => {
    // Redirect to your backend Google login endpoint.
    // Adjust the URL (and port) as needed.
    window.location.href =
      "https://localhost:7240/api/ExternalAuth/google-login";
  };

  return (
    <div className="login-container">
      {message && <div className="info-message">{message}</div>}
      {error && <div className="error-message">{error}</div>}
      <h1 className="login-title">Login</h1>
      <form className="login-form" onSubmit={handleSubmit}>
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
          <label htmlFor="password">Password</label>
          <input
            type="password"
            id="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <div className="button-container">
          <button type="submit" className="login-button">
            Login
          </button>
          <button
            type="button"
            className="google-login-button"
            onClick={handleGoogleLogin}
          >
            Continue with Google
          </button>
        </div>
        <div className="login-footer">
          <p>
            Don't have an account? <a href="/register">Sign Up</a>
          </p>
        </div>
      </form>
    </div>
  );
};

export default Login;
