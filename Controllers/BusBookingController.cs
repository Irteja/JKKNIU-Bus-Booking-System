using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using JKKNIUBusBookingSystem.db;
using JKKNIUBusBookingSystem.Dtos;
using JKKNIUBusBookingSystem.Entites;
using System.Threading.Tasks;
using System.Collections;


namespace JKKNIUBusBookingSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BusBookingController : ControllerBase
{
    private readonly IAuthService authService;
    private readonly JKKNIUBusBookingSystemDbContext context;

    public BusBookingController(IAuthService _authService, JKKNIUBusBookingSystemDbContext _context)
    {
        context = _context;
        authService = _authService;
    }

    [HttpPut("bookseat")]
    public async Task<ActionResult> BookSeat([FromBody] RequestBookingDtos bookingRequest)
    {
        var didBookedBefore = await context.ScheduleBusSeats.FirstOrDefaultAsync(seat =>
        seat.BookedBy == bookingRequest.BookedBy && seat.ScheduleId == bookingRequest.ScheduleId
        );

        if (didBookedBefore != null)
        {
            return Conflict(new { message = "You have already booked a seat in this bus!" });
        }

        var originalSeat = await context.ScheduleBusSeats.FirstOrDefaultAsync(
        scheduleBus => scheduleBus.ScheduleId == bookingRequest.ScheduleId
        &&
        scheduleBus.SeatName == bookingRequest.SeatName
        );
        if (originalSeat == null)
        {
            return NotFound("Seat not found.");
        }

        if (null != originalSeat.BookedBy)
        {
            // Console.WriteLine("bookedby : ", originalSeat.BookedBy);
            return Conflict("Seat is already booked.");
        }
        originalSeat.BookedBy = bookingRequest.BookedBy;

        try
        {
            await context.SaveChangesAsync();
            return Ok(new { message = "Seat booked successfully" });
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict("Seat booking conflict. Please try again.");
        }
    }

    [HttpDelete("unbookseat")]
    public async Task<ActionResult> UnbookSeat([FromBody] RequestBookingDtos bookingRequest)
    {
        var originalSeat = await context.ScheduleBusSeats.FirstOrDefaultAsync(
        scheduleBus => scheduleBus.ScheduleId == bookingRequest.ScheduleId
        &&
        scheduleBus.SeatName == bookingRequest.SeatName
        && scheduleBus.BookedBy == bookingRequest.BookedBy
        );
        if (originalSeat == null)
        {
            return NotFound("Seat not found.");
        }
        originalSeat.BookedBy = null;
        try
        {
            await context.SaveChangesAsync();
            return Ok(new { message = "Seat unbooked successfully" });
        }
        catch (DbUpdateConcurrencyException)
        {
            return StatusCode(500, new { message = "Internal server error." });
        }
    }

    [HttpGet("schedulebuses")]
    public async Task<IEnumerable<ScheduleBusDtos>> GetBusList([FromBody] QueryParameterForGettingScheduleBus queryParameterForGettingScheduleBus)
    {
        Console.WriteLine(queryParameterForGettingScheduleBus.StartingPlace);
        List<ScheduleBus>? Allbuses = await context.ScheduleBuses.Where(
        bus => bus.StartingAt >= queryParameterForGettingScheduleBus.CurrentTime
        && bus.ScheduledDate == queryParameterForGettingScheduleBus.CurrentDate
        && bus.StartingPlace == queryParameterForGettingScheduleBus.StartingPlace
        ).ToListAsync();

        var finalBuses = Allbuses.Select(bus => new ScheduleBusDtos
        {
            ScheduleId = bus.ScheduleId,
            BusId = bus.BusId,
            Name = bus.Name,
            StartingAt = bus.StartingAt,
            ScheduledDate = bus.ScheduledDate,
            StartingPlace = bus.StartingPlace
        });


        return finalBuses;
    }


    [HttpGet("getseat")]
    public async Task<ActionResult<IEnumerable<SeatDtos>>> GetSeatList([FromQuery] Guid ScheduleId)
    {
        if (ScheduleId == Guid.Empty)
        {
            return BadRequest("ScheduleId is required.");
        }

        var allBuses = await context.ScheduleBusSeats
        .Where(seats => seats.ScheduleId == ScheduleId)
        .ToListAsync();

        var finalBuses = allBuses.Select(bus => new SeatDtos
        {
            SeatName = bus.SeatName,
            IsBooked = bus.BookedBy != null ? true : false
        });


        return Ok(finalBuses);
    }

