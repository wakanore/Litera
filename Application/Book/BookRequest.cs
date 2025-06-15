public sealed record CreateBookRequest(
    int Id,
    string Name,
    string Style,
    int AuthorId
);

public sealed record UpdateBookRequest(
    int Id,
    string Name,
    string Style,
    int AuthorId
);
