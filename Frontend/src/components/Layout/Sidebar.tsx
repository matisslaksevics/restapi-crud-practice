import type { User } from '../../types/index.ts';

interface SidebarProps {
  activeView: string;
  onViewChange: (view: string) => void;
  user: User | null;
}

const Sidebar = ({ activeView, onViewChange, user }: SidebarProps) => {
  const isAdmin = user?.role === 'Admin';

  const userMenuItems = [
    { id: 'profile', label: 'My Profile', icon: '👤' },
  ];

  const adminMenuItems = isAdmin ? [
    { id: 'admin-clients', label: 'Client Management', icon: '👥' },
  ] : [];

  const allMenuItems = [...userMenuItems, ...adminMenuItems];

  return (
    <div style={{
      width: '250px',
      backgroundColor: 'white',
      boxShadow: '2px 0 5px rgba(0, 0, 0, 0.1)',
      padding: '1rem 0',
      minHeight: 'calc(100vh - 80px)'
    }}>
      <nav>
        {allMenuItems.map((item) => (
          <button
            key={item.id}
            onClick={() => onViewChange(item.id)}
            style={{
              width: '100%',
              padding: '0.75rem 1.5rem',
              border: 'none',
              backgroundColor: activeView === item.id ? '#3b82f6' : 'transparent',
              color: activeView === item.id ? 'white' : '#374151',
              textAlign: 'left',
              cursor: 'pointer',
              display: 'flex',
              alignItems: 'center',
              gap: '0.75rem',
              fontSize: '0.875rem',
              fontWeight: '500',
              transition: 'all 0.2s'
            }}
            onMouseEnter={(e) => {
              if (activeView !== item.id) {
                e.currentTarget.style.backgroundColor = '#f3f4f6';
              }
            }}
            onMouseLeave={(e) => {
              if (activeView !== item.id) {
                e.currentTarget.style.backgroundColor = 'transparent';
              }
            }}
          >
            <span style={{ fontSize: '1rem' }}>{item.icon}</span>
            {item.label}
          </button>
        ))}
      </nav>
    </div>
  );
};

export default Sidebar;