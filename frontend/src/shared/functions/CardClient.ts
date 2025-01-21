import { CARD_ENDPOINT } from '../assets/constants/URL_PATHS';
import { ICardDto } from '@/src/Shared/types/CardTypes';
import Result from '../objects/Result';

export const getAllCards = async (
  token: string
): Promise<Result<Array<ICardDto>>> => {
  try {
    const response = await fetch(`${CARD_ENDPOINT}/getall`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
    });

    if (response.status >= 400 && response.status <= 500)
      return Result.failure('Invalid login, username or password was wrong.');

    if (response.status === 500) return Result.failure('Internal server error');

    if (!response.ok) {
      const errorMessage = await response.json();
      return Result.failure(errorMessage);
    }

    const data: ICardDto[] = await response.json();
    return Result.ok(data);
  } catch (error) {
    return Result.failure('Something went wrong.');
  }
};

export const addCard = async (
  uri: string,
  name: string,
  token: string
): Promise<Result<ICardDto>> => {
  try {
    const blobResponse = await fetch(uri);
    const blob = await blobResponse.blob();

    const response = await fetch(`${CARD_ENDPOINT}/add`, {
      method: 'POST',
      headers: {
        Authorization: `Bearer ${token}`,
        'Content-Type': 'image/jpeg',
        'X-Card-Name': name,
      },
      body: blob,
    });

    if (response.status >= 400 && response.status <= 500)
      return Result.failure('Invalid login, username or password was wrong.');

    if (response.status === 500) return Result.failure('Internal server error');

    if (!response.ok) {
      const errorMessage = await response.json();
      return Result.failure(errorMessage);
    }

    const data: ICardDto = await response.json();
    return Result.ok(data);
  } catch (error) {
    return Result.failure('Something went wrong.');
  }
};

export const deleteCard = async (
  cardId: number,
  token: string
): Promise<Result<boolean>> => {
  try {
    const response = await fetch(`${CARD_ENDPOINT}/delete/${cardId}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
    });

    if (response.status >= 400 && response.status <= 500)
      return Result.failure('Invalid login, username or password was wrong.');

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
