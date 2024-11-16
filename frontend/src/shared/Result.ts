class Result<T> {
    data: T | null;
    message: string;
    error: Error | null;

    constructor(data: T | null, message: string = "", error: Error | null) {
        this.data = data;
        this.message = message;
        this.error = error;
    }

    public get isSuccess(): boolean {
        return this.data !== null && this.error === null;
    }

    public static success<T>(data: T) : Result<T> {
        return new Result<T>(data, "", null);
    }

    public static failure<T>(message: string, error: Error)
    {
        return new Result<T>(null, message, error);
    }
}