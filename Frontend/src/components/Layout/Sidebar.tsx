import { useAuth } from '../../context/AuthContext';
import { useNavigate } from 'react-router-dom';

interface SidebarProps {
  activeView: string;
}

const Sidebar = ({ activeView }: SidebarProps) => {
  const { user } = useAuth();
  const navigate = useNavigate();
  const isAdmin = user?.role === 'Admin';

  const menuItems = [
    { id: 'profile', label: 'My Profile', icon: '👤' },
    { id: 'books-list', label: 'Books List', icon: '📚' },
    { id: 'my-borrows', label: 'My Overdue Books', icon: '📖' },
  ];

  if (isAdmin) {
    menuItems.push(
      { id: 'admin-clients', label: 'Client Management', icon: '👥' },
      { id: 'book-management', label: 'Book Management', icon: '📋' },
      { id: 'admin-borrows', label: 'Overdue Borrows', icon: '🔄' }
    );
  }

  return (
    <div className="w-64 bg-white shadow-lg py-4 min-h-[calc(100vh-80px)]">
      <nav>
        {menuItems.map((item) => (
          <button
            key={item.id}
            onClick={() => navigate(`/${item.id}`)}
            className={`w-full px-6 py-3 flex items-center gap-3 text-sm font-medium transition-colors ${
              activeView === item.id
                ? 'bg-blue-500 text-white'
                : 'text-gray-700 hover:bg-gray-100'
            }`}
          >
            <span className="text-base">{item.icon}</span>
            {item.label}
          </button>
        ))}
      </nav>
    </div>
  );
};

export default Sidebar;