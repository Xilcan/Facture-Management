using Data.Models;

namespace Bussines.Services.Interfaces;
public interface IPdfService
{
    byte[] CreatePdfFile(Facture facture);
}
