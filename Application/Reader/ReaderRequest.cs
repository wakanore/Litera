public sealed record CreateReaderRequest(
     int Id,
     string Name,
     string Phone
);

public sealed record UpdateReaderRequest(
     int Id,
     string Name,
     string Phone
);