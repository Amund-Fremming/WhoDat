import { AUTH_ENDPOINT } from "@/src/Shared/domain/URL_PATHS";
import {
  IRegistrationRequest,
  ILoginRequest,
  IAuthResponse,
} from "@/src/Auth/AuthTypes";
import Result from "../Shared/domain/Result";

export const loginPlayer = async (
  request: ILoginRequest
): Promise<Result<IAuthResponse>> => {
  try {
    const response = await fetch(`${AUTH_ENDPOINT}/login`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(request),
    });

    if (!response.ok) {
      console.error(response.status, " loginPlayer: response was not 200.");
      const errorMessage: string = await response.json();
      // REMOVE THIS
      console.error("Error message: " + errorMessage);
      return Result.failure(errorMessage);
    }

    const data: IAuthResponse = await response.json();
    return Result.ok(data);
  } catch (error) {
    console.error(error, " loginPlayer: request failed.");
    return Result.failure("Something went wrong.");
  }
};

export const registerPlayer = async (
  request: IRegistrationRequest
): Promise<Result<IAuthResponse>> => {
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
      const errorMessage = await response.json();
      return Result.failure(errorMessage);
    }

    const data: IAuthResponse = await response.json();
    return Result.ok(data);
  } catch (error) {
    console.error(error, " registerPlayer: request failed");
    return Result.failure("Something went wrong.");
  }
};
