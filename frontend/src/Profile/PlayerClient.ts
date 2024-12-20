import { IPlayerDto } from "../Shared/domain/PlayerTypes";
import Result from "../Shared/domain/Result";
import { PLAYER_ENDPOINT } from "../Shared/domain/URL_PATHS";

export const updatePlayer = async (
  dto: IPlayerDto
): Promise<Result<IPlayerDto>> => {
  try {
    const response = await fetch(`${PLAYER_ENDPOINT}/update`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(dto),
    });

    if (response.status >= 400 && response.status <= 500)
      return Result.failure("Cannot update player, you do not have access.");

    if (response.status === 500) return Result.failure("Internal server error");

    if (!response.ok) {
      console.error(response.status, "updatePlayer: response was not 200.");
      const errorMessage = await response.json();
      return Result.failure(errorMessage);
    }

    const data: IPlayerDto = await response.json();
    return Result.ok(data);
  } catch (error) {
    console.error(error, "UpdatePlayer");
    console.log(error, "UpdatePlayer");
    return Result.failure("Something went wrong.");
  }
};

export const updatePlayerImage = async (uri: any): Promise<Result<boolean>> => {
  try {
    const blobResponse = await fetch(uri);
    const blob = await blobResponse.blob();

    const response = await fetch(`${PLAYER_ENDPOINT}/update-image`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: blob,
    });

    if (response.status >= 400 && response.status <= 500)
      return Result.failure("Cannot update image, you do not have access.");

    if (response.status === 500) return Result.failure("Internal server error");

    if (!response.ok) {
      console.error(
        response.status,
        "updatePlayerImage: response was not 200."
      );
      const errorMessage = await response.json();
      return Result.failure(errorMessage);
    }

    return Result.ok(true);
  } catch (error) {
    console.error(error, "UpdatePlayerImage");
    console.log(error, "UpdatePlayerImage");
    return Result.failure("Something went wrong.");
  }
};
