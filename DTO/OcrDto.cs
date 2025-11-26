namespace template_backend.Models.DTOs;

public record OcrMedicineResult(string Name, int Qty = 1);

public record OcrMatchResult(
    bool Matched,
    Guid? ProductId,
    string? ProductName,
    string Name,
    int Qty,
    string? Reason
);
