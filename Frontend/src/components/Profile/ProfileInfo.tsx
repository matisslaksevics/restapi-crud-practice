import type { User } from '../../types';

interface ProfileInfoProps {
  user: User;
}

const ProfileInfo = ({ user }: ProfileInfoProps) => {
  return (
    <div>
      <h3 className="text-xl font-bold mb-4">
        User Profile
      </h3>
      <div className="space-y-2">
        <p><strong>Username:</strong> {user.username}</p>
        <p><strong>Role:</strong> {user.role}</p>
        <p><strong>User ID:</strong> {user.id}</p>
      </div>
    </div>
  );
};

export default ProfileInfo;