import api from '@/services/api';
import type { DashboardSummary } from '@/types/dashboard';

const dashboardService = {
    getSummary: async (): Promise<DashboardSummary> => {
        const response = await api.get<DashboardSummary>('/api/v1/dashboard/summary');
        return response.data;
    },
};

export default dashboardService;
