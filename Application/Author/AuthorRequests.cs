public sealed record CreateAuthorRequest(
    int Id,
    string Name,
    string Phone
);

public sealed record UpdateAuthorRequest(
    int Id,
    string Name,
    string Phone
);
