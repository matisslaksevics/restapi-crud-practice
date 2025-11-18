import { useState } from 'react';
import { useAuth } from '../context/AuthContext';
import MainLayout from '../components/Layout/MainLayout';
import ProfilePage from './Profile';
import ClientManagement from './ClientManagement';
import LoadingSpinner from '../components/Common/LoadingSpinner';
import Unauthorized from './Unauthorized';

const Dashboard = () => {
  const { user, isLoading } = useAuth();
  const [activeView, setActiveView] = useState('profile');

  const renderActiveView = () => {
    if (isLoading) {
      return <LoadingSpinner message="Loading..." />;
    }

    if (!user) {
      return <div>User not found</div>;
    }

    if (activeView === 'admin-clients' && user.role !== 'Admin') {
      return <Unauthorized />;
    }

    switch (activeView) {
      case 'profile':
        return <ProfilePage />;
      case 'admin-clients':
        return <ClientManagement />;
      default:
        return <ProfilePage />;
    }
  };

  if (!user && !isLoading) {
    return null;
  }

  return (
    <MainLayout activeView={activeView} onViewChange={setActiveView}>
      {renderActiveView()}
    </MainLayout>
  );
};

export default Dashboard;