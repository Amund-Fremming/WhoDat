import { CARD_ENDPOINT } from "./URL_PATHS";
import { ICard, ICardInputDto } from "@/interfaces/CardTypes";

export const getAllCards = async (token: string) => {
  try {
    const response = await fetch(`${CARD_ENDPOINT}/getall`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });

    if (!response.ok) {
      throw new Error(
        "Error in getCardsFromGallery response " + response.status
      );
    }

    const data: Array<ICard> = await response.json();
    return data;
  } catch (error) {
    console.error("Error while fetching all cards " + error);
    throw new Error("Error while fetching all cards " + error);
  }
};

export const addCard = async (blob: Blob, token: string) => {
  console.log(`${CARD_ENDPOINT}/add`);
  try {
    const response = await fetch(`${CARD_ENDPOINT}/add`, {
      method: "POST",
      headers: {
        Authorization: `Bearer ${token}`,
      },
      body: blob,
    });

    if (!response.ok) {
      throw new Error("Error in addCardToGallery response " + response.status);
    }
  } catch (error) {
    console.error("Error when adding a card " + error);
    throw new Error("Error when adding a card " + error);
  }
};

export const deleteCard = async (cardId: number, token: string) => {
  try {
    const response = await fetch(`${CARD_ENDPOINT}/delete/${cardId}`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });

    if (!response.ok) {
      throw new Error(
        "Error in deleteCardFromGallery response " + response.status
      );
    }
  } catch (error) {
    console.error("Error x" + error);
    throw new Error("Error x" + error);
  }
};
