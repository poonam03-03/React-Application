import { useEffect, useState } from "react";
import {
  Box,
  Paper,
  Typography,
  Avatar,
  Divider,
  CircularProgress
} from "@mui/material";
import api from "../api/axios";

export default function Profile() {
  const [user, setUser] = useState(null);

  useEffect(() => {
    const loadProfile = async () => {
      try {
        const res = await api.get("/user/profile");
        setUser(res.data);
      } catch (err) {
        console.error(err);
      }
    };

    loadProfile();
  }, []);

  if (!user) {
    return (
      <Box sx={{ p: 4, textAlign: "center" }}>
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Box sx={{ p: 4 }}>
      <Typography variant="h4" fontWeight="bold" mb={3}>
        My Profile
      </Typography>

      <Paper sx={{ p: 4, maxWidth: 650, borderRadius: 3 }}>
        <Avatar
          sx={{
            width: 90,
            height: 90,
            bgcolor: "#2563EB",
            fontSize: 34,
            mb: 2
          }}
        >
          {user.name?.charAt(0).toUpperCase()}
        </Avatar>

        <Typography variant="h5">{user.name}</Typography>
        <Typography color="text.secondary">{user.role}</Typography>

        <Divider sx={{ my: 3 }} />

        <Typography><b>Email:</b> {user.email}</Typography>
        <Typography mt={2}><b>Role:</b> {user.role}</Typography>
        <Typography mt={2}><b>User ID:</b> {user.id}</Typography>
      </Paper>
    </Box>
  );
}