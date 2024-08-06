import { AUTH_ENDPOINT } from "./URL_PATHS";
import { IRegistrationRequest, IAuthResponse } from "@/interfaces/AuthTypes";

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
    throw new Error("Error while registering a user " + error);
  }
};
