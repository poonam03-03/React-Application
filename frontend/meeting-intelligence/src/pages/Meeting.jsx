import { useEffect, useState } from "react";
import {
  Box,
  Typography,
  Chip,
  TextField,
  Paper,
  Button
} from "@mui/material";
import { DataGrid } from "@mui/x-data-grid";
import VisibilityIcon from "@mui/icons-material/Visibility";
import api from "../api/axios";
import DeleteIcon from "@mui/icons-material/Delete";

export default function Meetings() {
  const [rows, setRows] = useState([]);
  const [search, setSearch] = useState("");

  const load = async () => {
    const res = await api.get("/meeting");
    setRows(res.data);
  };

  useEffect(() => {
    load();
  }, []);

  const filtered = rows.filter(x =>
    x.title.toLowerCase().includes(search.toLowerCase())
  );

  const columns = [
    {
      field: "title",
      headerName: "Meeting Title",
      flex: 1
    },
    {
      field: "status",
      headerName: "Status",
      width: 140,
      renderCell: (params) => (
        <Chip
          size="small"
          label={params.value}
          color={
            params.value === "Completed"
              ? "success"
              : params.value === "Processing"
              ? "warning"
              : "info"
          }
        />
      )
    },
    {
      field: "uploadedAt",
      headerName: "Uploaded",
      width: 180,
      valueGetter: (value) =>
        new Date(value).toLocaleDateString()
    },
    {
        field: "action",
        headerName: "Actions",
        width: 180,
        renderCell: (params) => (
          <Box>
            <Button
              size="small"
              startIcon={<VisibilityIcon />}
            >
              View
            </Button>
      
            <Button
              color="error"
              size="small"
              startIcon={<DeleteIcon />}
              onClick={async () => {
                await api.delete(
                  `/meeting/${params.row.id}`
                );
                load();
              }}
            >
              Delete
            </Button>
          </Box>
        )
    }
  ];

  return (
    <Box>
      <Typography
        variant="h4"
        fontWeight="bold"
        mb={3}
      >
        Meetings
      </Typography>

      <Paper sx={{ p: 2, mb: 3 }}>
        <TextField
          fullWidth
          label="Search Meetings"
          value={search}
          onChange={(e) =>
            setSearch(e.target.value)
          }
        />
      </Paper>

      <Paper sx={{ height: 500 }}>
        <DataGrid
          rows={filtered}
          columns={columns}
          getRowId={(row) => row.id}
          pageSizeOptions={[5, 10]}
        />
      </Paper>
    </Box>
  );
}