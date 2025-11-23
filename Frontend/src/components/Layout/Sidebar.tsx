import { useAuth } from '../../context/AuthContext';

interface SidebarProps {
  activeView: string;
  onViewChange: (view: string) => void;
}

const Sidebar = ({ activeView, onViewChange }: SidebarProps) => {
  const { user } = useAuth();
  const isAdmin = user?.role === 'Admin';

  const menuItems = [
    { id: 'profile', label: 'My Profile', icon: '👤' },
  ];

  menuItems.push({ id: 'books-list', label: 'Books List', icon: '📚' });

  if (isAdmin) {
    menuItems.push(
      { id: 'admin-clients', label: 'Client Management', icon: '👥' },
      { id: 'book-management', label: 'Book Management', icon: '📖' }
    );
  }

  return (
    <div className="w-64 bg-white shadow-lg py-4 min-h-[calc(100vh-80px)]">
      <nav>
        {menuItems.map((item) => (
          <button
            key={item.id}
            onClick={() => onViewChange(item.id)}
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