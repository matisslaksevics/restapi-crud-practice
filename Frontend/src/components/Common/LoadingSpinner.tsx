interface LoadingSpinnerProps {
  message?: string;
  size?: 'sm' | 'md' | 'lg';
}

const LoadingSpinner = ({ message = 'Loading...', size = 'md' }: LoadingSpinnerProps) => {
  const sizeMap = {
    sm: 'w-5 h-5',
    md: 'w-10 h-10',
    lg: 'w-16 h-16'
  };

  return (
    <div className="text-center py-8 flex flex-col items-center gap-4">
      <div className={`${sizeMap[size]} border-4 border-gray-200 border-t-blue-500 rounded-full animate-spin`}></div>
      <div>{message}</div>
    </div>
  );
};

export default LoadingSpinner;