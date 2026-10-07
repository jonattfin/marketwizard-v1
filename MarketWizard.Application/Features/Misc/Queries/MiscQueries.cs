using MarketWizard.Application.Interfaces;
using MarketWizard.Domain.Entities;
using MediatR;

namespace MarketWizard.Application.Features.Misc.Queries;

public record GetIndicesQuery : IRequest<IndicePerformanceData>;
public record GetTopNewsQuery : IRequest<TopNewsData>;
public record GetSectorPerformanceQuery : IRequest<SectorPerformanceData>;
public record GetTopGainersQuery : IRequest<GainersData>;
public record GetTopLosersQuery : IRequest<GainersData>;
public record GetTopIndustriesQuery : IRequest<GainersData>;
public record GetWorstIndustriesQuery : IRequest<GainersData>;

public class MiscQueryHandlers(IMiscRepository miscRepository) :
    IRequestHandler<GetIndicesQuery, IndicePerformanceData>,
    IRequestHandler<GetTopNewsQuery, TopNewsData>,
    IRequestHandler<GetSectorPerformanceQuery, SectorPerformanceData>,
    IRequestHandler<GetTopGainersQuery, GainersData>,
    IRequestHandler<GetTopLosersQuery, GainersData>,
    IRequestHandler<GetTopIndustriesQuery, GainersData>,
    IRequestHandler<GetWorstIndustriesQuery, GainersData>
{
    public Task<IndicePerformanceData> Handle(GetIndicesQuery request, CancellationToken cancellationToken) =>
        miscRepository.GetIndices(cancellationToken);

    public Task<TopNewsData> Handle(GetTopNewsQuery request, CancellationToken cancellationToken) =>
        miscRepository.GetTopNews(cancellationToken);

    public Task<SectorPerformanceData> Handle(GetSectorPerformanceQuery request, CancellationToken cancellationToken) =>
        miscRepository.GetSectorPerformance(cancellationToken);

    public Task<GainersData> Handle(GetTopGainersQuery request, CancellationToken cancellationToken) =>
        miscRepository.GetTopGainers(cancellationToken);

    public Task<GainersData> Handle(GetTopLosersQuery request, CancellationToken cancellationToken) =>
        miscRepository.GetTopLosers(cancellationToken);

    public Task<GainersData> Handle(GetTopIndustriesQuery request, CancellationToken cancellationToken) =>
        miscRepository.GetTopIndustries(cancellationToken);

    public Task<GainersData> Handle(GetWorstIndustriesQuery request, CancellationToken cancellationToken) =>
        miscRepository.GetWorstIndustries(cancellationToken);
}
