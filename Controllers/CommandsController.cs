using System.Collections.Generic;
using AutoMapper;
using Cammender.Data;
using Commander.Data;
using Commander.Dtos;
using Commander.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Commander.Controllers
{
  [Route("api/commands")]
  [ApiController]
  public class CommandsController : ControllerBase
  {

    private readonly ICommanderRepo _repo;
    private readonly IMapper _mapper;

    public CommandsController(ICommanderRepo repository, IMapper mapper)
    {
      _repo = repository;
      _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
    {
      var commandItems = _repo.GetAllCommands();

      return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
    }

    [HttpGet("{id}", Name = "GetCommandById")]
    public ActionResult<CommandReadDto> GetCommandById(int id)
    {
      var commandItem = _repo.GetCommandById(id);

      if (commandItem != null)
      {
        return Ok(_mapper.Map<CommandReadDto>(commandItem));
      }
      return NotFound();
    }

    [HttpPost]
    public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto cmd)
    {
      // map the input to our Command model
      var commandModel = _mapper.Map<Command>(cmd);

      _repo.CreateCommand(commandModel);
      _repo.SaveChanges();

      // change that back to read mode
      var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

      return CreatedAtRoute(nameof(GetCommandById),
      new { Id = commandReadDto.Id }, commandReadDto);

    }

    [HttpPut("{id}")]
    public ActionResult UpdateCommand(int id, CommandUpdateDto cmd)
    {
      // check whether the resource exists
      var commandModelFromRepo = _repo.GetCommandById(id);

      if (commandModelFromRepo == null)
      {
        return NotFound();
      }

      // maps from the cmd we pass in to the commandModelFromRepo we have found
      _mapper.Map(cmd, commandModelFromRepo);

      // at this point changes are made but it is good practice to call it
      _repo.UpdateCommand(commandModelFromRepo);

      // then save it
      _repo.SaveChanges();

      return NoContent();
    }

    [HttpPatch("{id}")]
    public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
    {
      // check whether the resource exists
      var commandModelFromRepo = _repo.GetCommandById(id);

      if (commandModelFromRepo == null)
      {
        return NotFound();
      }

      // create a new command update dto
      var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo);

      patchDoc.ApplyTo(commandToPatch, ModelState);

      // validation on patch
      if (!TryValidateModel(commandToPatch))
      {
        return ValidationProblem(ModelState);
      }

      // update the data in the model repo now
      _mapper.Map(commandToPatch, commandModelFromRepo);

      _repo.UpdateCommand(commandModelFromRepo);

      // then save it
      _repo.SaveChanges();

      return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteCommand(int id)
    {
      // check whether the resource exists
      var commandModelFromRepo = _repo.GetCommandById(id);

      if (commandModelFromRepo == null)
      {
        return NotFound();
      }

      // if found delete
      _repo.DeleteCommand(commandModelFromRepo);
      _repo.SaveChanges();

      return NoContent();
    }

  }
}