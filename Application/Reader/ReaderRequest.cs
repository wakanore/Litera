using Application;
using System.ComponentModel.DataAnnotations;

public sealed record CreateReaderRequest(
    [property: Required] int Id,
    [property: Required] string Name,
    [property: Required] string Phone,
    [property: Required] List<CreateBookRequest> Books,
    [property: Required] string Description
);

public sealed record UpdateReaderRequest(
    [property: Required] int Id,
    [property: Required] string Name,
    [property: Required] string Phone,
    [property: Required] List<UpdateBookRequest> Books,
    [property: Required] string Description
);