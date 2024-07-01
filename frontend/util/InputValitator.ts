const textRegex = /^[A-Za-z]+$/;
const numberRegex = /^[0-9]+$/;

export const validText = (input: string): boolean => {
  return textRegex.test(input);
};

export const validNumber = (input: string): boolean => {
  return numberRegex.test(input);
};
