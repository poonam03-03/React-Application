import { Box, Typography, Button, Divider } from "@mui/material";
import { Link, useLocation, useNavigate } from "react-router-dom";

import DashboardIcon from "@mui/icons-material/Dashboard";
import DescriptionIcon from "@mui/icons-material/Description";
import AnalyticsIcon from "@mui/icons-material/Analytics";
import PersonIcon from "@mui/icons-material/Person";
import LogoutIcon from "@mui/icons-material/Logout";

export default function Sidebar() {
  const { pathname } = useLocation();
  const navigate = useNavigate();

  const menus = [
    { name: "Dashboard", path: "/", icon: <DashboardIcon /> },
    { name: "Meetings", path: "/meeting", icon: <DescriptionIcon /> },
    { name: "Analytics", path: "/analytics", icon: <AnalyticsIcon /> },
    { name: "Profile", path: "/profile", icon: <PersonIcon /> },
  ];

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/login", { replace: true });
  };

  return (
    <Box
    sx={{
      width: 240,
      minWidth: 240,
      bgcolor: "#0F172A",
      color: "white",
      p: 2.5,
      display: "flex",
      flexDirection: "column",
  
      position: "sticky",
      top: 0,
      height: "100vh",
      alignSelf: "flex-start",
    }}
  >
      <Typography variant="h5" fontWeight="bold" mb={4}>
        AI Meet
      </Typography>

      {menus.map((m) => (
        <Button
          key={m.path}
          component={Link}
          to={m.path}
          startIcon={m.icon}
          fullWidth
          sx={{
            justifyContent: "flex-start",
            color: "white",
            mb: 1,
            py: 1.2,
            borderRadius: 2,
            bgcolor: pathname === m.path ? "#2563EB" : "transparent",
            "&:hover": {
              bgcolor: pathname === m.path ? "#1D4ED8" : "#374151",
            },
          }}
        >
          {m.name}
        </Button>
      ))}

      <Box sx={{ flexGrow: 1 }} />

      <Divider sx={{ bgcolor: "#374151", mb: 2 }} />

      <Button
        fullWidth
        color="error"
        variant="outlined"
        startIcon={<LogoutIcon />}
        onClick={handleLogout}
        sx={{
          borderColor: "#EF4444",
          color: "#FCA5A5",
          "&:hover": {
            borderColor: "#DC2626",
            bgcolor: "rgba(220,38,38,0.1)",
          },
        }}
      >
        Logout
      </Button>
    </Box>
  );
}