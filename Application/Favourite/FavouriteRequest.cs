using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

public sealed record CreateFavouriteRequest(
    [property: Required] int AuthorId,
    [property: Required] int ReaderId
);

public sealed record UpdateFavouriteRequest(
    [property: Required] int AuthorId,
    [property: Required] int ReaderId
);