using backend.DTO.CashIn;
using backend.DTO.CashOut;

namespace backend.Interfaces;

public interface ICashService
{
    Task<bool> CashInAsync(CashInDto dto);

    Task<List<CashInListDto>> GetAllCashInAsync(int? year = null, int? month = null);

    Task<bool> CashOutAsync(CashOutDto dto);

    Task<List<CashOutDto>> GetAllCashOutAsync(int? year = null, int? month = null);

}
