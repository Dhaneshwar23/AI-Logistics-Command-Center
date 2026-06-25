using AILogistics.Application.Interfaces;
using AILogistics.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Infrastructure.Services
{
    public class TrackingEventService : ITrackingEventService
    {
        private readonly ApplicationDbContext _context;

        public TrackingEventService(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
