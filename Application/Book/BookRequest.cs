using System.ComponentModel.DataAnnotations;

public sealed record CreateBookRequest(
    int Id,
    [Required] string Name,
    [Required] string Style,
    [Required] int AuthorId
);

public sealed record UpdateBookRequest(
    int Id,
    [Required] string Name,
    [Required] List<BookReaderDto> Readers,
    [Required] BookAuthorDto Author
);

public sealed record BookReaderDto(
    [Required] int Id,
    [Required] string Name
);


public sealed record BookAuthorDto(
    [Required] int Id,
    [Required] string Name,
    [Required] string Phone
);