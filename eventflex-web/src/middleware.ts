import { cookies } from "next/headers";
import { NextRequest, NextResponse } from "next/server";
import { decrypt } from "./lib/session";

const protectedRoutes = ["/dashboard", "/users"];
const publicRoutes = ["/login"];

export default async function middleware(req: NextRequest) {
  const path = req.nextUrl.pathname;
  const isProtectedRoute = protectedRoutes.includes(path);
  const isPublicRoute = publicRoutes.includes(path);

  const cookieStore = await cookies();
  const cookie = cookieStore.get("session")?.value;

  // 只有当存在有效的cookie时才尝试解密
  let session = null;
  if (cookie) {
    try {
      session = await decrypt(cookie);
      console.log("middleware decrypted session:", session);
    } catch (error) {
      console.error("Failed to verify session:", error);
      // 清除无效的session cookie
      const response = NextResponse.next();
      response.cookies.delete("session");
      return response;
    }
  }

  // 如果用户未登录且请求的是受保护的路由，则重定向到登录页面
  if (isProtectedRoute && !session?.userId) {
    console.log("middleware redirecting to login page due to invalid or missing session");
    const response = NextResponse.redirect(new URL("/login", req.nextUrl));
    response.cookies.delete("session"); // 确保清除任何无效的session cookie
    return response;
  }

  // 如果用户已登录并且请求的是公共路由（如登录页面），则重定向到仪表板
  if (isPublicRoute && session?.userId) {
    console.log("middleware redirecting to dashboard");
    return NextResponse.redirect(new URL("/dashboard", req.nextUrl));
  }

  return NextResponse.next();
}