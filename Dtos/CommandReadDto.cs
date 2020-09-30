namespace Commander.Dtos
{

  /* 
  these are the fields that we want to expose to the front end
  we took out the Platfor just for the sake of it
   */
  public class CommandReadDto
  {
    public int Id { get; set; }

    public string HowTo { get; set; }

    public string Line { get; set; }

  }
}