import { useState, useEffect } from 'react';
import type { BookSummaryDto, CreateBookDto, UpdateBookDto } from '../../types/index.ts';

interface BookFormProps {
  book?: BookSummaryDto | null;
  onSubmit: (bookData: CreateBookDto | UpdateBookDto) => void;
  onCancel: () => void;
  isLoading?: boolean;
}

const BookForm = ({ book, onSubmit, onCancel, isLoading = false }: BookFormProps) => {
  const [formData, setFormData] = useState({
    bookName: '',
    releaseDate: ''
  });

  useEffect(() => {
    if (book) {
      setFormData({
        bookName: book.bookName,
        releaseDate: book.releaseDate.split('T')[0]
      });
    }
  }, [book]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSubmit(formData);
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50">
      <div className="bg-white rounded-lg p-6 w-full max-w-md">
        <h2 className="text-xl font-bold mb-4">
          {book ? 'Edit Book' : 'Add New Book'}
        </h2>
        
        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <label htmlFor="bookName" className="block text-sm font-medium text-gray-700 mb-1">
              Book Name *
            </label>
            <input
              type="text"
              id="bookName"
              name="bookName"
              value={formData.bookName}
              onChange={handleChange}
              required
              maxLength={100}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          </div>

          <div className="mb-6">
            <label htmlFor="releaseDate" className="block text-sm font-medium text-gray-700 mb-1">
              Release Date *
            </label>
            <input
              type="date"
              id="releaseDate"
              name="releaseDate"
              value={formData.releaseDate}
              onChange={handleChange}
              required
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          </div>

          <div className="flex justify-end gap-3">
            <button
              type="button"
              onClick={onCancel}
              disabled={isLoading}
              className="px-4 py-2 text-gray-600 border border-gray-300 rounded-md hover:bg-gray-50 disabled:opacity-50"
            >
              Cancel
            </button>
            <button
              type="submit"
              disabled={isLoading}
              className="px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 disabled:opacity-50"
            >
              {isLoading ? 'Saving...' : (book ? 'Update' : 'Create')}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default BookForm;