import { useAuth } from '../context/AuthContext';
import ProfileView from '../components/Profile/ProfileView';
import LoadingSpinner from '../components/Common/LoadingSpinner';

const ProfilePage = () => {
  const { user, isLoading } = useAuth();

  if (isLoading) {
    return <LoadingSpinner message="Loading profile..." />;
  }

  if (!user) {
    return <div>User not found</div>;
  }

  return <ProfileView user={user} />;
};

export default ProfilePage;