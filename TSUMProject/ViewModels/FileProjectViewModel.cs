namespace TSUMProject.ViewModels;

public class FileProjectViewModel : BaseViewModel
{
    public string UrlFile { get; set; } = string.Empty;
    public int ProjectId { get; set; }
    public ProjectViewModel Project { get; set; } = new ProjectViewModel();
}