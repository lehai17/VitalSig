import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { createUseStyles } from "react-jss";
import { useAuth } from "../state/AuthContext";

const useStyles = createUseStyles((theme) => ({
  wrapper: {
    minHeight: "100vh",
    display: "grid",
    placeItems: "center",
    padding: 24
  },
  card: {
    width: "100%",
    maxWidth: 1040,
    display: "grid",
    gridTemplateColumns: "1.1fr 0.9fr",
    overflow: "hidden",
    borderRadius: theme.radii.lg,
    boxShadow: theme.shadows.panel,
    "@media (max-width: 860px)": {
      gridTemplateColumns: "1fr"
    }
  },
  hero: {
    padding: 40,
    background: theme.gradients.hero,
    color: "#fff",
    display: "flex",
    flexDirection: "column",
    justifyContent: "space-between",
    minHeight: 520
  },
  heroTitle: {
    fontSize: 48,
    lineHeight: 1.1,
    fontWeight: 800,
    fontFamily: theme.fonts.heading,
    marginBottom: 18
  },
  heroText: {
    maxWidth: 420,
    lineHeight: 1.7,
    color: "rgba(255,255,255,0.86)"
  },
  metricGrid: {
    display: "grid",
    gridTemplateColumns: "repeat(2, 1fr)",
    gap: 14
  },
  metric: {
    padding: 18,
    borderRadius: theme.radii.md,
    background: "rgba(255,255,255,0.14)"
  },
  metricValue: {
    fontSize: 24,
    fontWeight: 800
  },
  formPanel: {
    padding: 40,
    background: "rgba(255,255,255,0.95)"
  },
  heading: {
    fontSize: 32,
    color: theme.colors.navy,
    marginBottom: 8,
    fontWeight: 800
  },
  subtitle: {
    color: theme.colors.textSecondary,
    lineHeight: 1.6,
    marginBottom: 24
  },
  tabs: {
    display: "flex",
    gap: 10,
    marginBottom: 22
  },
  tab: {
    border: 0,
    cursor: "pointer",
    borderRadius: theme.radii.sm,
    padding: [12, 14],
    fontWeight: 700,
    background: theme.colors.surfaceMuted
  },
  activeTab: {
    background: theme.colors.navy,
    color: "#fff"
  },
  form: {
    display: "grid",
    gap: 14
  },
  input: {
    border: `1px solid ${theme.colors.border}`,
    borderRadius: theme.radii.sm,
    padding: [13, 14],
    fontSize: 14
  },
  submit: {
    marginTop: 8,
    border: 0,
    cursor: "pointer",
    borderRadius: theme.radii.sm,
    background: theme.colors.accent,
    color: "#fff",
    padding: [14, 16],
    fontWeight: 800
  },
  error: {
    color: theme.colors.danger,
    fontWeight: 700
  }
}));

export default function LoginPage() {
  const classes = useStyles();
  const navigate = useNavigate();
  const { login, register } = useAuth();
  const [mode, setMode] = useState("login");
  const [form, setForm] = useState({
    fullName: "",
    email: "demo@vitalsig.local",
    phoneNumber: "",
    password: "Demo@123"
  });
  const [error, setError] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);

  function updateField(field, value) {
    setForm((current) => ({ ...current, [field]: value }));
  }

  async function handleSubmit(event) {
    event.preventDefault();
    setError("");
    setIsSubmitting(true);

    try {
      if (mode === "login") {
        await login({
          email: form.email,
          password: form.password
        });
      } else {
        await register(form);
      }

      navigate("/");
    } catch (submissionError) {
      setError(submissionError.message);
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <div className={classes.wrapper}>
      <div className={classes.card}>
        <div className={classes.hero}>
          <div>
            <div className={classes.heroTitle}>QR cuu ho cho nhung khoanh khac khong the tu noi.</div>
            <div className={classes.heroText}>
              Tao ho so bao ho, in ma QR len vong tay hoac the deo, de nguoi xung quanh co
              the lien he nguoi than va xem thong tin y te can thiet trong tinh huong khan cap.
            </div>
          </div>
          <div className={classes.metricGrid}>
            <div className={classes.metric}>
              <div className={classes.metricValue}>1 QR</div>
              <div>Cho lien he khan cap va huong dan so cuu co ban</div>
            </div>
            <div className={classes.metric}>
              <div className={classes.metricValue}>2 luong</div>
              <div>Tim nguoi lac va ho so benh ly tren cung mot nen tang</div>
            </div>
          </div>
        </div>
        <div className={classes.formPanel}>
          <div className={classes.heading}>Dang nhap Vitalsig</div>
          <div className={classes.subtitle}>
            Ban co the dang nhap bang tai khoan demo de test nhanh hoac dang ky moi ngay tren
            giao dien nay.
          </div>
          <div className={classes.tabs}>
            <button className={`${classes.tab} ${mode === "login" ? classes.activeTab : ""}`} onClick={() => setMode("login")} type="button">
              Dang nhap
            </button>
            <button className={`${classes.tab} ${mode === "register" ? classes.activeTab : ""}`} onClick={() => setMode("register")} type="button">
              Dang ky
            </button>
          </div>
          <form className={classes.form} onSubmit={handleSubmit}>
            {mode === "register" ? (
              <>
                <input className={classes.input} placeholder="Ho ten" value={form.fullName} onChange={(event) => updateField("fullName", event.target.value)} />
                <input className={classes.input} placeholder="So dien thoai" value={form.phoneNumber} onChange={(event) => updateField("phoneNumber", event.target.value)} />
              </>
            ) : null}
            <input className={classes.input} placeholder="Email" value={form.email} onChange={(event) => updateField("email", event.target.value)} />
            <input className={classes.input} placeholder="Mat khau" type="password" value={form.password} onChange={(event) => updateField("password", event.target.value)} />
            {error ? <div className={classes.error}>{error}</div> : null}
            <button className={classes.submit} disabled={isSubmitting} type="submit">
              {isSubmitting ? "Dang xu ly..." : mode === "login" ? "Vao dashboard" : "Tao tai khoan"}
            </button>
          </form>
        </div>
      </div>
    </div>
  );
}
