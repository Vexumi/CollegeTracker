namespace KST.DataAccess.Enums;

public enum ProjectState
{
    RequestedIncomplete,
    Requested,
    Approved,
    Rejected,
    Live,
    WaitingForMark,
    MarkRejected,
    Completed
}