import { AUTH_ENDPOINT } from "./URL_PATHS";
import { IRegistrationRequest, IAuthResponse } from "@/interfaces/AuthTypes";

export const registerPlayer = async (request: IRegistrationRequest) => {
  console.log(
    `${AUTH_ENDPOINT}/register` === "http://localhost:5158/api/auth/register"
  );
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
