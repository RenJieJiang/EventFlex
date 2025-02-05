import { jwtVerify } from "jose";
import { NextRequest, NextResponse } from "next/server";
import { cookies } from "next/headers";

const protectedRoutes = ["/dashboard", "/users"];
const publicRoutes = ["/login"];

export default async function middleware(req: NextRequest) {
  const path = req.nextUrl.pathname;
  const isProtectedRoute = protectedRoutes.some((route) => path.startsWith(route));
  const isPublicRoute = publicRoutes.some((route) => path.startsWith(route));
  const cookieStore = await cookies();

  console.log("Middleware: path:", path, "isProtectedRoute:", isProtectedRoute, "isPublicRoute:", isPublicRoute);

  const token = req.cookies.get("access_token")?.value;
  console.log("Middleware: token:", token);

  // check if token is expired
  if (token) {
    try {
      console.log("secretKey:", process.env.JWT_SECRET);
      const secretKey = new TextEncoder().encode(process.env.JWT_SECRET);
      await jwtVerify(token, secretKey);
    } catch (error) {
      console.error("Token verification failed:", error);

      // Clear cookies and redirect to login
      cookieStore.set("access_token", "", { maxAge: -1 });
      return NextResponse.redirect(new URL("/login", req.nextUrl));
    }
  }


  if (isProtectedRoute && !token) {
    console.log("Middleware: Redirecting to login due to missing token when accessing:", path);
    return NextResponse.redirect(new URL("/login", req.nextUrl));
  }

  if (isPublicRoute && token) {
    console.log("Middleware: Redirecting to dashboard (already logged in).");
    return NextResponse.redirect(new URL("/dashboard", req.nextUrl));
  }

  return NextResponse.next();
}

export const config = {
  matcher: ["/((?!_next/static|_next/image|favicon.ico).*)"], // Ignore static files
};