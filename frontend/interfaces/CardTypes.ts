export interface ICardInputDto {
  name: string;
  image: File;
}

export interface ICard {
  cardId: number;
  galleryID: number;
  name: string;
  url: string;
}
