import { Box } from "@mui/material";
import { Outlet } from "react-router-dom";
import Sidebar from "./Sidebar";

export default function MainLayout() {
  return (
    <Box
      sx={{
        display: "flex",
        minHeight: "100vh",
        bgcolor: "#F8FAFC",
      }}
    >
      <Sidebar />

      <Box
        component="main"
        sx={{
          flex: 1,
          p: 3,
          overflow: "auto",
        }}
      >
        <Outlet />
      </Box>
    </Box>
  );
}