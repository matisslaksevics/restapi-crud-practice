import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import { AuthProvider, useAuth } from './context/AuthContext'
import Login from './pages/Login'
import Register from './pages/Register'
import Dashboard from './pages/dashboard'

const ProtectedRoute = ({ children }: { children: React.ReactNode }) => {
  const { user, isLoading } = useAuth()
  
  console.log('ProtectedRoute - user:', user, 'isLoading:', isLoading)
  
  if (isLoading) {
    console.log('ProtectedRoute - showing loading')
    return <div style={{ padding: '20px', textAlign: 'center' }}>Loading...</div>
  }
  
  if (user) {
    console.log('ProtectedRoute - user exists, rendering children')
    return <>{children}</>
  } else {
    console.log('ProtectedRoute - no user, redirecting to login')
    return <Navigate to="/login" />
  }
}

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/" element={
            <ProtectedRoute>
              <Dashboard />
            </ProtectedRoute>
          } />
          <Route path="*" element={<Navigate to="/login" />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  )
}

export default App