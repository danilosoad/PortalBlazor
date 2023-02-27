namespace PortalBlazor.Core.Services.JobJourney
{
    public interface IJobJourneyService
    {
        DateTime GetHourToLeave(DateTime beginJourney, DateTime beginLaunch, DateTime endLaunch);
    }
}