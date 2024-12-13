import { IPlayerDto } from "../Shared/domain/PlayerTypes";
import Result from "../Shared/domain/Result";
import { PLAYER_ENDPOINT } from "../Shared/domain/URL_PATHS";

export const updatePlayer = async (
  dto: IPlayerDto
): Promise<Result<boolean>> => {
  try {
    const response = await fetch(`${PLAYER_ENDPOINT}/players/update`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(dto),
    });

    console.log(`${PLAYER_ENDPOINT}/players/update`);

    if (response.status >= 400 && response.status <= 500)
      return Result.failure("Invalid login, username or password was wrong.");

    if (response.status === 500) return Result.failure("Internal server error");

    if (!response.ok) {
      console.error(response.status, " registerPlayer: response was not 200.");
      const errorMessage = await response.json();
      return Result.failure(errorMessage);
    }

    return Result.ok(true);
  } catch (error) {
    console.error(error, "UpdatePlayer");
    console.log(error, "UpdatePlayer");
    return Result.failure("Something went wrong.");
  }
};
