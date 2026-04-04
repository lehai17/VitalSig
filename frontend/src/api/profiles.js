import { apiRequest } from "./client";

export function getProfiles(token) {
  return apiRequest("/api/profiles", { token });
}

export function getProfileById(profileId, token) {
  return apiRequest(`/api/profiles/${profileId}`, { token });
}

export function createProfile(payload, token) {
  return apiRequest("/api/profiles", {
    method: "POST",
    body: payload,
    token
  });
}

export function getQrCode(profileId, token) {
  return apiRequest(`/api/profiles/${profileId}/qr`, { token });
}

export function regenerateQrCode(profileId, token) {
  return apiRequest(`/api/profiles/${profileId}/qr/regenerate`, {
    method: "POST",
    token
  });
}

export function getScanLogs(profileId, token) {
  return apiRequest(`/api/profiles/${profileId}/scan-logs`, { token });
}
