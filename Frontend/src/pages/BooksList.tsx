import { useState, useEffect } from 'react';
import BookList from '../components/Book/BookList';
import BookHeader from '../components/Book/BookHeader';
import LoadingSpinner from '../components/Common/LoadingSpinner';
import ErrorMessage from '../components/Common/ErrorMessage';
import { useAuth } from '../context/AuthContext';
import type { BookSummaryDto } from '../types/index.ts';
import { bookService } from '../services/BookService';

const BooksList = () => {
  const { user } = useAuth();
  const [books, setBooks] = useState<BookSummaryDto[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadBooks();
  }, []);

  const loadBooks = async () => {
    try {
      setIsLoading(true);
      setError(null);
      const booksData = await bookService.getAllBooks();
      setBooks(booksData);
    } catch (error) {
      console.error('Failed to load books:', error);
      setError('Failed to load books');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div>
      <BookHeader 
        totalBooks={books.length} 
        isAdmin={false} 
      />

      {error && (
        <ErrorMessage message={error} onRetry={loadBooks} />
      )}

      {isLoading ? (
        <LoadingSpinner message="Loading books..." />
      ) : (
        <BookList
          books={books}
          isAdmin={false} 
        />
      )}
    </div>
  );
};

export default BooksList;