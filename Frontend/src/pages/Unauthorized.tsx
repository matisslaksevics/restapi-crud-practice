const Unauthorized = () => {
  return (
    <div className="bg-white p-8 rounded-lg shadow-sm text-center">
      <h2 className="text-2xl font-bold text-red-600 mb-4">Access Denied</h2>
      <p className="text-gray-600 mb-6">
        You don't have permission to access this page. Admin privileges required.
      </p>
      <button
        onClick={() => window.history.back()}
        className="bg-blue-500 text-white px-6 py-2 rounded hover:bg-blue-600 transition-colors"
      >
        Go Back
      </button>
    </div>
  );
};

export default Unauthorized;