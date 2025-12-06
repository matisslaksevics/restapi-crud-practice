import { useState, useEffect } from 'react';
import type { BookSummaryDto, ClientSummaryDto, CreateBorrowDto, CreateUserBorrowDto, UpdateBorrowDto } from '../../types/index.ts';

interface BorrowFormProps {
  books: BookSummaryDto[];
  clients?: ClientSummaryDto[];
  onSubmit: (borrowData: CreateBorrowDto | CreateUserBorrowDto | UpdateBorrowDto) => void;
  onCancel: () => void;
  isLoading?: boolean;
  isAdmin?: boolean;
  existingBorrow?: any;
}

const BorrowForm = ({ 
  books, 
  clients = [], 
  onSubmit, 
  onCancel, 
  isLoading = false, 
  isAdmin = false,
  existingBorrow 
}: BorrowFormProps) => {
  const [formData, setFormData] = useState({
    clientId: '',
    bookId: 0,
    borrowDate: new Date().toISOString().split('T')[0],
    returnDate: ''
  });

  useEffect(() => {
    if (existingBorrow) {
      setFormData({
        clientId: existingBorrow.clientId || '',
        bookId: existingBorrow.bookId || 0,
        borrowDate: existingBorrow.borrowDate || new Date().toISOString().split('T')[0],
        returnDate: existingBorrow.returnDate || ''
      });
    }
  }, [existingBorrow]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    
    const submitData: any = {
      bookId: formData.bookId,
      borrowDate: formData.borrowDate,
    };

    if (formData.returnDate) {
      submitData.returnDate = formData.returnDate;
    }

    if (isAdmin) {
      submitData.clientId = formData.clientId;
    }

    onSubmit(submitData);
  };

  const handleChange = (e: React.ChangeEvent<HTMLSelectElement | HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: name === 'bookId' ? parseInt(value) : value
    }));
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50">
      <div className="bg-white rounded-lg p-6 w-full max-w-md">
        <h2 className="text-xl font-bold mb-4">
          {existingBorrow ? 'Edit Borrow Record' : 'Create New Borrow'}
        </h2>
        
        <form onSubmit={handleSubmit}>
          {isAdmin && (
            <div className="mb-4">
              <label htmlFor="clientId" className="block text-sm font-medium text-gray-700 mb-1">
                Client *
              </label>
              <select
                id="clientId"
                name="clientId"
                value={formData.clientId}
                onChange={handleChange}
                required
                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              >
                <option value="">Select a client</option>
                {clients.map((client) => (
                  <option key={client.id} value={client.id}>
                    {client.username} {client.firstName ? `(${client.firstName} ${client.lastName})` : ''}
                  </option>
                ))}
              </select>
            </div>
          )}

          <div className="mb-4">
            <label htmlFor="bookId" className="block text-sm font-medium text-gray-700 mb-1">
              Book *
            </label>
            <select
              id="bookId"
              name="bookId"
              value={formData.bookId}
              onChange={handleChange}
              required
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            >
              <option value="">Select a book</option>
              {books.map((book) => (
                <option key={book.id} value={book.id}>
                  {book.bookName}
                </option>
              ))}
            </select>
          </div>

          <div className="mb-4">
            <label htmlFor="borrowDate" className="block text-sm font-medium text-gray-700 mb-1">
              Borrow Date *
            </label>
            <input
              type="date"
              id="borrowDate"
              name="borrowDate"
              value={formData.borrowDate}
              onChange={handleChange}
              required
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          </div>

          <div className="mb-6">
            <label htmlFor="returnDate" className="block text-sm font-medium text-gray-700 mb-1">
              Return Date (Optional)
            </label>
            <input
              type="date"
              id="returnDate"
              name="returnDate"
              value={formData.returnDate}
              onChange={handleChange}
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
              {isLoading ? 'Saving...' : (existingBorrow ? 'Update' : 'Create')}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default BorrowForm;