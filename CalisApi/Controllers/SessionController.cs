using CalisApi.Database.Interfaces;
using CalisApi.Models;
using CalisApi.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace CalisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IUserRepository _userRepository;

        public SessionController(ISessionRepository sessionRepository, IUserRepository userRepository)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
        }

        //obtener todas las clases api/session y filtrar por fecha api/session?datetime=2025-12-25 por ej
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] DateTime? datetime)
        {
            if (datetime.HasValue)
            {
                Debug.WriteLine($"Date to filter: {datetime.Value.Date}");
                var sessions = await _sessionRepository.GetAllSessionsByDate(datetime.Value.Date);
                if (sessions == null)
                {
                    return NoContent();
                }
                return Ok(sessions);
            }
            else
            {
                var all = await _sessionRepository.GetAll();
                return Ok(all);
            }
        }

        //obtener una clase por id api/session/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSessionById(int id)
        {
            var session = await _sessionRepository.GetSessionById(id);
            if (session == null)
            {
                return NotFound("La sesión no existe.");
            }
            return Ok(session);

        }

        //obtener la clase y los usuarios alistados
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetFullSessionDetails(int id)
        {
            var session =  await _sessionRepository.GetSessionById(id);
            if (session == null)
            {
                return NotFound("La sesión no existe.");
            }
            var enrolledUsers = await _sessionRepository.GetEnrolledUsers(id);
            var result = new
            {
                Session = session,
                EnrolledUsers = enrolledUsers
            };
            return Ok(result);

        }

        //obtener usuarios alistados a una clase
        [HttpGet("{id:int?}/Users")]
        public async Task<IActionResult> GetSessionUsers(int id) {
            var exist = await _sessionRepository.GetSessionById(id);
            if(exist == null)
            {
                return NotFound("La sesion no existe");
            }

            try
            {
                var usuarios = await _sessionRepository.GetEnrolledUsers(id);
                return Ok(usuarios);
            }catch(Exception e)
            {
                return StatusCode(500, "Error interno: " + e.Message);
            }

        }


        [Authorize(Roles ="Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateSession([FromBody] SessionDto session)
        {
            var exist = await _sessionRepository.GetSessionByDate(session.Date);
            if (exist != null)
            {
                return Conflict("Ya existe una sesión programada para esta fecha.");
            }
            Session s = new Session
            {
                Title = session.Title,
                Description = session.Description,
                Date = session.Date,
                LimitedSpots = session.LimitedSpots,
                Enrolled = 0
            };
            await _sessionRepository.Create(s);
            return Ok(s);

        }

        
    }
}
