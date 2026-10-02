namespace PlainApp.Enums
{
    public enum PlanStatusCode
    {
        NotStarted = 0,
        JustStarted = 1,
        InProgress = 2,
        DeadlineComing = 3,
        CompletedEarly = 4,
        CompletedOnTime = 5,
        CompletedLate = 6
    }

    public enum SimplifiedPlanStatusCode
    {
        NotStarted = 0,
        InProgress = 1,
        Completed = 2
    }
}
