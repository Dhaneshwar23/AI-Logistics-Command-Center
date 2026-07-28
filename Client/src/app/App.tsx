import LoginPage from '@/features/auth/pages/LoginPage'
import './App.css'
import { Navigate, Route, Routes } from 'react-router-dom'
import ProtectedRoute from '@/routes/ProtectedRoute'

function App() {
 const CustomersPage = () => <h1>Customers Page</h1>;

const ShipmentsPage = () => <h1>Shipments Page</h1>;

const TrackingPage = () => <h1>Tracking Page</h1>;
  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />

      <Route element={<ProtectedRoute />}>
        <Route path="/customers" element={<CustomersPage />} />
        <Route path="/shipments" element={<ShipmentsPage />} />
        <Route path="/tracking" element={<TrackingPage />} />
      </Route>

      <Route path="/" element={<Navigate to="/customers" replace />} /> 
    </Routes>
  );
}

export default App
