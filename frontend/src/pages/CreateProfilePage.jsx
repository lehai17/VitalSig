import { useState } from "react";
import { useNavigate } from "react-router-dom";
import AppLayout from "../components/AppLayout";
import ProfileForm from "../components/ProfileForm";
import { createProfile } from "../api/profiles";
import { useAuth } from "../state/AuthContext";

export default function CreateProfilePage() {
  const navigate = useNavigate();
  const { token } = useAuth();
  const [isSubmitting, setIsSubmitting] = useState(false);

  async function handleCreateProfile(payload) {
    setIsSubmitting(true);

    try {
      const profile = await createProfile(payload, token);
      navigate(`/profiles/${profile.id}`);
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <AppLayout title="Tao ho so moi" subtitle="Nhap thong tin nhan dien, lien he khan cap va thong tin y te can thiet de sinh ma QR bao ho.">
      <ProfileForm isSubmitting={isSubmitting} onSubmit={handleCreateProfile} />
    </AppLayout>
  );
}
