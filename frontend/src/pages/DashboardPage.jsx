import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { createUseStyles } from "react-jss";
import AppLayout from "../components/AppLayout";
import StatCard from "../components/StatCard";
import { getProfiles } from "../api/profiles";
import { useAuth } from "../state/AuthContext";

const useStyles = createUseStyles((theme) => ({
  stats: {
    display: "grid",
    gridTemplateColumns: "repeat(auto-fit, minmax(220px, 1fr))",
    gap: 16,
    marginTop: 28
  },
  panel: {
    marginTop: 24,
    borderRadius: theme.radii.lg,
    background: theme.gradients.panel,
    boxShadow: theme.shadows.panel,
    padding: 24
  },
  list: {
    display: "grid",
    gap: 14
  },
  item: {
    display: "grid",
    gridTemplateColumns: "1.4fr 0.7fr 0.7fr auto",
    gap: 14,
    alignItems: "center",
    padding: 18,
    borderRadius: theme.radii.md,
    background: "rgba(255,255,255,0.9)",
    "@media (max-width: 760px)": {
      gridTemplateColumns: "1fr"
    }
  },
  itemName: {
    fontWeight: 800,
    color: theme.colors.navy,
    marginBottom: 4
  },
  itemMeta: {
    color: theme.colors.textSecondary,
    fontSize: 14
  },
  badge: {
    display: "inline-flex",
    padding: [8, 10],
    borderRadius: 999,
    background: theme.colors.accentSoft,
    color: theme.colors.accentDark,
    fontWeight: 700,
    fontSize: 13
  },
  linkButton: {
    justifySelf: "end",
    textDecoration: "none",
    padding: [11, 14],
    borderRadius: theme.radii.sm,
    background: theme.colors.navy,
    color: "#fff",
    fontWeight: 700
  },
  empty: {
    padding: 28,
    textAlign: "center",
    color: theme.colors.textSecondary
  },
  error: {
    color: theme.colors.danger,
    fontWeight: 700
  }
}));

export default function DashboardPage() {
  const classes = useStyles();
  const { token } = useAuth();
  const [profiles, setProfiles] = useState([]);
  const [error, setError] = useState("");

  useEffect(() => {
    async function load() {
      try {
        const data = await getProfiles(token);
        setProfiles(data);
      } catch (loadError) {
        setError(loadError.message);
      }
    }

    load();
  }, [token]);

  const activeProfiles = profiles.filter((profile) => profile.isActive).length;
  const publicProfiles = profiles.filter((profile) => profile.isPublic).length;
  const totalContacts = profiles.reduce((sum, profile) => sum + profile.emergencyContactCount, 0);

  return (
    <AppLayout title="Dashboard bao ho" subtitle="Quan ly cac ho so QR de in len vong tay, the deo hoac mat sau dien thoai cho nguoi than.">
      <div className={classes.stats}>
        <StatCard label="Tong ho so" value={profiles.length} helper="So ho so dang duoc quan ly." />
        <StatCard label="Ho so dang mo" value={activeProfiles} helper="San sang de nguoi khac quet QR." />
        <StatCard label="Ho so cong khai" value={publicProfiles} helper="Da cho phep hien thong tin tren trang public." />
        <StatCard label="Lien he khan cap" value={totalContacts} helper="Tong so dau moi lien lac trong he thong." />
      </div>
      <div className={classes.panel}>
        {error ? <div className={classes.error}>{error}</div> : null}
        {!error && profiles.length === 0 ? (
          <div className={classes.empty}>Chua co ho so nao. Bam "Tao ho so" de tao QR dau tien.</div>
        ) : (
          <div className={classes.list}>
            {profiles.map((profile) => (
              <div className={classes.item} key={profile.id}>
                <div>
                  <div className={classes.itemName}>{profile.displayName}</div>
                  <div className={classes.itemMeta}>{profile.profileType} · {profile.profileCode}</div>
                </div>
                <div className={classes.badge}>{profile.emergencyContactCount} lien he</div>
                <div className={classes.itemMeta}>{profile.isPublic ? "Cong khai" : "An"} · {profile.isActive ? "Dang dung" : "Tam khoa"}</div>
                <Link className={classes.linkButton} to={`/profiles/${profile.id}`}>Xem chi tiet</Link>
              </div>
            ))}
          </div>
        )}
      </div>
    </AppLayout>
  );
}
