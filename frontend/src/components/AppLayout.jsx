import { Link, useLocation, useNavigate } from "react-router-dom";
import { createUseStyles } from "react-jss";
import { useAuth } from "../state/AuthContext";

const useStyles = createUseStyles((theme) => ({
  shell: {
    maxWidth: 1180,
    margin: "0 auto",
    padding: [28, 20, 40]
  },
  topBar: {
    display: "flex",
    justifyContent: "space-between",
    alignItems: "center",
    gap: 16,
    marginBottom: 28,
    padding: 18,
    borderRadius: theme.radii.lg,
    background: theme.gradients.panel,
    boxShadow: theme.shadows.panel,
    backdropFilter: "blur(18px)"
  },
  brand: {
    display: "flex",
    flexDirection: "column",
    textDecoration: "none"
  },
  brandTitle: {
    fontFamily: theme.fonts.heading,
    fontSize: 28,
    fontWeight: 800,
    color: theme.colors.navy
  },
  brandText: {
    fontSize: 13,
    color: theme.colors.textSecondary
  },
  actions: {
    display: "flex",
    gap: 12,
    alignItems: "center",
    flexWrap: "wrap"
  },
  button: {
    appearance: "none",
    border: 0,
    cursor: "pointer",
    textDecoration: "none",
    borderRadius: theme.radii.sm,
    padding: [12, 16],
    fontWeight: 700,
    fontSize: 14
  },
  primary: {
    composes: "$button",
    background: theme.colors.accent,
    color: "#fff"
  },
  ghost: {
    composes: "$button",
    background: theme.colors.surfaceMuted,
    color: theme.colors.navy
  },
  userBadge: {
    padding: [8, 12],
    borderRadius: 999,
    background: theme.colors.accentSoft,
    color: theme.colors.accentDark,
    fontWeight: 700,
    fontSize: 13
  },
  pageTitle: {
    margin: [0, 0, 8],
    fontFamily: theme.fonts.heading,
    fontSize: 34,
    color: theme.colors.navy
  },
  pageSubtitle: {
    margin: 0,
    color: theme.colors.textSecondary,
    maxWidth: 760,
    lineHeight: 1.6
  }
}));

export default function AppLayout({ title, subtitle, children }) {
  const classes = useStyles();
  const { currentUser, logout } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  function handleLogout() {
    logout();
    navigate("/login");
  }

  return (
    <div className={classes.shell}>
      <div className={classes.topBar}>
        <Link className={classes.brand} to="/">
          <span className={classes.brandTitle}>Vitalsig</span>
          <span className={classes.brandText}>Ho so bao ho thong minh qua ma QR</span>
        </Link>

        <div className={classes.actions}>
          {currentUser ? <span className={classes.userBadge}>{currentUser.fullName}</span> : null}
          {location.pathname !== "/profiles/new" ? (
            <Link className={classes.primary} to="/profiles/new">
              Tao ho so
            </Link>
          ) : null}
          <button className={classes.ghost} onClick={handleLogout} type="button">
            Dang xuat
          </button>
        </div>
      </div>

      <h1 className={classes.pageTitle}>{title}</h1>
      {subtitle ? <p className={classes.pageSubtitle}>{subtitle}</p> : null}
      <div>{children}</div>
    </div>
  );
}
