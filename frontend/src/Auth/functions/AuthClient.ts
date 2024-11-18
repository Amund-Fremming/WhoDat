import { AUTH_ENDPOINT } from "./URL_PATHS";
import {
  IRegistrationRequest,
  ILoginRequest,
  IAuthResponse,
} from "@/src/Auth/AuthTypes";
import { Result } from "../shared/Result";

export const loginPlayer = async (request: ILoginRequest) : Promise<Result<IAuthResponse>> => {
  try {
    const response = await fetch(`${AUTH_ENDPOINT}/login`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(request),
    });

    if (!response.ok) {
      console.error(response.status, " loginPlayer: response was not 200.")
      return Result.failure("Login failed, password or username is wrong", null);
    }

    const data: IAuthResponse = await response.json();
    return Result.success(data);
  } catch (error) {
    var err = error as Error;
    console.error(error, " loginPlayer: request failed.");
    return Result.failure("Request failed.", err);
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
      console.error(response.status, " registerPlayer: response was not 200.");
      return Result.failure("Something went wrong, try another username.", null);
    }

    const data: IAuthResponse = await response.json();
    return data;
  } catch (error) {
    console.error(error, " registerPlayer: request failed");
    throw new Error("Error while registering a user " + error);
  }
};
