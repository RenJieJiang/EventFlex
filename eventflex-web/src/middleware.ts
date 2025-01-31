import { cookies } from "next/headers";
import { NextRequest, NextResponse } from "next/server";

const protectedRoutes = ["/dashboard", "/users"];
const publicRoutes = ["/login"];

export default async function middleware(req: NextRequest) {
  // const path = req.nextUrl.pathname;
  // const isProtectedRoute = protectedRoutes.some((route) => path.startsWith(route));
  // const isPublicRoute = publicRoutes.some((route) => path.startsWith(route));

  // console.log("Middleware: path:", path, "isProtectedRoute:", isProtectedRoute, "isPublicRoute:", isPublicRoute);
  // // const cookiesStore = await cookies();
  // const token = req.cookies.get("access_token")?.value;
  // console.log("Middleware: access_token:", token);
  // // const token = cookiesStore.get("access_token")?.value;

  // if (isProtectedRoute && !token) {
  //   console.log("Middleware: Redirecting to login due to missing token when accessing: ", path);
  //   return NextResponse.redirect(new URL("/login", req.nextUrl));
  // }

  // if (isPublicRoute && token) {
  //   console.log("Middleware: Redirecting to dashboard (already logged in).");
  //   return NextResponse.redirect(new URL("/dashboard", req.nextUrl));
  // }

  return NextResponse.next();
}

export const config = {
  matcher: ["/((?!_next/static|favicon.ico).*)"], // Ignore static files
};
