import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import { AuthProvider, useAuth } from './context/AuthContext'
import Login from './pages/Login'
import Register from './pages/Register'
import Dashboard from './pages/dashboard'
import LoadingSpinner from './components/Common/LoadingSpinner'

const ProtectedRoute = ({ children }: { children: React.ReactNode }) => {
  const { user, isLoading } = useAuth()
  
  if (isLoading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <LoadingSpinner message="Loading..." />
      </div>
    )
  }
  
  return user ? <>{children}</> : <Navigate to="/login" replace />
}

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/profile" element={
            <ProtectedRoute>
              <Dashboard view="profile" />
            </ProtectedRoute>
          } />
          
          <Route path="/books-list" element={
            <ProtectedRoute>
              <Dashboard view="books-list" />
            </ProtectedRoute>
          } />
          
          <Route path="/my-borrows" element={
            <ProtectedRoute>
              <Dashboard view="my-borrows" />
            </ProtectedRoute>
          } />
          
          <Route path="/admin-clients" element={
            <ProtectedRoute>
              <Dashboard view="admin-clients" />
            </ProtectedRoute>
          } />
          
          <Route path="/book-management" element={
            <ProtectedRoute>
              <Dashboard view="book-management" />
            </ProtectedRoute>
          } />
          
          <Route path="/admin-borrows" element={
            <ProtectedRoute>
              <Dashboard view="admin-borrows" />
            </ProtectedRoute>
          } />
          
          <Route path="/" element={<Navigate to="/profile" replace />} />
          <Route path="/dashboard/:view" element={<Navigate to="/:view" replace />} />
          <Route path="*" element={<Navigate to="/login" replace />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  )
}

export default App