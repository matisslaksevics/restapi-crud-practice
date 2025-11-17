import type { ReactNode } from 'react';
import Header from './Header';
import Sidebar from './Sidebar';

interface MainLayoutProps {
  children: ReactNode;
  activeView: string;
  onViewChange: (view: string) => void;
}

const MainLayout = ({ children, activeView, onViewChange }: MainLayoutProps) => {
  return (
    <div className="min-h-screen bg-gray-50">
      <Header />
      <div className="max-w-7xl mx-auto px-4 flex">
        <Sidebar activeView={activeView} onViewChange={onViewChange} />
        <main className="flex-1 p-8 min-h-[calc(100vh-80px)]">
          {children}
        </main>
      </div>
    </div>
  );
};

export default MainLayout;