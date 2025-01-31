import { clsx, type ClassValue } from "clsx";
import { FieldErrors } from "react-hook-form";
import { twMerge } from "tailwind-merge";
import * as https from "https";

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

export function safeStringifyFieldErrors(errors: FieldErrors) {
  const simpleErrors: any = {};
  for (const key in errors) {
    if (errors[key]) {
      simpleErrors[key] = errors[key].message;
    }
  }
  return JSON.stringify(simpleErrors, null, 2);
};

export const isDevelopment = process.env.NEXT_PUBLIC_ENV === 'development';
export const httpsAgent = new https.Agent({
    rejectUnauthorized: !isDevelopment,
  });