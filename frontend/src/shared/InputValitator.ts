const textRegex = /^[A-Za-z]+$/;
const numberRegex = /^[0-9]+$/;
const usernameRegex = /^[A-Za-z0-9]+$/;

export const validText = (input: string): boolean => {
  return textRegex.test(input);
};

export const validNumber = (input: string): boolean => {
  return numberRegex.test(input);
};

export const validUsername = (input: string): boolean => {
  return usernameRegex.test(input);
};
