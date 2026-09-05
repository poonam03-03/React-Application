import { Card, CardContent, Typography } from "@mui/material";

export default function StatCard({ title, value, color }) {
  return (
    <Card sx={{ borderLeft: `6px solid ${color}`, height: "100%" }}>
      <CardContent>
        <Typography color="text.secondary" variant="body2">
          {title}
        </Typography>

        <Typography variant="h4" fontWeight="bold" mt={1}>
          {value}
        </Typography>
      </CardContent>
    </Card>
  );
}