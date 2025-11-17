import { useState, useEffect } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

const Login = () => {
  const [username, setUsername] = useState('') 
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const { user, login, isLoading } = useAuth()
  const navigate = useNavigate()

  useEffect(() => {
    if (user) {
      console.log('User already authenticated, redirecting to dashboard')
      navigate('/', { replace: true })
    }
  }, [user, navigate])

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError('')
    
    try {
      await login(username, password)
      console.log('Login successful, redirecting to dashboard')
      navigate('/', { replace: true })
    } catch (error: any) {
      console.log('Login error:', error)
      const errorMessage = error.response?.data || 
        error.response?.statusText || 
        error.message || 
        'Login failed!'
      setError(`Login failed: ${errorMessage}`)
    }
  }

  return (
    <div className="min-h-screen bg-gray-100 flex items-center justify-center">
      <div className="bg-white p-8 rounded-lg shadow-md w-full max-w-md">
        <h1 className="text-2xl font-bold mb-6 text-center">Login</h1>
        
        {error && (
          <div className="bg-red-100 text-red-700 p-3 rounded mb-4">
            {error}
          </div>
        )}
        
        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <input
              type="text"  
              placeholder="Username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              required
            />
          </div>
          
          <div className="mb-6">
            <input
              type="password"
              placeholder="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              required
            />
          </div>
          
          <button
            type="submit"
            disabled={isLoading}
            className={`w-full text-white py-2 px-4 rounded-md font-medium transition-colors ${
              isLoading 
                ? 'bg-gray-400 cursor-not-allowed' 
                : 'bg-blue-500 hover:bg-blue-600'
            }`}
          >
            {isLoading ? 'Logging in...' : 'Login'}
          </button>
        </form>
        
        <div className="mt-6 text-center p-4 bg-green-50 rounded-md">
          <p>Don't have an account? 
            <Link to="/register" className="text-green-500 font-bold hover:text-green-600 ml-1">
              Register here
            </Link>
          </p>
        </div>
      </div>
    </div>
  )
}

export default Login