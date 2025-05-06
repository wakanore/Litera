using System.ComponentModel.DataAnnotations;

public sealed record CreateAuthorRequest(
    [property: Required] int Id,
    [property: Required, MaxLength(30)] string Name,
    [property: Required, MaxLength(15)] string Phone
);

public sealed record UpdateAuthorRequest(
    [property: Required] int Id,
    [property: Required, MaxLength(30)] string Name,
    [property: Required, MaxLength(15)] string Phone
);