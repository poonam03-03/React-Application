import { useEffect, useState } from "react";
import {
  Box,
  Typography,
  Paper,
  Chip,
  Grid,
  TextField,
  Checkbox,
  Button,
  Divider
} from "@mui/material";
import {
  PieChart,
  Pie,
  Cell,
  Tooltip,
  ResponsiveContainer
} from "recharts";
import SearchIcon from "@mui/icons-material/Search";
import InputAdornment from "@mui/material/InputAdornment";
import DownloadIcon from "@mui/icons-material/Download";
import UploadMeeting from "./UploadMeeting";
import StatCard from "../components/StatCard";
import api from "../api/axios";
import connection from "../services/signalr";

export default function Dashboard() {
  const [meetings, setMeetings] = useState([]);
  const [selected, setSelected] = useState(null);
  const [details, setDetails] = useState({});
  const [search, setSearch] = useState("");

  const [stats, setStats] = useState({
    total: 0,
    completed: 0,
    processing: 0,
    uploaded: 0,
  });

  const loadMeetings = async () => {
    const res = await api.get("/meeting");
    setMeetings(res.data);
  };

  const loadStats = async () => {
    const res = await api.get("/dashboard/stats");
    setStats(res.data);
  };

  const loadDetails = async (id) => {
    const res = await api.get(`/meeting/${id}`);
    setSelected(id);
    setDetails(res.data);
  };

  const toggleTask = async (id) => {
    await api.put(`/meeting/actionitem/${id}`);
    const res = await api.get(`/meeting/${selected}`);
    setDetails(res.data);
    loadStats();
  };

  useEffect(() => {
    loadMeetings();
    loadStats();

    if (connection.state === "Disconnected") {
      connection.start().catch(console.error);
    }

    connection.on("MeetingUpdated", async (id) => {
      await loadMeetings();
      await loadStats();

      if (selected === id) {
        const res = await api.get(`/meeting/${id}`);
        setDetails(res.data);
      }
    });

    return () => {
      connection.off("MeetingUpdated");
    };
  }, [selected]);

  const filtered = meetings.filter((m) =>
    m.title.toLowerCase().includes(search.toLowerCase())
  );

  const chartData = [
    { name: "Completed", value: stats.completed },
    { name: "Processing", value: stats.processing },
    { name: "Uploaded", value: stats.uploaded },
  ];

return (
  <Box
    sx={{
      p: { xs: 2, md: 4 },
      bgcolor: "#F8FAFC",
      minHeight: "100vh",
    }}
  >
    {/* Header */}
    <Box mb={4}>
      <Typography variant="h4" fontWeight="700">
        Dashboard
      </Typography>
      <Typography color="text.secondary">
        Welcome back • AI Meeting Intelligence
      </Typography>
    </Box>

    {/* KPI Cards */}
    <Grid container spacing={3} mb={4}>
      <Grid size={{ xs: 6, md: 3 }}>
        <StatCard title="Meetings" value={stats.total} color="#2563EB" />
      </Grid>

      <Grid size={{ xs: 6, md: 3 }}>
        <StatCard title="Completed" value={stats.completed} color="#16A34A" />
      </Grid>

      <Grid size={{ xs: 6, md: 3 }}>
        <StatCard title="Processing" value={stats.processing} color="#F59E0B" />
      </Grid>

      <Grid size={{ xs: 6, md: 3 }}>
        <StatCard title="Uploaded" value={stats.uploaded} color="#7C3AED" />
      </Grid>
    </Grid>

    {/* Upload + Chart */}
<Box
  sx={{
    display: "grid",  
    gridTemplateColumns: { xs: "1fr", lg: "2fr 1fr" },
    gap: 3,
    mb: 4,
    alignItems: "stretch",
  }}
>
  {/* Upload */}
  <UploadMeeting refresh={loadMeetings} />

  {/* Chart */}
  <Paper
    sx={{
      p: 3,
      borderRadius: 3,
      minHeight: 320,
      display: "flex",
      mt:2.5,
      flexDirection: "column",
    }}
  >
    <Typography variant="h6" mb={2}>
      Status Distribution
    </Typography>

    <Box sx={{ flex: 1, minHeight: 220 }}>
      <ResponsiveContainer width="100%" height="100%">
        <PieChart>
          <Pie data={chartData} dataKey="value" outerRadius={80} label>
            <Cell fill="#16A34A" />
            <Cell fill="#F59E0B" />
            <Cell fill="#7C3AED" />
          </Pie>
          <Tooltip />
        </PieChart>
      </ResponsiveContainer>
    </Box>
  </Paper>
</Box>
    {/* Search */}
    <Box sx={{ mb: 3 }}>
  <TextField
    fullWidth
    placeholder="Search meetings..."
    value={search}
    onChange={(e) => setSearch(e.target.value)}
    InputProps={{
      startAdornment: (
        <InputAdornment position="start">
          <SearchIcon />
        </InputAdornment>
      ),
    }}
    sx={{
      "& .MuiOutlinedInput-root": {
        bgcolor: "white",
        borderRadius: 3,
      },
    }}
  />
</Box>

    {/* Meeting Cards */}
    {filtered.map((m) => (
      <Paper
        key={m.id}
        sx={{
          p: 2,
          mb: 2,
          borderRadius: 3,
          cursor: "pointer",
          transition: "0.2s",
          "&:hover": {
            transform: "translateY(-2px)",
            boxShadow: 4,
          },
        }}
        onClick={() => {
          if (selected === m.id) {
            setSelected(null);
          } else {
            loadDetails(m.id);
          }
        }}
      >
        {/* Header */}
        <Box display="flex" justifyContent="space-between" alignItems="center">
          <Box display="flex" alignItems="center" gap={2}>
            <Box
              sx={{
                width: 56,
                height: 56,
                bgcolor: "#DBEAFE",
                borderRadius: 2,
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                fontWeight: "bold",
                color: "#2563EB",
              }}
            >
              TXT
            </Box>

            <Box>
              <Typography fontWeight={700}>{m.title}</Typography>
              <Typography variant="body2" color="text.secondary">
                {m.fileName}
              </Typography>
              <Typography variant="caption" color="text.secondary">
                {new Date(m.uploadedAt).toLocaleString()}
              </Typography>
            </Box>
          </Box>

          <Chip
            label={m.status}
            color={
              m.status === "Completed"
                ? "success"
                : m.status === "Processing"
                ? "warning"
                : "info"
            }
          />
        </Box>

        {/* Expanded Details */}
        {selected === m.id && (
          <Box mt={3}>
            <Divider sx={{ mb: 2 }} />

            <Button
              variant="contained"
              startIcon={<DownloadIcon />}
              sx={{ mb: 2 }}
              onClick={async (e) => {
                e.stopPropagation();

                const res = await api.get(`/meeting/${m.id}/pdf`, {
                  responseType: "blob",
                });

                const url = window.URL.createObjectURL(res.data);
                const a = document.createElement("a");
                a.href = url;
                a.download = `${m.title}.pdf`;
                a.click();
              }}
            >
              Download Report
            </Button>

            {/* AI Summary */}
            <Paper
              sx={{
                p: 2.5,
                borderRadius: 3,
                mb: 2,
                color: "white",
                background:
                  "linear-gradient(135deg,#2563EB,#7C3AED)",
              }}
            >
              <Typography fontWeight="bold" mb={1}>
                🤖 AI Summary
              </Typography>

              <Typography>{details.summary}</Typography>
            </Paper>

            {/* Action Items */}
            <Paper
              sx={{
                p: 2,
                bgcolor: "#FAF5FF",
                borderRadius: 2,
              }}
            >
              <Typography fontWeight="bold" color="#7C3AED" mb={2}>
                📋 Action Items
              </Typography>

              {details.actionItems?.length ? (
                details.actionItems.map((item) => (
                  <Box
                    key={item.id}
                    sx={{
                      display: "flex",
                      alignItems: "center",
                      bgcolor: "white",
                      borderRadius: 2,
                      p: 1.5,
                      mb: 1,
                    }}
                  >
                    <Checkbox
                      checked={item.isCompleted}
                      onClick={(e) => e.stopPropagation()}
                      onChange={() => toggleTask(item.id)}
                    />

                    <Typography
                      sx={{
                        textDecoration: item.isCompleted
                          ? "line-through"
                          : "none",
                        color: item.isCompleted
                          ? "text.secondary"
                          : "text.primary",
                      }}
                    >
                      {item.task}
                    </Typography>
                  </Box>
                ))
              ) : (
                <Typography color="text.secondary">
                  No action items found.
                </Typography>
              )}
            </Paper>
          </Box>
        )}
      </Paper>
    ))}
  </Box>
);
              }