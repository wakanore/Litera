using System.ComponentModel.DataAnnotations;

public sealed record CreateAuthorRequest(
    [Required] int Id,
    [Required, MaxLength(30)] string Name,
    [Required, MaxLength(15)] string Phone
);

public sealed record UpdateAuthorRequest(
    [Required] int Id,
    [Required, MaxLength(30)] string Name,
    [Required, MaxLength(15)] string Phone
);