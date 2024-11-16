import { CARD_ENDPOINT } from "./URL_PATHS";
import { ICard } from "@/src/domain/CardTypes";

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

    const data: ICard[] = await response.json();
    return data;
  } catch (error) {
    console.error("Error while fetching all cards " + error);
    throw new Error("Error while fetching all cards " + error);
  }
};

export const addCard = async (uri: string, name: string, token: string) => {
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

    if (!response.ok) {
      throw new Error("Error in addCardToGallery response " + response.status);
    }
  } catch (error) {
    console.error("Error when adding a card " + error);
    throw new Error("Error when adding a card " + error);
  }
};

export const deleteCard = async (cardId: number, token: string) => {
  console.log(`${CARD_ENDPOINT}/delete/${cardId}`);
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
    console.error("Error delete card " + error);
    throw new Error("Error delete card " + error);
  }
};
