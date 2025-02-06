import axios from "axios";

const API_URL = "http://localhost:3001/Inventory/Incoming"; // URL change to aws later

export const fetchIncomingInventory = async (currentPantry) => {
  return await axios.get(API_URL, { params: { pantryID: currentPantry } });
};
