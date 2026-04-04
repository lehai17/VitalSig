import { useState } from "react";
import { createUseStyles } from "react-jss";

const useStyles = createUseStyles((theme) => ({
  panel: {
    marginTop: 26,
    padding: 24,
    borderRadius: theme.radii.lg,
    background: theme.gradients.panel,
    boxShadow: theme.shadows.panel
  },
  grid: {
    display: "grid",
    gridTemplateColumns: "repeat(auto-fit, minmax(220px, 1fr))",
    gap: 16
  },
  field: {
    display: "flex",
    flexDirection: "column",
    gap: 8
  },
  full: {
    gridColumn: "1 / -1"
  },
  label: {
    fontWeight: 700,
    color: theme.colors.navy
  },
  input: {
    border: `1px solid ${theme.colors.border}`,
    borderRadius: theme.radii.sm,
    padding: [12, 14],
    fontSize: 14,
    background: "#fff"
  },
  textarea: {
    composes: "$input",
    minHeight: 104,
    resize: "vertical"
  },
  sectionTitle: {
    margin: [28, 0, 14],
    fontSize: 20,
    fontWeight: 800,
    color: theme.colors.navy
  },
  rowActions: {
    display: "flex",
    justifyContent: "space-between",
    alignItems: "center",
    marginTop: 22
  },
  subtleButton: {
    appearance: "none",
    border: 0,
    background: theme.colors.accentSoft,
    color: theme.colors.accentDark,
    padding: [11, 14],
    borderRadius: theme.radii.sm,
    fontWeight: 700,
    cursor: "pointer"
  },
  primaryButton: {
    appearance: "none",
    border: 0,
    background: theme.colors.accent,
    color: "#fff",
    padding: [14, 20],
    borderRadius: theme.radii.sm,
    fontWeight: 800,
    cursor: "pointer"
  },
  error: {
    marginTop: 14,
    color: theme.colors.danger,
    fontWeight: 700
  }
}));

const defaultContact = {
  fullName: "",
  relationship: "",
  phoneNumber: "",
  priority: 1,
  isPrimary: true
};

const defaultState = {
  profileType: "Combined",
  displayName: "",
  dateOfBirth: "",
  gender: "Unknown",
  avatarUrl: "",
  identificationNote: "",
  addressNote: "",
  isPublic: true,
  isActive: true,
  emergencyContacts: [{ ...defaultContact }],
  medicalInfo: {
    bloodType: "",
    chronicDiseases: "",
    allergies: "",
    currentMedications: "",
    emergencyInstructions: ""
  },
  accessSetting: {
    showFullName: true,
    showPhoto: true,
    showMedicalInfo: true,
    showEmergencyContacts: true,
    showAddressNote: false,
    allowScanLogging: true
  }
};

