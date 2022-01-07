﻿using DisasterTrackerApp.Models.ApiModels;
using DisasterTrackerApp.Models.Disaster;

namespace DisasterTrackerApp.BL.HttpClients.Contract
{
    public interface IClosedDisasterEventsClient
    {
        public Task<ApiResponse<List<FeatureDto>>> GetDisasterEventsAsync(CancellationToken cancellationToken);
    }
}