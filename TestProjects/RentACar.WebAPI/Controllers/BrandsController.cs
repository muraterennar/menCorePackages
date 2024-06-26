﻿using MenCore.Application.Request;
using Microsoft.AspNetCore.Mvc;
using RentACar.Application.Features.Brands.Commands.Create;
using RentACar.Application.Features.Brands.Commands.Delete;
using RentACar.Application.Features.Brands.Commands.Update;
using RentACar.Application.Features.Brands.Queries.GetById;
using RentACar.Application.Features.Brands.Queries.GetList;
using RentACar.Application.Services.Repositories;

namespace RentACar.WebAPI.Controllers;

[Route("api/[controller]")]
public class BrandsController : BaseController
{
    protected readonly IBrandRepository _brandRepository;

    public BrandsController(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    [HttpGet("getlist")]
    public async Task<IActionResult> GetLis([FromQuery] PageRequest pageRequest)
    {
        GetListBrandQuery getListBrandQuery = new() { PageRequest = pageRequest };
        var response = await Mediator.Send(getListBrandQuery);
        return Ok(response);
    }

    [HttpGet("getbyid/{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdBrendQuery getByIdBrendQuery = new() { Id = id };
        var response = await Mediator.Send(getByIdBrendQuery);
        return Ok(response);
    }

    [HttpPost("add")]
    public async Task<IActionResult> GetAll([FromBody] CreateBrandCommand createBrandCommand)
    {
        var resposne = await Mediator.Send(createBrandCommand);
        return Ok(resposne);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateBrandCommand updateBrandCommand)
    {
        var response = await Mediator.Send(updateBrandCommand);
        return Ok(response);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromBody] DeletedBrandCommand deletedBrandCommand)
    {
        var response = await Mediator.Send(deletedBrandCommand);
        return Ok(response);
    }
}