export default function ProfileForm({ onSubmit, isSubmitting }) {
  const classes = useStyles();
  const [form, setForm] = useState(defaultState);
  const [error, setError] = useState("");

  function updateField(field, value) {
    setForm((current) => ({ ...current, [field]: value }));
  }

  function updateMedicalField(field, value) {
    setForm((current) => ({
      ...current,
      medicalInfo: { ...current.medicalInfo, [field]: value }
    }));
  }

  function updateAccessField(field, value) {
    setForm((current) => ({
      ...current,
      accessSetting: { ...current.accessSetting, [field]: value }
    }));
  }

  function updateContact(index, field, value) {
    setForm((current) => ({
      ...current,
      emergencyContacts: current.emergencyContacts.map((contact, contactIndex) =>
        contactIndex === index ? { ...contact, [field]: value } : contact
      )
    }));
  }

  function addContact() {
    setForm((current) => ({
      ...current,
      emergencyContacts: [
        ...current.emergencyContacts,
        {
          ...defaultContact,
          priority: current.emergencyContacts.length + 1,
          isPrimary: false
        }
      ]
    }));
  }

  async function handleSubmit(event) {
    event.preventDefault();
    setError("");

    if (!form.displayName.trim()) {
      setError("Ten hien thi la bat buoc.");
      return;
    }

    if (!form.emergencyContacts[0]?.phoneNumber.trim()) {
      setError("Can it nhat mot so dien thoai lien he khan cap.");
      return;
    }

    try {
      await onSubmit({
        ...form,
        dateOfBirth: form.dateOfBirth || null
      });
      setForm(defaultState);
    } catch (submissionError) {
      setError(submissionError.message);
    }
  }

  return (
    <form className={classes.panel} onSubmit={handleSubmit}>
      <div className={classes.grid}>
        <div className={classes.field}>
          <label className={classes.label}>Loai ho so</label>
          <select className={classes.input} value={form.profileType} onChange={(event) => updateField("profileType", event.target.value)}>
            <option value="Combined">Combined</option>
            <option value="MissingPerson">MissingPerson</option>
            <option value="Medical">Medical</option>
          </select>
        </div>
        <div className={classes.field}>
          <label className={classes.label}>Ten hien thi</label>
          <input className={classes.input} value={form.displayName} onChange={(event) => updateField("displayName", event.target.value)} />
        </div>
        <div className={classes.field}>
          <label className={classes.label}>Ngay sinh</label>
          <input className={classes.input} type="date" value={form.dateOfBirth} onChange={(event) => updateField("dateOfBirth", event.target.value)} />
        </div>
        <div className={classes.field}>
          <label className={classes.label}>Gioi tinh</label>
          <select className={classes.input} value={form.gender} onChange={(event) => updateField("gender", event.target.value)}>
            <option value="Unknown">Unknown</option>
            <option value="Male">Male</option>
            <option value="Female">Female</option>
            <option value="Other">Other</option>
          </select>
        </div>
        <div className={`${classes.field} ${classes.full}`}>
          <label className={classes.label}>Ghi chu nhan dien</label>
          <textarea className={classes.textarea} value={form.identificationNote} onChange={(event) => updateField("identificationNote", event.target.value)} />
        </div>
        <div className={`${classes.field} ${classes.full}`}>
          <label className={classes.label}>Ghi chu dia chi</label>
          <textarea className={classes.textarea} value={form.addressNote} onChange={(event) => updateField("addressNote", event.target.value)} />
        </div>
      </div>

      <div className={classes.sectionTitle}>Lien he khan cap</div>
      <div className={classes.grid}>
        {form.emergencyContacts.map((contact, index) => (
          <div className={classes.field} key={`${contact.phoneNumber}-${index}`}>
            <label className={classes.label}>Lien he #{index + 1}</label>
            <input className={classes.input} placeholder="Ho ten" value={contact.fullName} onChange={(event) => updateContact(index, "fullName", event.target.value)} />
            <input className={classes.input} placeholder="Moi quan he" value={contact.relationship} onChange={(event) => updateContact(index, "relationship", event.target.value)} />
            <input className={classes.input} placeholder="So dien thoai" value={contact.phoneNumber} onChange={(event) => updateContact(index, "phoneNumber", event.target.value)} />
          </div>
        ))}
      </div>

      <div className={classes.rowActions}>
        <button className={classes.subtleButton} onClick={addContact} type="button">
          Them lien he
        </button>
      </div>

      <div className={classes.sectionTitle}>Thong tin y te</div>
      <div className={classes.grid}>
        <div className={classes.field}>
          <label className={classes.label}>Nhom mau</label>
          <input className={classes.input} value={form.medicalInfo.bloodType} onChange={(event) => updateMedicalField("bloodType", event.target.value)} />
        </div>
        <div className={`${classes.field} ${classes.full}`}>
          <label className={classes.label}>Benh nen</label>
          <textarea className={classes.textarea} value={form.medicalInfo.chronicDiseases} onChange={(event) => updateMedicalField("chronicDiseases", event.target.value)} />
        </div>
        <div className={`${classes.field} ${classes.full}`}>
          <label className={classes.label}>Di ung</label>
          <textarea className={classes.textarea} value={form.medicalInfo.allergies} onChange={(event) => updateMedicalField("allergies", event.target.value)} />
        </div>
        <div className={`${classes.field} ${classes.full}`}>
          <label className={classes.label}>Huong dan khan cap</label>
          <textarea className={classes.textarea} value={form.medicalInfo.emergencyInstructions} onChange={(event) => updateMedicalField("emergencyInstructions", event.target.value)} />
        </div>
      </div>

      <div className={classes.sectionTitle}>Quyen hien thi</div>
      <div className={classes.grid}>
        {[
          ["showFullName", "Hien ten"],
          ["showMedicalInfo", "Hien thong tin y te"],
          ["showEmergencyContacts", "Hien lien he khan cap"],
          ["showAddressNote", "Hien ghi chu dia chi"],
          ["allowScanLogging", "Cho phep luu lich su quet"]
        ].map(([field, label]) => (
          <label className={classes.field} key={field}>
            <span className={classes.label}>{label}</span>
            <select className={classes.input} value={String(form.accessSetting[field])} onChange={(event) => updateAccessField(field, event.target.value === "true")}>
              <option value="true">Co</option>
              <option value="false">Khong</option>
            </select>
          </label>
        ))}
      </div>

      {error ? <div className={classes.error}>{error}</div> : null}

      <div className={classes.rowActions}>
        <div />
        <button className={classes.primaryButton} disabled={isSubmitting} type="submit">
          {isSubmitting ? "Dang luu..." : "Tao ho so"}
        </button>
      </div>
    </form>
  );
}