    [HttpGet("getuserseat")]
    public async Task<ActionResult<IEnumerable>> GetUserSeat([FromQuery] Guid ScheduleId)
    {
        if (ScheduleId == Guid.Empty)
        {
            return BadRequest("ScheduleId is required.");
        }

        var allBuses = await context.ScheduleBusSeats
        .FirstOrDefaultAsync(seats => seats.ScheduleId == ScheduleId);


        return Ok(allBuses);
    }

    [HttpGet("checkseat")]
    public async Task<IActionResult> CheckIfAnySeatBookByTheUser([FromQuery] Guid userId)
    {
        var allBuses = await GetCurrentBusList();
        foreach (ScheduleBus bus in allBuses)
        {
            var exists = await context.ScheduleBusSeats.FirstOrDefaultAsync(seats =>
            seats.BookedBy == userId &&
            seats.ScheduleId == bus.ScheduleId

            );
            // Console.WriteLine("incoming");
            if (exists != null) return Ok(new { message = "You have a seat already!", seat = exists, busName = bus.Name });
        }
        return NotFound(new { message = "No seat booked by the user" });
    }

    [HttpGet("bookinghistory")]
    public async Task<IEnumerable> GetUserBookingHistory([FromQuery] Guid userId)
    {
        var finalBookingList = await (from seats in context.ScheduleBusSeats
                                      join scheduleBus in context.ScheduleBuses
                                      on seats.ScheduleId equals scheduleBus.ScheduleId
                                      where seats.BookedBy == userId
                                      select new SeatBookingHistoryDtos
                                      {
                                          busName = scheduleBus.Name,
                                          seatName = seats.SeatName,
                                          scheduledDate = scheduleBus.ScheduledDate,
                                          startingAt = scheduleBus.StartingAt,
                                          startingPlace = scheduleBus.StartingPlace,
                                          theTownBusWas = scheduleBus.theTownBusIsNow
                                      }).ToListAsync();
        return finalBookingList;
    }
    [HttpGet("seatlistdetailforadmin")]
    public async Task<ActionResult<IEnumerable<SeatDetailsForAdminDtos>>> GetSeatListDetailForAdmin([FromQuery] Guid scheduleId)
    {
        // Console.WriteLine("came this far->"+scheduleId);
        try
        {
            var seatHistory = await (from seat in context.ScheduleBusSeats
                                     join student in context.Students
                                     on seat.BookedBy equals student.id into studentGroup
                                     from student in studentGroup.DefaultIfEmpty()
                                     where seat.ScheduleId == scheduleId
                                     select new SeatDetailsForAdminDtos
                                     {
                                         seatName = seat.SeatName,
                                         studentName = student != null ? student.Name : "N/A",
                                         rollNumber = student != null ? student.RollNumber : "N/A",
                                         registrationNumber = student != null ? student.RegistrationNumber : "N/A",
                                         phone = student != null ? student.Phone : "N/A",
                                         departmentName = student != null ? student.DepartmentName : "N/A",
                                         session = student != null ? student.Session : "N/A"
                                     }).ToListAsync();
            return Ok(new { message = "Got seat details for the schedule Bus", list = seatHistory });
        }
        catch
        {
            return StatusCode(500, new { message = "Internal Server Error!" });
        }
    }

    private async Task<IEnumerable<ScheduleBus>> GetCurrentBusList()
    {
        TimeSpan CurrentTime = TimeSpan.Parse(DateTime.Now.ToString("HH\\:mm\\:ss"));
        DateOnly CurrentDate = DateOnly.Parse(DateOnly.FromDateTime(DateTime.Now).ToString("MM-dd-yyyy"));
        List<ScheduleBus>? Allbuses = await context.ScheduleBuses.Where(
        bus => bus.StartingAt >= CurrentTime
        && bus.ScheduledDate == CurrentDate
        ).ToListAsync();

        return Allbuses;
    }
}
