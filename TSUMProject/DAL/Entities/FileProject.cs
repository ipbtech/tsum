namespace TSUMProject.DAL.Entities;

public class FileProject : BaseEntity
{
    public string UrlFile { get; set; } = string.Empty;
    public int ProjectId { get; set; }
    public Project? Project { get; set; }
}

