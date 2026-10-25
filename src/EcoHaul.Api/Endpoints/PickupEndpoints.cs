using Microsoft.AspNetCore.Mvc;
using EcoHaul.Api.Extensions;
using EcoHaul.Api.Infrastructure;
using EcoHaul.Application.Abstractions.Messaging;
using EcoHaul.Domain;
using EcoHaul.Application.Pickups;
using EcoHaul.Application.Pickups.Collect;
using EcoHaul.Application.Pickups.Create;
using EcoHaul.Application.Pickups.Get;
using EcoHaul.Application.Pickups.GetById;
using EcoHaul.Application.Pickups.Patch;
using EcoHaul.Application.Pickups.Replace;
using EcoHaul.Application.Pickups.Schedule;
using EcoHaul.SharedKernel;

namespace EcoHaul.Api.Endpoints;

public static class PickupEndpoints
{
    private const string CustomerHeader = "X-Customer-Id";

    public static IEndpointRouteBuilder MapPickupEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/pickups");

        group.MapGet("/", List);
        group.MapGet("/{id:guid}", Get);
        group.MapPost("/", Create);
        group.MapPut("/{id:guid}", Replace);
        group.MapPatch("/{id:guid}", Patch);
        group.MapPost("/{id:guid}/schedule", Schedule);
        group.MapPost("/{id:guid}/collect", Collect);

        return app;
    }

    private static async Task<IResult> List(
        [FromHeader(Name = CustomerHeader)] Guid customerId,
        PickupStatus? status,
        DateOnly? requestedFrom,
        DateOnly? requestedTo,
        int? offset,
        int? limit,
        IQueryHandler<GetPickupsQuery, PickupPage> handler,
        CancellationToken cancellationToken)
    {
        Result<PickupPage> result = await handler.Handle(
            new GetPickupsQuery(
                customerId,
                status,
                requestedFrom,
                requestedTo,
                offset ?? 0,
                limit ?? PickupPage.DefaultLimit),
            cancellationToken);

        return result.Match(Results.Ok, CustomResults.Problem);
    }

    private static async Task<IResult> Get(
        [FromHeader(Name = CustomerHeader)] Guid customerId,
        Guid id,
        IQueryHandler<GetPickupQuery, PickupResponse> handler,
        CancellationToken cancellationToken)
    {
        Result<PickupResponse> result = await handler.Handle(
            new GetPickupQuery(id, customerId),
            cancellationToken);

        return result.Match(Results.Ok, CustomResults.Problem);
    }

    private static async Task<IResult> Create(
        [FromHeader(Name = CustomerHeader)] Guid customerId,
        PickupBody body,
        ICommandHandler<CreatePickupCommand, PickupResponse> handler,
        CancellationToken cancellationToken)
    {
        Result<PickupResponse> result = await handler.Handle(
            new CreatePickupCommand(
                customerId,
                body.SiteName,
                body.Material,
                body.WeightKg,
                body.RequestedOn,
                body.Notes),
            cancellationToken);

        return result.Match(
            pickup => Results.Created($"/pickups/{pickup.Id}", pickup),
            CustomResults.Problem);
    }

    private static async Task<IResult> Replace(
        [FromHeader(Name = CustomerHeader)] Guid customerId,
        Guid id,
        PickupBody body,
        ICommandHandler<ReplacePickupCommand> handler,
        CancellationToken cancellationToken)
    {
        Result result = await handler.Handle(
            new ReplacePickupCommand(
                id,
                customerId,
                body.SiteName,
                body.Material,
                body.WeightKg,
                body.RequestedOn,
                body.Notes),
            cancellationToken);

        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    private static async Task<IResult> Patch(
        [FromHeader(Name = CustomerHeader)] Guid customerId,
        Guid id,
        PatchPickupBody body,
        ICommandHandler<PatchPickupCommand> handler,
        CancellationToken cancellationToken)
    {
        Result result = await handler.Handle(
            new PatchPickupCommand(
                id,
                customerId,
                body.SiteName,
                body.Material,
                body.WeightKg,
                body.RequestedOn,
                body.Notes),
            cancellationToken);

        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    private static async Task<IResult> Schedule(
        [FromHeader(Name = CustomerHeader)] Guid customerId,
        Guid id,
        ICommandHandler<SchedulePickupCommand> handler,
        CancellationToken cancellationToken)
    {
        Result result = await handler.Handle(
            new SchedulePickupCommand(id, customerId),
            cancellationToken);

        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    private static async Task<IResult> Collect(
        [FromHeader(Name = CustomerHeader)] Guid customerId,
        Guid id,
        ICommandHandler<CollectPickupCommand> handler,
        CancellationToken cancellationToken)
    {
        Result result = await handler.Handle(
            new CollectPickupCommand(id, customerId),
            cancellationToken);

        return result.Match(Results.NoContent, CustomResults.Problem);
    }
}

public sealed record PickupBody(
    string SiteName,
    Material Material,
    decimal WeightKg,
    DateOnly RequestedOn,
    string? Notes);

public sealed record PatchPickupBody(
    Optional<string?> SiteName,
    Optional<Material?> Material,
    Optional<decimal?> WeightKg,
    Optional<DateOnly?> RequestedOn,
    Optional<string?> Notes);
