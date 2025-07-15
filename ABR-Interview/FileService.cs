namespace ABR_Interview;

public class FileService
{
    private readonly GuestSessionService _guestSessionService;

    public FileService(GuestSessionService guestSessionService)
    {
        _guestSessionService = guestSessionService;
    }

    public async Task<List<string>> GetAllFiles(string userId)
    {
        // Retrieve files from some storage
        var files = new List<string>
        {
            "file1.txt",
            "file2.txt",
            "file3.txt"
        };

        return files;
    }

    public async Task<List<string>> GetAllGuestFiles(string userId)
    {
        // Retrieve files from some storage
        var files = new List<string>
        {
            "file1.txt"
        };

        return files;
    }
}
