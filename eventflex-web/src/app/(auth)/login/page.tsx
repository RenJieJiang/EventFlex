'use client';

import { useActionState, useState } from 'react';
import { useFormStatus } from 'react-dom';
import { login } from './actions';
import { useRouter } from 'next/navigation';

// 定义初始状态
const initialState = { errors: { email: [], password: [] }};

function SubmitButton() {
    const { pending } = useFormStatus();

    return (
        <button type="submit" className="w-full bg-blue-600 text-white p-2 rounded hover:bg-blue-700 disabled:bg-gray-400">
            {pending ? 'Submitting...' : 'Login'}
        </button>
    );
}

export default function LoginPage() {
  const [state, loginAction, pending] = useActionState(login, initialState);

  return (
      <div className="flex items-center justify-center min-h-screen bg-gray-100">
          <div className="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4 w-full max-w-sm">
              <h2 className="text-2xl font-bold mb-6 text-center">Login</h2>
              <form action={loginAction}>
                  <div className="mb-4">
                      <label htmlFor="email" className="block text-gray-700 text-sm font-bold mb-2">Email</label>
                      <input id="email" name="email" type="email" required 
                          className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                      />
                  </div>
                  <div className="mb-6">
                      <label htmlFor="password" className="block text-gray-700 text-sm font-bold mb-2">Password</label>
                      <input id="password" name="password" type="password" required 
                          className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 mb-3 leading-tight focus:outline-none focus:shadow-outline"
                      />
                  </div>
                  {state?.errors && (
                      <div className="mb-4 text-red-500">
                          {Object.entries(state.errors).map(([field, errors]) =>
                              errors.map((error, index) => (
                                  <p key={index}>{field}: {error}</p>
                              ))
                          )}
                      </div>
                  )}
                  <SubmitButton />
              </form>
              <p className="text-center text-gray-500 text-xs mt-4">
                  &copy;{new Date().getFullYear()} EventFlex. All rights reserved.
              </p>
              {/* <div><pre>{JSON.stringify(state, null, 2)}</pre></div> */}
          </div>
      </div>
  );
}