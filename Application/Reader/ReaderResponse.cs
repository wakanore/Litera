using Application;

public record ReaderResponse(
    int Id,
    string Name,
    string Phone,
    List<CreateBookRequest> Books,
    string Description
);