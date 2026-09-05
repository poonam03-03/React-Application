import { useState } from "react";
import {
  Paper,
  Typography,
  TextField,
  Button
} from "@mui/material";
//import CloudUploadIcon from "@mui/icons-material/CloudUpload";
import api from "../api/axios";

export default function UploadMeeting({ refresh }) {
  const [title, setTitle] = useState("");
  const [file, setFile] = useState(null);

  const upload = async () => {
    if (!file) return alert("Select a file");

    const form = new FormData();
    form.append("title", title);
    form.append("file", file);

    await api.post("/meeting/upload", form);

    setTitle("");
    setFile(null);

    refresh();
  };

  return (
    <Paper
    sx={{
      p: 3,
      mt:2.5,
      borderRadius: 3,
      width: "100%",
      boxSizing: "border-box",
    }}>
    <Typography variant="h6" fontWeight={700} mb={2}>
      Upload Meeting
    </Typography>
  
    <TextField
      fullWidth
      label="Meeting Title"
      value={title}
      onChange={(e) => setTitle(e.target.value)}
      sx={{ mb: 2 }}
    />
  
    <Button variant="outlined" component="label" fullWidth sx={{ py: 3 }}>
      Browse TXT / PDF / DOCX
      <input
        hidden
        type="file"
        accept=".txt,.pdf,.docx"
        onChange={(e) => setFile(e.target.files[0])}
      />
    </Button>
  
    {file && (
      <Typography variant="body2" mt={1}>
        Selected: {file.name}
      </Typography>
    )}
  
    <Button
      fullWidth 
      variant="contained"
      sx={{ mt: 2, py: 1.5, borderRadius: 2 }}
      onClick={upload}
    >
      Upload Meeting
    </Button>
  </Paper>
  );
}