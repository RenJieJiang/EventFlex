import { cookies } from 'next/headers';

export async function POST() {

  console.log('POST /logout');

  const cookieStore = await cookies();
  cookieStore.set("access_token", "", { maxAge: -1, path: "/" });
  cookieStore.set("refresh_token", "", { maxAge: -1, path: "/" });
  cookieStore.set("id", "", { maxAge: -1, path: "/" });
  cookieStore.set("userName", "", { maxAge: -1, path: "/" });
  cookieStore.set("email", "", { maxAge: -1, path: "/" });

  return new Response(JSON.stringify({ success: true }), { status: 200 });
}