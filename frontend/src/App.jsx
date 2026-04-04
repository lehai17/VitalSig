import { Navigate, Route, Routes } from "react-router-dom";
import { createUseStyles } from "react-jss";
import { useAuth } from "./state/AuthContext";
import LoginPage from "./pages/LoginPage";
import DashboardPage from "./pages/DashboardPage";
import CreateProfilePage from "./pages/CreateProfilePage";
import ProfileDetailPage from "./pages/ProfileDetailPage";

const useStyles = createUseStyles((theme) => ({
  appShell: {
    minHeight: "100vh",
    background: theme.gradients.page,
    color: theme.colors.textPrimary,
    fontFamily: theme.fonts.body
  }
}));

function PrivateRoute({ children }) {
  const { token, bootstrapped } = useAuth();

  if (!bootstrapped) {
    return null;
  }

  return token ? children : <Navigate to="/login" replace />;
}

export default function App() {
  const classes = useStyles();

  return (
    <div className={classes.appShell}>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/" element={<PrivateRoute><DashboardPage /></PrivateRoute>} />
        <Route path="/profiles/new" element={<PrivateRoute><CreateProfilePage /></PrivateRoute>} />
        <Route path="/profiles/:profileId" element={<PrivateRoute><ProfileDetailPage /></PrivateRoute>} />
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </div>
  );
}
