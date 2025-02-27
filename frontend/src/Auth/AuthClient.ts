import { AUTH_ENDPOINT } from '@/src/Shared/assets/constants/URL_PATHS';
import {
  IRegistrationRequest,
  ILoginRequest,
  IAuthResponse,
} from '@/src/Auth/AuthTypes';
import Result from '../Shared/objects/Result';

export const loginPlayer = async (
  request: ILoginRequest
): Promise<Result<IAuthResponse>> => {
  try {
    const response = await fetch(`${AUTH_ENDPOINT}/login`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(request),
    });

    if (response.status >= 400 && response.status <= 500) {
      return Result.failure('Invalid login, username or password was wrong.');
    }

    if (response.status === 500) return Result.failure('Internal server error');

    if (!response.ok) {
      const errorMessage: string = await response.json();
      return Result.failure(errorMessage);
    }

    const data: IAuthResponse = await response.json();
    return Result.ok(data);
  } catch (error) {
    return Result.failure('Something went wrong.');
  }
};

export const registerPlayer = async (
  request: IRegistrationRequest
): Promise<Result<IAuthResponse>> => {
  try {
    const response = await fetch(`${AUTH_ENDPOINT}/register`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(request),
    });

    if (response.status === 500) return Result.failure('Internal server error');

    if (response.status >= 400 && response.status <= 500) {
      return Result.failure(await response.text());
    }

    if (!response.ok) {
      const errorMessage = await response.json();
      return Result.failure(errorMessage);
    }

    const data: IAuthResponse = await response.json();
    return Result.ok(data);
  } catch (error) {
    return Result.failure('Something went wrong.');
  }
};
