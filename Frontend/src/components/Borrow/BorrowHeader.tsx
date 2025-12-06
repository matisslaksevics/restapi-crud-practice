interface BorrowHeaderProps {
  totalBorrows: number;
  onAddBorrow?: () => void;
  isAdmin?: boolean;
}

const BorrowHeader = ({ totalBorrows, onAddBorrow, isAdmin }: BorrowHeaderProps) => {
  return (
    <div className="flex justify-between items-center mb-6">
      <div>
        <h1 className="text-2xl font-bold text-gray-900">
          {isAdmin ? 'Overdue Books' : 'My Overdue Books'}
        </h1>
        <p className="text-gray-600">
          {totalBorrows} overdue book{totalBorrows !== 1 ? 's' : ''}
        </p>
      </div>
      {onAddBorrow && (
        <button
          onClick={onAddBorrow}
          className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 transition-colors"
        >
          New Borrow
        </button>
      )}
    </div>
  );
};

export default BorrowHeader;