namespace PortalBlazor.Core.Services.JobJourney
{
    public class JobJourneyService : IJobJourneyService
    {
        public DateTime GetHourToLeave(DateTime beginJourney, DateTime beginLaunch, DateTime endLaunch)
        {
            var journeyToBeWorked = new TimeSpan(8, 48, 0);
            var rest = beginLaunch - endLaunch;

            return beginJourney - rest + journeyToBeWorked;
        }
    }
}