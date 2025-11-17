import type { User } from '../../types';
import ProfileInfo from './ProfileInfo';
import ChangePasswordForm from './ChangePasswordForm';

interface ProfileViewProps {
  user: User;
}

const ProfileView = ({ user }: ProfileViewProps) => {
  return (
    <div className="bg-white p-6 rounded-lg shadow-sm">
      <ProfileInfo user={user} />
      <ChangePasswordForm />
    </div>
  );
};

export default ProfileView;