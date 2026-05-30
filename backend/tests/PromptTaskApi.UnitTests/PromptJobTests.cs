using FluentAssertions;
using PromptTaskApi.Domain.Prompts;
using Xunit;

namespace PromptTaskApi.UnitTests;

public sealed class PromptJobTests
{
    [Fact]
    public void Create_ShouldCreatePendingJob()
    {
        var job = PromptJob.Create("Summarize this document");

        job.Prompt.Should().Be("Summarize this document");
        job.Status.Should().Be(PromptStatus.Pending);
        job.Result.Should().BeNull();
        job.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public void Complete_ShouldSetCompletedStatusAndResult()
    {
        var job = PromptJob.Create("Prompt");

        job.MarkAsProcessing();
        job.Complete("Result");

        job.Status.Should().Be(PromptStatus.Completed);
        job.Result.Should().Be("Result");
        job.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public void Fail_ShouldSetFailedStatusAndErrorMessage()
    {
        var job = PromptJob.Create("Prompt");

        job.MarkAsProcessing();
        job.Fail("Model error");

        job.Status.Should().Be(PromptStatus.Failed);
        job.ErrorMessage.Should().Be("Model error");
    }
}
