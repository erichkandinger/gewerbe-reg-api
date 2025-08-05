namespace GewerbeRegApi.Models;

public class AddressItem
{
    public long PLZ { get; set; }
    public string? Ort { get; set; }
    public string? Land { get; set; }
}

public class PLZRequest
{
    public string? Staat { get; set; }

    public string? PLZ { get; set; }
}

public class PLZResponse
{
    public PLZResponse(string? plz, string? ort, string? gemeinde)
    {
        PLZ = plz;
        Ort = ort;
        Gemeinde = gemeinde;
    }

    public string? PLZ { get; set; }
    public string? Ort { get; set; }
    public string? Gemeinde { get; set; }
}