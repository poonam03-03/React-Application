import { useEffect, useState } from "react";
import {
  Box,
  Typography,
  Grid,
  Paper
} from "@mui/material";

import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  Tooltip,
  PieChart,
  Pie,
  Cell,
  ResponsiveContainer
} from "recharts";

import StatCard from "../components/StatCard";
import api from "../api/axios";

export default function Analytics() {

  const [data, setData] = useState({
    total:0,
    completed:0,
    processing:0,
    uploaded:0,
    trend:[]
  });

  const load = async () => {
    const res = await api.get("/analytics");
    setData(res.data);
  };

  useEffect(() => {
    load();
  }, []);

  const pie = [
    { name:"Completed", value:data.completed },
    { name:"Processing", value:data.processing },
    { name:"Uploaded", value:data.uploaded }
  ];

  return (
    <Box sx={{ p: 4 }}>
  <Typography variant="h4" fontWeight="bold" mb={3}>
    Analytics
  </Typography>

  {/* KPI Cards */}
  <Grid container spacing={2} sx={{ mb: 3 }}>
    <Grid size={{ xs: 12, sm: 6, md: 3 }}>
      <StatCard title="Meetings" value={data.total} color="#2563EB" />
    </Grid>

    <Grid size={{ xs: 12, sm: 6, md: 3 }}>
      <StatCard title="Completed" value={data.completed} color="#16A34A" />
    </Grid>

    <Grid size={{ xs: 12, sm: 6, md: 3 }}>
      <StatCard title="Processing" value={data.processing} color="#F59E0B" />
    </Grid>

    <Grid size={{ xs: 12, sm: 6, md: 3 }}>
      <StatCard title="Uploaded" value={data.uploaded} color="#7C3AED" />
    </Grid>
  </Grid>

  {/* Charts */}
  <Grid container spacing={3}>
    <Grid size={{ xs: 12, md: 8 }}>
      <Paper sx={{ p: 3, height: 380 }}>
        <Typography variant="h6" mb={2}>
          Meetings Trend
        </Typography>

        <ResponsiveContainer width="100%" height="100%">
          <LineChart data={data.trend}>
            <XAxis dataKey="date" />
            <YAxis allowDecimals={false} />
            <Tooltip />
            <Line
              type="monotone"
              dataKey="count"
              stroke="#2563EB"
              strokeWidth={3}
            />
          </LineChart>
        </ResponsiveContainer>
      </Paper>
    </Grid>

    <Grid size={{ xs: 12, md: 4 }}>
      <Paper sx={{ p: 3, height: 380 }}>
        <Typography variant="h6" mb={2}>
          Status Distribution
        </Typography>

        <ResponsiveContainer width="100%" height="100%">
          <PieChart>
            <Pie
              data={pie}
              dataKey="value"
              outerRadius={90}
              label
            >
              <Cell fill="#16A34A" />
              <Cell fill="#F59E0B" />
              <Cell fill="#7C3AED" />
            </Pie>
            <Tooltip />
          </PieChart>
        </ResponsiveContainer>
      </Paper>
    </Grid>
  </Grid>
</Box>
  );
}