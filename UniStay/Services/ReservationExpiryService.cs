// Services/ReservationExpiryService.cs
// Register in Program.cs:  builder.Services.AddHostedService<ReservationExpiryService>();

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using UniStay.Data;   // adjust to your DbContext namespace

namespace UniStay.Services;

public class ReservationExpiryService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ReservationExpiryService> _logger;

    // How often to check — every 15 minutes is fine
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(15);

    public ReservationExpiryService(
        IServiceScopeFactory scopeFactory,
        ILogger<ReservationExpiryService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("✅ ReservationExpiryService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CancelExpiredReservationsAsync("""Found {Count} expired reservations to cancel.""");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error inside ReservationExpiryService.");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task CancelExpiredReservationsAsync(string v)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DormitoryDbContext>();

        var now = DateTime.Now;
        var expired = await db.Applications
            .Include(a => a.PreferredRoom)
            .Where(a => a.Status == "PendingPayment"
                     && a.PaymentDeadline != null
                     && a.PaymentDeadline < now)
            .ToListAsync();

        if (!expired.Any()) return;

        _logger.LogInformation(v, expired.Count);

        foreach (var app in expired)
        {
            // 1. Cancel the application
            app.Status = "Cancelled";

            // 2. Free the room bed (decrement occupancy)
            if (app.PreferredRoom != null && app.PreferredRoom.CurrentOccupancy > 0)
                app.PreferredRoom.CurrentOccupancy--;

            // 3. Remove the provisional Allocation
            var allocation = await db.Allocations
                .FirstOrDefaultAsync(al => al.StudentId == app.StudentId
                                        && al.RoomId == app.PreferredRoom.RoomId
                                        && al.Status == "Reserved");
            if (allocation != null)
                db.Allocations.Remove(allocation);

            _logger.LogInformation(
                "  ↩ Cancelled reservation for StudentId={S}, RoomId={R}",
                app.StudentId, app.PreferredRoom.RoomId);
        }

        await db.SaveChangesAsync();
        _logger.LogInformation("Expiry cleanup done. {Count} reservations cancelled.", expired.Count);
    }
}