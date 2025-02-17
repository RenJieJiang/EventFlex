import { z } from "zod";

export const eventTypeSchema = z.object({
  id: z.string().optional(), // Optional for create, required for update
  name: z.string().min(1, "Name is required"),
  description: z.string(),
});