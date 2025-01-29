import { z } from "zod";

export const userSchema = z.object({
  id: z.string().uuid().optional(), // Optional for create, required for update
  userName: z.string().min(1, "Name is required"),
  email: z.string().min(1,"Email is required").email("Invalid email address"),
  phoneNumber: z.string().min(1, "Phone number is required"),
  tenantId: z.string().nullable().optional(),
});