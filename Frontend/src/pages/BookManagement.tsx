import { useState, useEffect } from 'react';
import BookList from '../components/Book/BookList';
import BookForm from '../components/Book/BookForm';
import BookHeader from '../components/Book/BookHeader';
import LoadingSpinner from '../components/Common/LoadingSpinner';
import ErrorMessage from '../components/Common/ErrorMessage';
import Unauthorized from './Unauthorized';
import { useAuth } from '../context/AuthContext';
import type { BookSummaryDto, CreateBookDto, UpdateBookDto } from '../types/index.ts';
import { bookService } from '../services/BookService';

const BookManagement = () => {
  const { user } = useAuth();
  const [books, setBooks] = useState<BookSummaryDto[]>([]);
  const [editingBook, setEditingBook] = useState<BookSummaryDto | null>(null);
  const [showBookForm, setShowBookForm] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const isAdmin = user?.role === 'Admin';

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

  const handleCreateBook = async (bookData: CreateBookDto) => {
    try {
      setIsLoading(true);
      const newBook = await bookService.createBook(bookData);
      setBooks(prev => [...prev, newBook]);
      setShowBookForm(false);
      alert('Book created successfully');
    } catch (error) {
      console.error('Failed to create book:', error);
      alert('Failed to create book');
    } finally {
      setIsLoading(false);
    }
  };

  const handleUpdateBook = async (bookData: UpdateBookDto) => {
    if (!editingBook) return;
    
    try {
      setIsLoading(true);
      const updatedBook = await bookService.updateBook(editingBook.id, bookData);
      setBooks(prev => prev.map(book => 
        book.id === editingBook.id ? updatedBook : book
      ));
      setEditingBook(null);
      setShowBookForm(false);
      alert('Book updated successfully');
    } catch (error) {
      console.error('Failed to update book:', error);
      alert('Failed to update book');
    } finally {
      setIsLoading(false);
    }
  };

  const handleDeleteBook = async (id: number) => {
    if (!confirm('Are you sure you want to delete this book?')) return;
    
    try {
      await bookService.deleteBook(id);
      setBooks(prev => prev.filter(book => book.id !== id));
      alert('Book deleted successfully');
    } catch (error: any) {
      console.error('Failed to delete book:', error);
      alert(`Failed to delete book: ${error.response?.data?.message || error.message}`);
    }
  };

  const handleEditBook = (book: BookSummaryDto) => {
    setEditingBook(book);
    setShowBookForm(true);
  };

  const handleAddBook = () => {
    setEditingBook(null);
    setShowBookForm(true);
  };

  const handleCancelForm = () => {
    setShowBookForm(false);
    setEditingBook(null);
  };

  const handleFormSubmit = (bookData: CreateBookDto | UpdateBookDto) => {
    if (editingBook) {
      handleUpdateBook(bookData as UpdateBookDto);
    } else {
      handleCreateBook(bookData as CreateBookDto);
    }
  };

  if (!user) {
    return <Unauthorized />;
  }

  return (
    <div>
      <BookHeader 
        totalBooks={books.length} 
        onAddBook={isAdmin ? handleAddBook : undefined}
        isAdmin={isAdmin}
      />

      {error && (
        <ErrorMessage message={error} onRetry={loadBooks} />
      )}

      {showBookForm && (
        <BookForm
          book={editingBook}
          onSubmit={handleFormSubmit}
          onCancel={handleCancelForm}
          isLoading={isLoading}
        />
      )}

      {isLoading && !showBookForm ? (
        <LoadingSpinner message="Loading books..." />
      ) : (
        <BookList
          books={books}
          onEdit={isAdmin ? handleEditBook : undefined}
          onDelete={isAdmin ? handleDeleteBook : undefined}
          isAdmin={isAdmin}
        />
      )}
    </div>
  );
};

export default BookManagement;