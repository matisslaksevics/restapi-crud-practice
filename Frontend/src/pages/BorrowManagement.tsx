import { useState, useEffect } from 'react';
import BorrowList from '../components/Borrow/BorrowList';
import BorrowForm from '../components/Borrow/BorrowForm';
import BorrowHeader from '../components/Borrow/BorrowHeader';
import LoadingSpinner from '../components/Common/LoadingSpinner';
import ErrorMessage from '../components/Common/ErrorMessage';
import Unauthorized from './Unauthorized';
import { useAuth } from '../context/AuthContext';
import type { 
  BorrowSummaryDto, 
  CreateBorrowDto, 
  UpdateBorrowDto, 
  BookSummaryDto, 
  ClientSummaryDto,
  CreateUserBorrowDto
} from '../types/index.ts';
import { borrowService } from '../services/BorrowService';
import { bookService } from '../services/BookService';
import { clientService } from '../services/ClientService';

const BorrowManagement = () => {
  const { user } = useAuth();
  const [borrows, setBorrows] = useState<BorrowSummaryDto[]>([]);
  const [books, setBooks] = useState<BookSummaryDto[]>([]);
  const [clients, setClients] = useState<ClientSummaryDto[]>([]);
  const [editingBorrow, setEditingBorrow] = useState<BorrowSummaryDto | null>(null);
  const [showBorrowForm, setShowBorrowForm] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  if (!user || user.role !== 'Admin') {
    return <Unauthorized />;
  }

  useEffect(() => {
    loadBorrows();
    loadBooks();
    loadClients();
  }, []);

  const loadBorrows = async () => {
    try {
      setIsLoading(true);
      setError(null);
      const borrowsData = await borrowService.getAllBorrows();
      setBorrows(borrowsData);
    } catch (error) {
      console.error('Failed to load borrows:', error);
      setError('Failed to load borrow records');
    } finally {
      setIsLoading(false);
    }
  };

  const loadBooks = async () => {
    try {
      const booksData = await bookService.getAllBooks();
      setBooks(booksData);
    } catch (error) {
      console.error('Failed to load books:', error);
    }
  };

  const loadClients = async () => {
    try {
      const clientsData = await clientService.getAllClients();
      setClients(clientsData);
    } catch (error) {
      console.error('Failed to load clients:', error);
    }
  };

  const handleCreateBorrow = async (borrowData: CreateBorrowDto) => {
    try {
      setIsLoading(true);
      await borrowService.adminCreateBorrow(borrowData);
      await loadBorrows();
      setShowBorrowForm(false);
      alert('Borrow record created successfully');
    } catch (error) {
      console.error('Failed to create borrow:', error);
      alert('Failed to create borrow record');
    } finally {
      setIsLoading(false);
    }
  };

  const handleUpdateBorrow = async (borrowData: UpdateBorrowDto) => {
    if (!editingBorrow) return;
    
    try {
      setIsLoading(true);
      await borrowService.updateBorrow(editingBorrow.id, borrowData);
      await loadBorrows();
      setEditingBorrow(null);
      setShowBorrowForm(false);
      alert('Borrow record updated successfully');
    } catch (error) {
      console.error('Failed to update borrow:', error);
      alert('Failed to update borrow record');
    } finally {
      setIsLoading(false);
    }
  };

  const handleDeleteBorrow = async (id: number) => {
    if (!confirm('Are you sure you want to delete this borrow record?')) return;
    
    try {
      await borrowService.deleteBorrow(id);
      setBorrows(prev => prev.filter(borrow => borrow.id !== id));
      alert('Borrow record deleted successfully');
    } catch (error: any) {
      console.error('Failed to delete borrow:', error);
      alert(`Failed to delete borrow record: ${error.response?.data?.message || error.message}`);
    }
  };

  const handleAddBorrow = () => {
    setEditingBorrow(null);
    setShowBorrowForm(true);
  };

  const handleEditBorrow = (borrow: BorrowSummaryDto) => {
    setEditingBorrow(borrow);
    setShowBorrowForm(true);
  };

  const handleCancelForm = () => {
    setShowBorrowForm(false);
    setEditingBorrow(null);
  };

  const handleFormSubmit = (borrowData: CreateBorrowDto | CreateUserBorrowDto | UpdateBorrowDto) => {
  if (editingBorrow) {
    handleUpdateBorrow(borrowData as UpdateBorrowDto);
  } else {
    handleCreateBorrow(borrowData as CreateBorrowDto);
  }
};

  return (
    <div>
      <BorrowHeader 
        totalBorrows={borrows.length} 
        onAddBorrow={handleAddBorrow}
        isAdmin={true}
      />

      {error && (
        <ErrorMessage message={error} onRetry={loadBorrows} />
      )}

      {showBorrowForm && (
        <BorrowForm
          books={books}
          clients={clients}
          onSubmit={handleFormSubmit}
          onCancel={handleCancelForm}
          isLoading={isLoading}
          isAdmin={true}
          existingBorrow={editingBorrow}
        />
      )}

      {isLoading && !showBorrowForm ? (
        <LoadingSpinner message="Loading borrow records..." />
      ) : (
        <BorrowList
          borrows={borrows}
          onEdit={handleEditBorrow}
          onDelete={handleDeleteBorrow}
          isAdmin={true}
        />
      )}
    </div>
  );
};

export default BorrowManagement;