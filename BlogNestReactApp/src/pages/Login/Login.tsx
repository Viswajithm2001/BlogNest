// src/pages/Login/Login.tsx
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';

const Login = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();
  const { login } = useAuth();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');

    try {
      const response = await mockLoginApi(username, password);
      login({
        username: response.username,
        profilePictureUrl: response.profilePictureUrl
      });
      navigate('/');
    } catch (err) {
      setError('Invalid username or password');
    }
  };

  const mockLoginApi = async (username: string, password: string) => {
    await new Promise(resolve => setTimeout(resolve, 500));
    if (username && password) {
      return { username, profilePictureUrl: 'https://i.pravatar.cc/40' };
    }
    throw new Error('Invalid credentials');
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-r from-blue-100 to-blue-50 py-12 px-4 sm:px-6 lg:px-8">
      <div className="max-w-md w-full bg-white rounded-xl shadow-lg p-8 space-y-6">
        <div className="text-center">
          <h2 className="text-3xl font-extrabold text-gray-900 mb-2">
            Welcome Back
          </h2>
          <p className="text-sm text-gray-500">
            Sign in to access your BlogNest account
          </p>
        </div>

        <form className="space-y-5" onSubmit={handleSubmit}>
          {error && (
            <div className="rounded-md bg-red-50 p-3">
              <p className="text-sm text-red-700 text-center">{error}</p>
            </div>
          )}

          <div className="space-y-4">
            <div>
              <input
                type="text"
                required
                className="appearance-none block w-full px-4 py-3 border border-gray-300 rounded-lg placeholder-gray-400 text-gray-900 focus:outline-none focus:ring-2 focus:ring-blue-400 focus:border-transparent sm:text-sm"
                placeholder="Username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
              />
            </div>

            <div>
              <input
                type="password"
                required
                className="appearance-none block w-full px-4 py-3 border border-gray-300 rounded-lg placeholder-gray-400 text-gray-900 focus:outline-none focus:ring-2 focus:ring-blue-400 focus:border-transparent sm:text-sm"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </div>
          </div>

          <div>
            <button
              type="submit"
              className="w-full py-3 px-4 bg-blue-600 text-white font-semibold rounded-lg shadow-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-1 focus:ring-blue-500 transition-all"
            >
              Sign In
            </button>
          </div>
        </form>

        <div className="text-center text-sm text-gray-500">
          Don't have an account?{' '}
          <a
            href="/register"
            className="font-medium text-blue-600 hover:text-blue-700"
          >
            Sign Up
          </a>
        </div>
      </div>
    </div>
  );
};

export default Login;
