import { IPlayerDto } from '../Shared/types/PlayerTypes';
import Result from '../Shared/objects/Result';
import { PLAYER_ENDPOINT } from '../Shared/objects/URL_PATHS';

export const updatePlayer = async (
  dto: IPlayerDto,
  token: string
): Promise<Result<IPlayerDto>> => {
  try {
    const response = await fetch(`${PLAYER_ENDPOINT}/update`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(dto),
    });

    if (response.status >= 400 && response.status < 500) {
      return Result.failure(
        'Something went wrong. Username may already exist.'
      );
    }

    if (response.status === 500) return Result.failure('Internal server error');

    if (!response.ok) {
      const errorMessage = await response.json();
      return Result.failure(errorMessage);
    }

    const data: IPlayerDto = await response.json();
    return Result.ok(data);
  } catch (error) {
    return Result.failure('Something went wrong.');
  }
};

export const updatePlayerImage = async (
  uri: any,
  token: string
): Promise<Result<boolean>> => {
  try {
    const blobResponse = await fetch(uri);
    const blob = await blobResponse.blob();

    const response = await fetch(`${PLAYER_ENDPOINT}/update-image`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      body: blob,
    });

    if (response.status >= 400 && response.status < 500)
      return Result.failure(
        'Cannot update image, you do not have access.' + response.status
      );

    if (response.status === 500) return Result.failure('Internal server error');

    if (!response.ok) {
      const errorMessage = await response.json();
      return Result.failure(errorMessage);
    }

    return Result.ok(true);
  } catch (error) {
    return Result.failure('Something went wrong.');
  }
};
