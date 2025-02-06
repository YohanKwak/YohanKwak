// src/services/profileService.js
import axios from 'axios';

const API_URL = 'http://localhost:3001/Profile';

export const saveProfile = async (profileData) => {
  return await axios.post(API_URL, profileData);
};

export const getProfileById = async (profileID) => {
  return await axios.get(`${API_URL}/${profileID}`);
};

export const getAllProfiles = async () => {
  return await axios.get(API_URL);
};

export const updateProfile = async (profileID, profileData) => {
  return await axios.put(`${API_URL}/${profileID}`, profileData);
};

export const deleteProfile = async (profileID) => {
  return await axios.delete(`${API_URL}/${profileID}`);
};
