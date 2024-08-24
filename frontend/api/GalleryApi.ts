import { ICard, ICardInputDto, IGallery } from "@/interfaces/GalleryTypes";
import { GALLERY_ENDPOINT } from "./URL_PATHS";

export const getCardsFromGallery = async (galleryId: number) => {
  try {
    const response = await fetch(
      `${GALLERY_ENDPOINT}/galleries/${galleryId}/cards`,
      {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
      }
    );

    if (!response.ok) {
      throw new Error(
        "Error in getCardsFromGallery response " + response.status
      );
    }

    const data: Array<ICard> = await response.json();
    return data;
  } catch (error) {
    console.error("Error while fetching player gallery cards " + error);
    throw new Error("Error while fetching player gallery cards " + error);
  }
};

export const addCardToGallery = async (
  galleryId: number,
  cardInputDto: ICardInputDto,
  token: string
) => {
  try {
    const response = await fetch(
      `${GALLERY_ENDPOINT}/galleries/${galleryId}/cards`,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(cardInputDto),
      }
    );

    if (!response.ok) {
      throw new Error("Error in addCardToGallery response " + response.status);
    }
  } catch (error) {
    console.error("Error when adding a card to the gallery " + error);
    throw new Error("Error when adding a card to the gallery " + error);
  }
};

export const deleteCardFromGallery = async (
  galleryId: number,
  cardId: number
) => {
  try {
    const response = await fetch(
      `${GALLERY_ENDPOINT}/galleries/${galleryId}/cards/${cardId}`,
      {
        method: "DELETE",
        headers: {
          "Content-Type": "application/json",
        },
      }
    );

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

export const createGallery = async (gallery: IGallery) => {
  try {
    const response = await fetch(`${GALLERY_ENDPOINT}/galleries`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(gallery),
    });

    if (!response.ok) {
      throw new Error("Error in x response " + response.status);
    }
  } catch (error) {
    console.error("Error when creating a Gallery " + error);
    throw new Error("Error when creating a Gallery " + error);
  }
};

export const deleteGallery = async (galleryId: number) => {
  try {
    const response = await fetch(`${GALLERY_ENDPOINT}/galleries/${galleryId}`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
      },
    });

    if (!response.ok) {
      throw new Error("Error in deleteGallery response " + response.status);
    }
  } catch (error) {
    console.error("Error when deleting a Gallery " + error);
    throw new Error("Error when deleting a Gallery " + error);
  }
};

export const GetAllPlayerCards = async (token: string) => {
  console.log(
    "http://localhost:5158/api/Player/galleries/allcards" ===
      `${GALLERY_ENDPOINT}/galleries/allcards`
  );

  try {
    const response = await fetch(`${GALLERY_ENDPOINT}/galleries/allcards`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });

    if (!response.ok) {
      throw new Error("Error in getAllPlayerCards response " + response.status);
    }

    const data: Array<ICard> = await response.json();
    return data;
  } catch (error) {
    console.error("Error when getting all cards " + error);
    throw new Error("Error when getting all cards " + error);
  }
};
