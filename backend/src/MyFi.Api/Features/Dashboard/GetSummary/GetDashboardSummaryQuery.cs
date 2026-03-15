using MediatR;
using MyFi.Api.Common.Results;
using System.Text.Json.Serialization;

namespace MyFi.Api.Features.Dashboard;

public sealed record GetDashboardSummaryQuery : IRequest<Result<DashboardSummaryResponse>>
{
    [JsonIgnore]
    public Guid UserId { get; init; }
}
