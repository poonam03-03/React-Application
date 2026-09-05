import { Routes, Route, Navigate } from "react-router-dom";

import MainLayout from "./layout/MainLayout";
import Login from "./pages/Login";
import Dashboard from "./pages/Dashboard";
import Meetings from "./pages/Meeting";
import Analytics from "./pages/Analytics";
import Profile from "./pages/Profile";

function ProtectedRoute({ children }) {
  const token = localStorage.getItem("token");
  return token ? children : <Navigate to="/login" replace />;
}

function App() {
  return (
    <Routes>
      {/* Public Route */}
      <Route path="/login" element={<Login />} />

      {/* Protected Routes */}
      <Route
        element={
          <ProtectedRoute>
            <MainLayout />
          </ProtectedRoute>
        }
      >
        <Route path="/" element={<Dashboard />} />
        <Route path="/meeting" element={<Meetings />} />
        <Route path="/analytics" element={<Analytics />} />
        <Route path="/profile" element={<Profile />} />
      </Route>

      {/* Default */}
      <Route path="*" element={<Navigate to="/login" replace />} />
    </Routes>
  );
}

export default App;