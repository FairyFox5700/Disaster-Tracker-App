using DisasterTrackerApp.Entities;
using DisasterTrackerApp.Models.Calendar;

namespace DisasterTrackerApp.BL.Mappers.Implementation;

public static class WatchChannelDataMapper
{
    public static WatchChannelEntity? MapChannelDataDtoToEntity(WatchChannelData? channelData)
    {
        if (channelData == null)
        {
            return null;
        }
        
        return new WatchChannelEntity
            {
                UserId = channelData.UserId,
                ChannelId = channelData.ChannelId,
                ChannelToken = channelData.ChannelToken,
                LastTimeTriggered = channelData.LastTimeTriggered,
                ResourceId = channelData.ResourceId
            };
    }
    
    public static WatchChannelData? MapChannelDataEntityToDto(WatchChannelEntity? channelData)
    {
        if (channelData == null)
        {
            return null;
        }
        
        return new WatchChannelData(channelData.ChannelToken, channelData.UserId, channelData.ChannelId, 
            channelData.ResourceId, channelData.LastTimeTriggered);
    }
}