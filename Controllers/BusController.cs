using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using JKKNIUBusBookingSystem.db;
using JKKNIUBusBookingSystem.Dtos;
using JKKNIUBusBookingSystem.Entites;
using System.Runtime.InteropServices;


namespace JKKNIUBusBookingSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BusController : ControllerBase
{
    private readonly JKKNIUBusBookingSystemDbContext context;
    public BusController(JKKNIUBusBookingSystemDbContext _context)
    {
        context = _context;
    }

    [HttpPost("addbus")]
    public async Task<ActionResult<BusDtos>> AddBus([FromBody] BusDtos bus)
    {
        var newBus = new Bus
        {
            Id = Guid.NewGuid(),
            Name = bus.Name,
            deckerType = bus.deckerType,
            BusModel = bus.BusModel,
            NumberOfExtraSeats = bus.NumberOfExtraSeats,
            NumberOfRowInCenter = bus.NumberOfRowInCenter,
            NumberOfSeatsInCenterleft = bus.NumberOfSeatsInCenterleft,
            NumberOfSeatsInCenterRight = bus.NumberOfSeatsInCenterRight,
            NumberOfSeatsInLastSeries = bus.NumberOfSeatsInLastSeries
        };
        await context.Buses.AddAsync(newBus);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(AddBus), new { id = newBus.Id }, newBus);
    }
    [HttpGet("allbuses")]
    public async Task<IEnumerable<GettingBusDtos>> GetBusList()
    {
        List<Bus>? Allbuses = await context.Buses.ToListAsync();

        var finalBuses = Allbuses.Select(bus => new GettingBusDtos
        {
            Id = bus.Id,
            Name = bus.Name
        });

        return finalBuses;
    }
    [HttpGet("busdetails")]
    public async Task<IActionResult> GetBusDetails([FromQuery] Guid scheduleId)
    {
        try
        {
            Guid busId = await getBusId(scheduleId);
            var busDetail = await context.Buses.FirstOrDefaultAsync(bus =>
            bus.Id == busId
            );
            return Ok(new { message = "Bus found!", busDetail = busDetail });
        }
        catch
        {
            return NotFound(new { message = "No buses found!" });
        }

    }
    private async Task<Guid> getBusId(Guid scheduleId)
    {
        try
        {
            var bus = await context.ScheduleBuses.FirstOrDefaultAsync(bus =>
            bus.ScheduleId == scheduleId
            );
            if (bus == null) return Guid.Empty;
            return bus.BusId;
        }
        catch
        {
            return Guid.Empty;
        }
    }
}