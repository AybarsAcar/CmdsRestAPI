using System;
using System.Collections.Generic;
using System.Linq;
using Cammender.Data;
using Commander.Models;

namespace Commander.Data
{
  public class SqlCommanderRepo : ICommanderRepo
  {
    private readonly CommanderContext _ctx;

    public SqlCommanderRepo(CommanderContext ctx)
    {
      _ctx = ctx;
    }

    public void CreateCommand(Command cmd)
    {
      if (cmd == null)
      {
        throw new ArgumentNullException(nameof(cmd));
      }

      _ctx.Commands.Add(cmd);
    }

    public void DeleteCommand(Command cmd)
    {
      if (cmd == null)
      {
        throw new ArgumentNullException(nameof(cmd));
      }

      _ctx.Commands.Remove(cmd);
    }

    public IEnumerable<Command> GetAllCommands()
    {
      return _ctx.Commands.ToList();
    }

    public Command GetCommandById(int id)
    {
      return _ctx.Commands.FirstOrDefault(p => p.Id == id);
    }

    /* 
    we call save changes in our db context
    data wont be saved in the database unless we call it
     */
    public bool SaveChanges()
    {
      return _ctx.SaveChanges() >= 0;
    }

    public void UpdateCommand(Command cmd)
    {

    }
  }
}