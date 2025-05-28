using System.ComponentModel.DataAnnotations;

public sealed record CreateReaderRequest(
    [Required] int Id,
    [Required, MaxLength(30)] string Name,
    [Required, MaxLength(15)] string Phone
);

public sealed record UpdateReaderRequest(
    [Required] int Id,
    [Required, MaxLength(30)] string Name,
    [Required, MaxLength(15)] string Phone
);