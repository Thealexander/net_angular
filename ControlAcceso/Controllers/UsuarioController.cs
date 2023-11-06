﻿using ControlAcceso.Core.Application;
using ControlAcceso.Core.DTO;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControlAcceso.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private readonly IMediator _mediator;

        public UsuarioController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UsuarioDTO>> Registrar(Register.UsuarioRegisterCommand parametros)
        {
            return await _mediator.Send(parametros);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDTO>> Login(Login.UsuarioLoginCommand parametros)
        {
            return await _mediator.Send(parametros);
        }

        [HttpGet]
        public async Task<ActionResult<UsuarioDTO>> Get()
        {
            return await _mediator.Send(new ActualUser.UsuarioActualCommnad());
        }

    }
}
