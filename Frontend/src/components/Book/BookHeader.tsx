interface BookHeaderProps {
  totalBooks: number;
  onAddBook?: () => void;
  isAdmin?: boolean;
}

const BookHeader = ({ totalBooks, onAddBook, isAdmin }: BookHeaderProps) => {
  return (
    <div className="flex justify-between items-center mb-6">
      <div>
        <h1 className="text-2xl font-bold text-gray-900">
          {isAdmin ? 'Book Management' : 'Books List'}
        </h1>
        <p className="text-gray-600">{totalBooks} books available</p>
      </div>
      {isAdmin && onAddBook && (
        <button
          onClick={onAddBook}
          className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 transition-colors"
        >
          Add New Book
        </button>
      )}
    </div>
  );
};

export default BookHeader;