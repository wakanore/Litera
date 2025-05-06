using Application;
using System.ComponentModel.DataAnnotations;

public sealed record CreateBookRequest(
    [property: Required] int Id,
    [property: Required] string Name,
    [property: Required] List<CreateReaderRequest> Readers,
    [property: Required] CreateAuthorRequest Author
);

public sealed record UpdateBookRequest(
    [property: Required] int Id,
    [property: Required] string Name,
    [property: Required] List<CreateReaderRequest> Readers,
    [property: Required] CreateAuthorRequest Author
);