import { AUTH_ENDPOINT } from "./URL_PATHS";
import {
  IRegistrationRequest,
  ILoginRequest,
  IAuthResponse,
} from "@/src/domain/AuthTypes";

export const loginPlayer = async (request: ILoginRequest) => {
  try {
    const response = await fetch(`${AUTH_ENDPOINT}/login`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(request),
    });

    if (!response.ok) {
      throw new Error("Error in login response " + response.status);
    }

    const data: IAuthResponse = await response.json();
    console.log("auth response :" + data.playerID);
    return data;
  } catch (error) {
    console.error("Error while logging in a user " + error);
    throw new Error("Error while logging in a user " + error);
  }
};

export const registerPlayer = async (request: IRegistrationRequest) => {
  try {
    const response = await fetch(`${AUTH_ENDPOINT}/register`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(request),
    });

    if (!response.ok) {
      throw new Error("Error in register response " + response.status);
    }

    const data: IAuthResponse = await response.json();
    return data;
  } catch (error) {
    console.error("Error while registering a user " + error);
    throw new Error("Error while registering a user " + error);
  }
};
