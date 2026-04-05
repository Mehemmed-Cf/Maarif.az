using Application.Modules.RoomsModule.Commands.RoomAddCommand;
using Application.Modules.RoomsModule.Commands.RoomEditCommand;
using Application.Modules.RoomsModule.Commands.RoomRemoveCommand;
using Application.Modules.RoomsModule.Queries.RoomGetAllQuery;
using Application.Modules.RoomsModule.Queries.RoomGetByIdQuery;
using Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomsController : AdminBaseController
    {
        private readonly IRoomRepository roomRepository;
        private readonly IBuildingRepository buildingRepository;
        private readonly IMediator mediator;

        public RoomsController(IRoomRepository roomRepository, IBuildingRepository buildingRepository, IMediator mediator)
        {
            this.roomRepository = roomRepository;
            this.buildingRepository = buildingRepository;
            this.mediator = mediator;
        }

        private async Task PopulateViewBagsAsync(int? selectedBuildingId = null)
        {
            var buildings = await Task.Run(() => buildingRepository.GetAll().ToList());
            ViewBag.Buildings = new SelectList(buildings, "Id", "Name", selectedBuildingId);
        }

        public async Task<IActionResult> Index()
        {
            var response = await mediator.Send(new RoomGetAllRequest());
            return View(response);
        }

        public async Task<IActionResult> Details([FromRoute] RoomGetByIdRequest request)
        {
            var response = await mediator.Send(request);
            return View(response);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateViewBagsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomAddRequest request)
        {
            if (!ModelState.IsValid)
            {
                await PopulateViewBagsAsync(request.BuildingId);
                return View(request);
            }

            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await mediator.Send(new RoomGetByIdRequest { Id = id });

            var editRequest = new RoomEditRequest
            {
                Id = response.Id,
                Number = response.Number,
                BuildingId = response.BuildingId
            };

            await PopulateViewBagsAsync(response.BuildingId);

            return View(editRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoomEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                await PopulateViewBagsAsync(request.BuildingId);
                return View(request);
            }

            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            await mediator.Send(new RoomRemoveRequest { Id = id });
            return Ok(new { message = "Room successfully deleted" });
        }
    }
}
