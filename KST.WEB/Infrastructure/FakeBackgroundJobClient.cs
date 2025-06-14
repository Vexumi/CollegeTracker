using Hangfire;
using Hangfire.Common;
using Hangfire.States;

namespace KST.WEB.Infrastructure;

public sealed class FakeBackgroundJobClient : IBackgroundJobClient
{
    public string Create(Job job, IState state)
    {
        return "Fake Job";
    }

    public bool ChangeState(string jobId, IState state, string expectedState)
    {
        return true;
    }
}