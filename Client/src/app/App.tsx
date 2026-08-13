import LoginPage from '@/features/auth/pages/LoginPage'
import './App.css'
import { Navigate, Route, Routes } from 'react-router-dom'
import ProtectedRoute from '@/routes/ProtectedRoute'
import MainLayout from '@/components/layouts/MainLayout';
import CustomersPage from '@/features/customers/CustomersPage';
import ShipmentsPage from '@/features/shipments/ShipmentsPage';
import useAuth from '@/context/useAuth';
import TrackingPage from '@/features/tracking/TrackingPage';

function App() {
  //const CustomersPage = () => <h1>Customers Page</h1>;

  //const ShipmentsPage = () => <h1>Shipments Page</h1>;

  const DashboardPage = () => <h1>Dashboard Page</h1>;

  const RootRedirect = () => {
    const { isAuthenticated } = useAuth();

    return (
      <Navigate to={isAuthenticated ? "/dashboard" : "/login"} />
    );
  };

  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />

      <Route element={<ProtectedRoute />}>
        <Route element={<MainLayout />}>
          <Route path="/customers" element={<CustomersPage />} />
          <Route path="/shipments" element={<ShipmentsPage />} />
          <Route path="/tracking" element={<TrackingPage />} />
          <Route path="/dashboard" element={<DashboardPage />} />
        </Route>
      </Route>
      <Route path="/" element={<RootRedirect />} />
    </Routes>
  );
}

export default App
