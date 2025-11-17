import { createContext, useContext, useState, useEffect } from 'react'
import type { User, AuthResponse, UserProfileDto } from '../types'
import api from '../services/api'

interface AuthContextType {
  user: User | null
  login: (username: string, password: string) => Promise<void>
  register: (username: string, password: string) => Promise<void>
  logout: () => void
  isLoading: boolean
}

const AuthContext = createContext<AuthContextType | undefined>(undefined)

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = useState<User | null>(null)
  const [isLoading, setIsLoading] = useState(true)

  useEffect(() => {
    checkAuth()
  }, [])

  const checkAuth = async () => {
    const accessToken = localStorage.getItem('accessToken')
    
    console.log('checkAuth called, accessToken exists:', !!accessToken)
    
    if (!accessToken) {
      console.log('No access token found, setting isLoading to false')
      setIsLoading(false)
      return
    }

    try {
      console.log('Checking authentication with existing token...')
      await fetchUser()
    } catch (error) {
      console.error('Auto-login failed:', error)
      console.log('Clearing tokens due to auto-login failure')
      clearTokens()
      setUser(null)
    } finally {
      console.log('Auth check completed, setting isLoading to false')
      setIsLoading(false)
    }
  }

  const fetchUser = async (): Promise<void> => {
    try {
      console.log('Fetching user profile...')
      const response = await api.get<UserProfileDto>('/auth/profile')
      const userData = response.data;
      
      console.log('User profile response:', userData)
    
      if (!userData.username) {
        console.error('No username in profile response!')
        throw new Error('Invalid user data received')
      }
  
      const userObj: User = {
        id: userData.id || 'unknown-id',
        username: userData.username,
        role: userData.role || 'User'
      }
      
      console.log('User object created:', userObj)
      setUser(userObj)
    } catch (error: any) {
      console.error('Failed to fetch user profile:', error)
      console.error('Error details:', error.response?.data || error.message)
      throw error
    }
  }

  const login = async (username: string, password: string) => {
    try {
      console.log('Attempting login...')
      const response = await api.post<AuthResponse>('/auth/login', { 
        username,
        password 
      })
      
      const { accessToken, refreshToken } = response.data
      
      console.log('Login successful, tokens received')
      localStorage.setItem('accessToken', accessToken)
      localStorage.setItem('refreshToken', refreshToken)
      
      await fetchUser()
    } catch (error) {
      console.error('Login failed:', error)
      clearTokens()
      throw error
    }
  }

  const register = async (username: string, password: string) => {
    try {
      console.log('Attempting registration...')
      const response = await api.post('/auth/register', {
        username,
        password
      })
    
      console.log('Registration successful:', response.data)
    } catch (error) {
      console.error('Registration failed:', error)
      throw error
    }
  }

  const logout = async () => {
    try {
      console.log('Logging out...')
      await api.post('/auth/signout')
    } catch (error) {
      console.error('Logout error:', error)
    } finally {
      console.log('Clearing tokens on logout')
      clearTokens()
      setUser(null)
    }
  }

  const clearTokens = () => {
    localStorage.removeItem('accessToken')
    localStorage.removeItem('refreshToken')
  }

  return (
    <AuthContext.Provider value={{ user, login, register, logout, isLoading }}>
      {children}
    </AuthContext.Provider>
  )
}

export const useAuth = () => {
  const context = useContext(AuthContext)
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider')
  }
  return context
}