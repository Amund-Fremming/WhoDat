import { CARD_ENDPOINT } from "../domain/URL_PATHS";
import { ICard } from "@/src/Shared/domain/CardTypes";
import Result from "../domain/Result";

export const getAllCards = async (
  token: string
): Promise<Result<Array<ICard>>> => {
  try {
    const response = await fetch(`${CARD_ENDPOINT}/getall`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });

    if (response.status >= 400 && response.status <= 500)
      return Result.failure("Invalid login, username or password was wrong.");

    if (response.status === 500) return Result.failure("Internal server error");

    if (!response.ok) {
      console.error("getAllCards: response was not 200.");
      const errorMessage = await response.json();
      return Result.failure(errorMessage);
    }

    const data: ICard[] = await response.json();
    return Result.ok(data);
  } catch (error) {
    console.error("(getAllCards)" + error);
    return Result.failure("Something went wrong.");
  }
};

export const addCard = async (
  uri: string,
  name: string,
  token: string
): Promise<Result<boolean>> => {
  try {
    const blobResponse = await fetch(uri);
    const blob = await blobResponse.blob();

    const response = await fetch(`${CARD_ENDPOINT}/add`, {
      method: "POST",
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "image/jpeg",
        "X-Card-Name": name,
      },
      body: blob,
    });

    if (response.status >= 400 && response.status <= 500)
      return Result.failure("Invalid login, username or password was wrong.");

    if (response.status === 500) return Result.failure("Internal server error");

    if (!response.ok) {
      console.error("addCard: response was not 200.");
      const errorMessage = await response.json();
      return Result.failure(errorMessage);
    }

    return Result.ok(true);
  } catch (error) {
    console.error("(addCard)" + error);
    return Result.failure("Something went wrong.");
  }
};

export const deleteCard = async (
  cardId: number,
  token: string
): Promise<Result<boolean>> => {
  console.log(`${CARD_ENDPOINT}/delete/${cardId}`);
  try {
    throw new Error("dummy error");
    const response = await fetch(`${CARD_ENDPOINT}/delete/${cardId}`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });

    if (response.status >= 400 && response.status <= 500)
      return Result.failure("Invalid login, username or password was wrong.");

    if (response.status === 500) return Result.failure("Internal server error");

    if (!response.ok) {
      throw new Error(
        "Error in deleteCardFromGallery response " + response.status
      );
    }

    return Result.ok(true);
  } catch (error) {
    console.error("(deleteCard)" + error);
    return Result.failure("Something went wrong.");
  }
};
