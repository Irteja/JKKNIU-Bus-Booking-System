using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using JKKNIUBusBookingSystem.db;
using JKKNIUBusBookingSystem.Dtos;
using JKKNIUBusBookingSystem.Entites;
using System.Threading.Tasks;


namespace JKKNIUBusBookingSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ScheduleBusController : ControllerBase
{
    private readonly JKKNIUBusBookingSystemDbContext context;

    public ScheduleBusController(JKKNIUBusBookingSystemDbContext _context)
    {
        context = _context;
    }

    [HttpPost("addschedule")]
    public async Task<ActionResult> AddBusSchedule([FromBody] ScheduleBusDtos bus)
    {

        var newScheduleBus = new ScheduleBus
        {
            ScheduleId = Guid.NewGuid(),
            BusId = bus.BusId,
            Name = bus.Name,
            StartingAt = bus.StartingAt,
            StartingPlace = bus.StartingPlace,
            ScheduledDate = bus.ScheduledDate,
            theTownBusIsNow=bus.theTownBusIsNow
        };
        await context.ScheduleBuses.AddAsync(newScheduleBus);
        await context.SaveChangesAsync();

        var seats = await MakeBusSeatList(newScheduleBus);

        await context.ScheduleBusSeats.AddRangeAsync(seats);
        await context.SaveChangesAsync();


        return Ok("Added Successfully");
    }

    private async Task<List<ScheduleBusSeats>> MakeBusSeatList(ScheduleBus scheduledBus)
    {
        var mainBus = await context.Buses.Where(b => b.Id == scheduledBus.BusId).ToListAsync();
        var seats = new List<ScheduleBusSeats>();

        void AddSeat(string seatName) =>
        seats.Add(new ScheduleBusSeats { ScheduleId = scheduledBus.ScheduleId, SeatName = seatName });

        // Extra Seats
        for (int i = 1; i <= mainBus[0].NumberOfExtraSeats; i++)
        {
            AddSeat($"Ex-{i}");
        }

        // Center Row Seats
        for (int i = 0; i < mainBus[0].NumberOfRowInCenter; i++)
        {
            char rowChar = (char)('A' + i);
            int totalSeats = mainBus[0].NumberOfSeatsInCenterleft + mainBus[0].NumberOfSeatsInCenterRight;

            for (int j = 1; j <= totalSeats; j++)
            {
                AddSeat($"{rowChar}-{j}");
            }
        }

        // Last Row Seats
        for (int i = 1; i <= mainBus[0].NumberOfSeatsInLastSeries; i++)
        {
            AddSeat($"Last-{i}");
        }

        return seats;
    }

    [HttpPost("schedulebuses")]
    public async Task<IEnumerable<ScheduleBusDtos>> GetBusList([FromBody] QueryParameterForGettingScheduleBus queryParameterForGettingScheduleBus)
    {
        // Console.WriteLine(queryParameterForGettingScheduleBus.StartingPlace);
        List<ScheduleBus>? Allbuses = await context.ScheduleBuses.Where(
        bus => bus.StartingAt >= queryParameterForGettingScheduleBus.CurrentTime
        && bus.ScheduledDate == queryParameterForGettingScheduleBus.CurrentDate
        ).ToListAsync();

        var finalBuses = Allbuses.Select(bus => new ScheduleBusDtos
        {
            ScheduleId = bus.ScheduleId,
            BusId = bus.BusId,
            Name = bus.Name,
            StartingAt = bus.StartingAt,
            ScheduledDate = bus.ScheduledDate,
            StartingPlace = bus.StartingPlace,
            theTownBusIsNow=bus.theTownBusIsNow
        });
        

        return finalBuses;
    }
}