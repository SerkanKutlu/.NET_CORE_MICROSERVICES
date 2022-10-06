using Core.Events;
using Core.Interfaces;
using Hangfire;

namespace DocumentService.Infrastructure.Hangfire;

public class JobService
{
    private readonly IKafkaPublisher _kafkaPublisher;

    public JobService(IKafkaPublisher kafkaPublisher)
    {
        _kafkaPublisher = kafkaPublisher;
    }

    public void AddLogJob(string documentId)
    {
        var targetHour = CalculateJobTime();
        var documentUploaded = new DocumentUploaded(documentId, DateTime.Now);
        var enqueueTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, targetHour, 0, 0);
        BackgroundJob.Schedule(() => _kafkaPublisher.Publish(documentUploaded), enqueueTime);
    }

    private int CalculateJobTime()
    {
        var hoursList = new List<int>{24, 6, 12, 18};
        var actualHour = DateTime.Now.Hour;
        var difference = int.MaxValue;
        var targetHour = 0;
        foreach (var hour in hoursList)
        {
            var tempDifference = (hour - actualHour)>0? (hour-actualHour) : int.MaxValue;
            difference = tempDifference < difference ? tempDifference : difference;
            targetHour = difference == tempDifference ? hour : targetHour;
            targetHour = targetHour == 24 ? 0 : targetHour;
        }
        return targetHour;
    }

    public void RemoveDeletedJob(string documentId)
    {
        var monitor = JobStorage.Current.GetMonitoringApi();
        var scheduledJobs = monitor.ScheduledJobs(0, int.MaxValue);
        foreach (var job in scheduledJobs)
        {
            var jobArg = (DocumentUploaded)job.Value.Job.Args[0];
            if (jobArg.DocumentId == documentId)
            {
                BackgroundJob.Delete(job.Key);
            }
        }
    }
}