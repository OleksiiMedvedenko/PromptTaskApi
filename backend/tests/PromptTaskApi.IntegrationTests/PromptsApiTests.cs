using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using PromptTaskApi.Api.Contracts;
using PromptTaskApi.Application.Prompts;
using PromptTaskApi.Domain.Prompts;
using Xunit;

namespace PromptTaskApi.IntegrationTests;

public sealed class PromptsApiTests : IClassFixture<PromptTaskApiWebApplicationFactory>
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly HttpClient _client;

    public PromptsApiTests(PromptTaskApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateBatch_ShouldReturnAcceptedPromptJobs()
    {
        var request = new CreatePromptsHttpRequest(["Prompt one", "Prompt two"]);

        var response = await _client.PostAsJsonAsync("/api/prompts/batch", request);

        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        var jobs = await response.Content.ReadFromJsonAsync<IReadOnlyList<PromptJobDto>>(JsonOptions);
        jobs.Should().NotBeNull();
        jobs!.Should().HaveCount(2);
        jobs.Should().OnlyContain(x => x.Status == PromptStatus.Pending);
    }

    [Fact]
    public async Task CreateBatch_WithEmptyPrompts_ShouldReturnBadRequest()
    {
        var request = new CreatePromptsHttpRequest([]);

        var response = await _client.PostAsJsonAsync("/api/prompts/batch", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>(JsonOptions);
        error.Should().NotBeNull();
        error!.Code.Should().Be("VALIDATION_FAILED");
        error.Errors.Should().NotBeNull();
        error.Errors!.Should().Contain(x => x.Code == "PROMPTS_REQUIRED");
    }

    [Fact]
    public async Task GetById_WhenPromptDoesNotExist_ShouldReturnNotFoundWithErrorCode()
    {
        var response = await _client.GetAsync($"/api/prompts/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>(JsonOptions);
        error.Should().NotBeNull();
        error!.Code.Should().Be("PROMPT_NOT_FOUND");
    }
}
