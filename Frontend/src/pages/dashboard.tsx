import { useAuth } from '../context/AuthContext';
import MainLayout from '../components/Layout/MainLayout';
import ProfilePage from './Profile';
import ClientManagement from './ClientManagement';
import BookManagement from './BookManagement';
import BooksList from './BooksList';
import UserBorrows from './UserBorrows';
import BorrowManagement from './BorrowManagement';
import { useParams, Navigate } from 'react-router-dom';

interface DashboardProps {
  view: string;
}

const Dashboard = ({ view }: DashboardProps) => {
  const { user } = useAuth();
  
  if (!user) {
    return <Navigate to="/login" replace />;
  }
  
  const isAdmin = user.role === 'Admin';
  
  // Check authorization for admin-only views
  if ((view === 'admin-clients' || view === 'book-management' || view === 'admin-borrows') && !isAdmin) {
    return <Navigate to="/profile" replace />;
  }

  const renderActiveView = () => {
    switch (view) {
      case 'profile':
        return <ProfilePage />;
      case 'admin-clients':
        return <ClientManagement />;
      case 'book-management':
        return <BookManagement />;
      case 'books-list':
        return <BooksList />;
      case 'my-borrows':
        return <UserBorrows />;
      case 'admin-borrows':
        return <BorrowManagement />;
      default:
        return <Navigate to="/profile" replace />;
    }
  };

  return (
    <MainLayout activeView={view}>
      {renderActiveView()}
    </MainLayout>
  );
};

export default Dashboard;