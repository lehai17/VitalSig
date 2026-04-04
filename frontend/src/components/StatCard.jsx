import { createUseStyles } from "react-jss";

const useStyles = createUseStyles((theme) => ({
  card: {
    borderRadius: theme.radii.md,
    background: theme.gradients.panel,
    boxShadow: theme.shadows.soft,
    padding: 22,
    minHeight: 128
  },
  label: {
    fontSize: 12,
    textTransform: "uppercase",
    letterSpacing: 1.2,
    color: theme.colors.textSecondary,
    marginBottom: 10
  },
  value: {
    fontSize: 34,
    fontWeight: 800,
    color: theme.colors.navy,
    marginBottom: 8
  },
  helper: {
    color: theme.colors.textSecondary,
    lineHeight: 1.5
  }
}));

export default function StatCard({ label, value, helper }) {
  const classes = useStyles();

  return (
    <div className={classes.card}>
      <div className={classes.label}>{label}</div>
      <div className={classes.value}>{value}</div>
      <div className={classes.helper}>{helper}</div>
    </div>
  );
}
