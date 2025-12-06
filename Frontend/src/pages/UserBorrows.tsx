import { useState, useEffect } from 'react';
import BorrowList from '../components/Borrow/BorrowList';
import BorrowForm from '../components/Borrow/BorrowForm';
import BorrowHeader from '../components/Borrow/BorrowHeader';
import LoadingSpinner from '../components/Common/LoadingSpinner';
import ErrorMessage from '../components/Common/ErrorMessage';
import { useAuth } from '../context/AuthContext';
import type { BorrowSummaryDto, CreateUserBorrowDto, BookSummaryDto } from '../types/index.ts';
import { borrowService } from '../services/BorrowService';
import { bookService } from '../services/BookService';

const UserBorrows = () => {
  const { user } = useAuth();
  const [borrows, setBorrows] = useState<BorrowSummaryDto[]>([]);
  const [books, setBooks] = useState<BookSummaryDto[]>([]);
  const [showBorrowForm, setShowBorrowForm] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadBorrows();
    loadBooks();
  }, []);

  const loadBorrows = async () => {
    try {
      setIsLoading(true);
      setError(null);
      const borrowsData = await borrowService.getMyBorrows();
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

  const handleCreateBorrow = async (borrowData: CreateUserBorrowDto) => {
    try {
      setIsLoading(true);
      await borrowService.createBorrow(borrowData);
      await loadBorrows();
      setShowBorrowForm(false);
      alert('Book borrowed successfully');
    } catch (error) {
      console.error('Failed to create borrow:', error);
      alert('Failed to borrow book');
    } finally {
      setIsLoading(false);
    }
  };

  const handleAddBorrow = () => {
    setShowBorrowForm(true);
  };

  const handleCancelForm = () => {
    setShowBorrowForm(false);
  };

  return (
    <div>
      <BorrowHeader 
        totalBorrows={borrows.length} 
        onAddBorrow={handleAddBorrow}
        isAdmin={false}
      />

      {error && (
        <ErrorMessage message={error} onRetry={loadBorrows} />
      )}

      {showBorrowForm && (
        <BorrowForm
          books={books}
          onSubmit={handleCreateBorrow}
          onCancel={handleCancelForm}
          isLoading={isLoading}
          isAdmin={false}
        />
      )}

      {isLoading && !showBorrowForm ? (
        <LoadingSpinner message="Loading borrow records..." />
      ) : (
        <BorrowList
          borrows={borrows}
          isAdmin={false}
        />
      )}
    </div>
  );
};

export default UserBorrows;