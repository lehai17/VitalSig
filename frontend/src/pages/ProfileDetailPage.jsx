import { useEffect, useState } from "react";
import { createUseStyles } from "react-jss";
import { useParams } from "react-router-dom";
import AppLayout from "../components/AppLayout";
import { getProfileById, getQrCode, getScanLogs, regenerateQrCode } from "../api/profiles";
import { useAuth } from "../state/AuthContext";

const useStyles = createUseStyles((theme) => ({
  grid: {
    marginTop: 24,
    display: "grid",
    gridTemplateColumns: "1.1fr 0.9fr",
    gap: 18,
    "@media (max-width: 980px)": {
      gridTemplateColumns: "1fr"
    }
  },
  panel: {
    borderRadius: theme.radii.lg,
    background: theme.gradients.panel,
    boxShadow: theme.shadows.panel,
    padding: 24
  },
  sectionTitle: {
    margin: [0, 0, 14],
    color: theme.colors.navy,
    fontWeight: 800,
    fontSize: 22
  },
  detailList: {
    display: "grid",
    gap: 12
  },
  detailRow: {
    paddingBottom: 12,
    borderBottom: `1px solid ${theme.colors.border}`
  },
  label: {
    fontSize: 12,
    textTransform: "uppercase",
    letterSpacing: 1,
    color: theme.colors.textSecondary,
    marginBottom: 4
  },
  value: {
    color: theme.colors.textPrimary,
    lineHeight: 1.6
  },
  qrPreview: {
    width: "100%",
    maxWidth: 320,
    borderRadius: theme.radii.md,
    background: "#fff",
    padding: 16,
    boxShadow: theme.shadows.soft
  },
  qrImage: {
    width: "100%",
    display: "block"
  },
  button: {
    marginTop: 14,
    border: 0,
    cursor: "pointer",
    borderRadius: theme.radii.sm,
    background: theme.colors.accent,
    color: "#fff",
    padding: [12, 16],
    fontWeight: 800
  },
  scanList: {
    display: "grid",
    gap: 12
  },
  scanItem: {
    padding: 16,
    borderRadius: theme.radii.md,
    background: "rgba(255,255,255,0.88)"
  },
  publicLink: {
    wordBreak: "break-all",
    color: theme.colors.accentDark
  },
  error: {
    color: theme.colors.danger,
    fontWeight: 700
  }
}));

export default function ProfileDetailPage() {
  const classes = useStyles();
  const { token } = useAuth();
  const { profileId } = useParams();
  const [profile, setProfile] = useState(null);
  const [qr, setQr] = useState(null);
  const [scanLogs, setScanLogs] = useState([]);
  const [error, setError] = useState("");

  useEffect(() => {
    async function load() {
      try {
        const [profileData, qrData, scanLogData] = await Promise.all([
          getProfileById(profileId, token),
          getQrCode(profileId, token),
          getScanLogs(profileId, token)
        ]);

        setProfile(profileData);
        setQr(qrData);
        setScanLogs(scanLogData);
      } catch (loadError) {
        setError(loadError.message);
      }
    }

    load();
  }, [profileId, token]);

  async function handleRegenerateQr() {
    try {
      const qrData = await regenerateQrCode(profileId, token);
      setQr(qrData);
    } catch (qrError) {
      setError(qrError.message);
    }
  }

  return (
    <AppLayout title={profile?.displayName ?? "Chi tiet ho so"} subtitle="Theo doi QR dang active, thong tin y te hien thi cong khai va lich su quet tu nguoi ho tro.">
      {error ? <div className={classes.error}>{error}</div> : null}
      <div className={classes.grid}>
        <div className={classes.panel}>
          <div className={classes.sectionTitle}>Thong tin ho so</div>
          {profile ? (
            <div className={classes.detailList}>
              <div className={classes.detailRow}>
                <div className={classes.label}>Loai ho so</div>
                <div className={classes.value}>{profile.profileType}</div>
              </div>
              <div className={classes.detailRow}>
                <div className={classes.label}>Ghi chu nhan dien</div>
                <div className={classes.value}>{profile.identificationNote || "Chua co"}</div>
              </div>
              <div className={classes.detailRow}>
                <div className={classes.label}>Lien he khan cap</div>
                <div className={classes.value}>
                  {profile.emergencyContacts?.map((contact) => (
                    <div key={`${contact.phoneNumber}-${contact.id}`}>
                      {contact.fullName} · {contact.relationship} · {contact.phoneNumber}
                    </div>
                  ))}
                </div>
              </div>
              <div className={classes.detailRow}>
                <div className={classes.label}>Thong tin y te</div>
                <div className={classes.value}>
                  Nhom mau: {profile.medicalInfo?.bloodType || "Chua co"}
                  <br />
                  Benh nen: {profile.medicalInfo?.chronicDiseases || "Chua co"}
                  <br />
                  Di ung: {profile.medicalInfo?.allergies || "Chua co"}
                </div>
              </div>
            </div>
          ) : null}
        </div>
        <div className={classes.panel}>
          <div className={classes.sectionTitle}>QR dang su dung</div>
          {qr ? (
            <>
              <div className={classes.qrPreview}>
                <img alt="Vitalsig QR" className={classes.qrImage} src={qr.qrImageDataUrl} />
              </div>
              <button className={classes.button} onClick={handleRegenerateQr} type="button">
                Tao lai QR
              </button>
              <div className={classes.detailRow} style={{ marginTop: 18, borderBottom: 0, paddingBottom: 0 }}>
                <div className={classes.label}>Public link</div>
                <div className={`${classes.value} ${classes.publicLink}`}>{qr.publicUrl}</div>
              </div>
            </>
          ) : null}
        </div>
      </div>
      <div className={classes.panel} style={{ marginTop: 18 }}>
        <div className={classes.sectionTitle}>Lich su quet</div>
        <div className={classes.scanList}>
          {scanLogs.length === 0 ? (
            <div className={classes.value}>Chua co luot quet nao duoc ghi nhan.</div>
          ) : (
            scanLogs.map((scanLog) => (
              <div className={classes.scanItem} key={scanLog.id}>
                <strong>{scanLog.actionType || "Viewed"}</strong>
                <div>{new Date(scanLog.scannedAtUtc).toLocaleString()}</div>
                <div>{scanLog.locationText || "Khong co vi tri"}</div>
                <div>{scanLog.note || "Khong co ghi chu"}</div>
              </div>
            ))
          )}
        </div>
      </div>
    </AppLayout>
  );
}
