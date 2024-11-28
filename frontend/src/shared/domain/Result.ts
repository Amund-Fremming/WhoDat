export default class Result<T> {
  data: T | null;
  message: string;

  constructor(data: T | null, message: string = "") {
    this.data = data;
    this.message = message;
  }

  public get isError(): boolean {
    return this.message !== "";
  }

  public static ok<T>(data: T): Result<T> {
    return new Result<T>(data, "");
  }

  public static failure<T>(message: string) {
    return new Result<T>(null, message);
  }
}
