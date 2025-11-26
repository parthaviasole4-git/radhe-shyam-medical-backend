using Microsoft.EntityFrameworkCore;
using template_backend.Data;
using template_backend.Models;
using template_backend.Models.DTOs;

public class OcrService
{
    private readonly AppDbContext _db;

    public OcrService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<OcrMedicineResult>> ExtractMedicinesAsync(IFormFile file)
    {
        // TODO: Replace with actual OCR later
        return new List<OcrMedicineResult>
        {
            new("Paracitamol 500", 2),
            new("Azee 500", 1),
            new("Amox 500", 1)
        };
    }

    public async Task<List<OcrMatchResult>> MatchAsync(List<OcrMedicineResult> input)
    {
        var products = await _db.Products.ToListAsync();
        const int threshold = 4;

        return input.Select(med =>
        {
            var best = products
                .Select(p => (p, score: Levenshtein(med.Name, p.Name)))
                .OrderBy(x => x.score)
                .FirstOrDefault();

            if (best.p == null || best.score > threshold)
                return new OcrMatchResult(
                    Matched: false,
                    ProductId: null,
                    ProductName: null,
                    Name: med.Name,
                    Qty: med.Qty,
                    Reason: "Not found in database"
                );

            return new OcrMatchResult(
                Matched: true,
                ProductId: best.p.Id,
                ProductName: best.p.Name,
                Name: med.Name,
                Qty: med.Qty,
                Reason: null
            );

        }).ToList();
    }

    private int Levenshtein(string a, string b)
    {
        int[,] d = new int[a.Length + 1, b.Length + 1];

        for (int i = 0; i <= a.Length; i++) d[i, 0] = i;
        for (int j = 0; j <= b.Length; j++) d[0, j] = j;

        for (int i = 1; i <= a.Length; i++)
            for (int j = 1; j <= b.Length; j++)
                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + (a[i - 1] == b[j - 1] ? 0 : 1)
                );

        return d[a.Length, b.Length];
    }
}
