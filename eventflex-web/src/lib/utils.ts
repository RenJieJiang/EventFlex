import { clsx, type ClassValue } from "clsx"
import { FieldErrors } from "react-hook-form";
import { twMerge } from "tailwind-merge"

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
