import { useState, useEffect, useMemo, useCallback } from "react";
import Cookies from "js-cookie";
import { Link, useNavigate } from "react-router-dom";
import { LoginRepository } from "../../../Repositories/LoginRepository";
import "./login.css";
import {
  Box,
  Button,
  IconButton,
  Paper,
  TextField,
  Typography,
} from "@mui/material";
import { Refresh } from "@mui/icons-material";
import { toast } from "react-toastify";

interface LoginResponse {
  token: string;
  refreshToken: string;
}

const Login: React.FC = () => {
  const [username, setUsername] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [captcha, setCaptcha] = useState<string | null>("");
  const [captchaImg, setCaptchaImg] = useState<string>("");
  const navigate = useNavigate();
  const repo = useMemo(() => new LoginRepository(), []);

  const fetchCaptcha = useCallback(async () => {
    try {
      const res = await repo.getCaptchaImage();
      if (res.status == 200) {
        setCaptchaImg(res.data);
      }
    } catch (err) {
      toast.error("Failed to load captcha : " + err);
    }
  }, [repo]);

  useEffect(() => {
    const token = Cookies.get("token");
    if (token) {
      navigate("/main");
    }

    fetchCaptcha();
  }, [fetchCaptcha, navigate]);

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    try {
      const response = await repo.login(username, password, captcha);

      if (response.status !== 200) {
        fetchCaptcha();
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
      toast.info("Login successful! Redirecting...");

      setTimeout(() => {
        navigate("/main");
      }, 500);
    } catch (err) {
      fetchCaptcha();
      toast.error("Invalid username or password.");
      console.log("Error in login: " + err);
    }
  };

  const handleGoogleLogin = () => {
    const backendUrl: string = import.meta.env.VITE_BACKEND_URL;
    window.location.href = backendUrl + "api/ExternalAuth/google-login";
  };

  const handleRefreshCaptcha = (e: React.MouseEvent) => {
    e.preventDefault();
    fetchCaptcha();
  };

  return (
    <Box
      sx={{
        minHeight: "100vh",
        backgroundColor: "#121212",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        p: 2,
      }}
    >
      <Paper
        elevation={10}
        sx={{
          display: "flex",
          flexWrap: "wrap",
          justifyContent: "center",
          alignItems: "center",
          maxWidth: 800,
          width: "100%",
          bgcolor: "#1e1e1e",
          boxShadow: "0 4px 20px rgba(0,0,0,0.5)",
          color: "#fff",
        }}
      >
        <Box
          sx={{
            flex: "1 1 100%",
            maxWidth: "400px",
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
          }}
        >
          <img
            src="src/assets/images.jpg"
            alt="login"
            style={{
              height: "100%",
              width: "100%",
              borderTopLeftRadius: "4px",
              borderBottomLeftRadius: "4px",
            }}
          />
        </Box>

        <Box
          sx={{
            flex: "1 1 300px",
            display: "flex",
            flexDirection: "column",
            justifyContent: "center",
            p: 3,
          }}
        >
          <Typography variant="h4" component="h1" gutterBottom>
            Login
          </Typography>
          <Box component="form" onSubmit={handleSubmit} noValidate>
            <TextField
              fullWidth
              label="Username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              margin="normal"
              required
              slotProps={{
                input: {
                  sx: { color: "#fff" },
                },
                inputLabel: {
                  sx: { color: "#aaa" },
                },
              }}
              sx={{
                "& .MuiOutlinedInput-notchedOutline": {
                  borderColor: "#444",
                },
                "&:hover .MuiOutlinedInput-notchedOutline": {
                  borderColor: "#555",
                },
                "&.Mui-focused .MuiOutlinedInput-notchedOutline": {
                  borderColor: "#03a9f4",
                },
                "& input:-webkit-autofill, & input:-webkit-autofill:hover, & input:-webkit-autofill:focus":
                  {
                    WebkitBoxShadow:
                      "0 0 0px 1000pxrgb(119, 119, 119) inset !important",
                    WebkitTextFillColor: "#fff !important",
                    transition: "background-color 600000s 0s, color 600000s 0s",
                  },
              }}
            />
            <TextField
              fullWidth
              type="password"
              label="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              margin="normal"
              required
              slotProps={{
                input: {
                  sx: { color: "#fff" },
                },
                inputLabel: {
                  sx: { color: "#aaa" },
                },
              }}
              sx={{
                "& .MuiOutlinedInput-notchedOutline": {
                  borderColor: "#444",
                },
                "&:hover .MuiOutlinedInput-notchedOutline": {
                  borderColor: "#555",
                },
                "&.Mui-focused .MuiOutlinedInput-notchedOutline": {
                  borderColor: "#03a9f4",
                },
                "& input:-webkit-autofill, & input:-webkit-autofill:hover, & input:-webkit-autofill:focus":
                  {
                    WebkitBoxShadow:
                      "0 0 0px 1000pxrgb(119, 119, 119) inset !important",
                    WebkitTextFillColor: "#fff !important",
                    transition: "background-color 600000s 0s, color 600000s 0s",
                  },
              }}
            />

            <Box sx={{ mt: 2 }}>
              <Typography variant="subtitle1">Captcha</Typography>
              <Box
                sx={{
                  display: "flex",
                  alignItems: "center",
                  gap: 1,
                  mt: 1,
                }}
              >
                {captchaImg && (
                  <img
                    src={`data:image/png;base64,${captchaImg}`}
                    alt="captcha"
                    style={{ height: "40px" }}
                  />
                )}
                <IconButton color="primary" onClick={handleRefreshCaptcha}>
                  <Refresh sx={{ color: "#fff" }} />
                </IconButton>
                <TextField
                  placeholder="Enter captcha"
                  value={captcha ?? ""}
                  onChange={(e) => setCaptcha(e.target.value)}
                  margin="normal"
                  required
                  size="small"
                  slotProps={{
                    input: {
                      sx: { color: "#fff" },
                    },
                    inputLabel: {
                      sx: { color: "#aaa" },
                    },
                  }}
                  sx={{
                    "& .MuiOutlinedInput-notchedOutline": {
                      borderColor: "#444",
                    },
                    "&:hover .MuiOutlinedInput-notchedOutline": {
                      borderColor: "#555",
                    },
                    "&.Mui-focused .MuiOutlinedInput-notchedOutline": {
                      borderColor: "#03a9f4",
                    },
                  }}
                />
              </Box>
            </Box>

            <Button
              fullWidth
              type="submit"
              variant="contained"
              sx={{
                mt: 2,
                backgroundColor: "#03a9f4",
                "&:hover": { backgroundColor: "#0288d1" },
              }}
            >
              Login
            </Button>
            <Button
              fullWidth
              variant="outlined"
              sx={{
                mt: 1,
                borderColor: "#03a9f4",
                color: "#03a9f4",
                "&:hover": {
                  backgroundColor: "rgba(3, 169, 244, 0.1)",
                  borderColor: "#03a9f4",
                },
              }}
              onClick={handleGoogleLogin}
            >
              Continue with Google
            </Button>
            <Typography variant="body2" align="center" sx={{ mt: 2 }}>
              Don't have an account?{" "}
              <Link to="/register" style={{ color: "#03a9f4" }}>
                Sign Up
              </Link>
            </Typography>
          </Box>
        </Box>
      </Paper>
    </Box>
  );
};

export default Login;
