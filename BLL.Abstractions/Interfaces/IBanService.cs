using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstractions.cs.Interfaces
{
    public interface IBanService
    {
        Task<IEnumerable<BanDTO>> GetAllBans();
        Task<BanDTO> GetBanById(int id);
        Task<BanDTO> GetBanByUserId(string userId);
        Task<BanDTO> BanUser(CreateBanDTO banDTO);
        Task<BanDTO> UpdateExistingBan(UpdateBanDTO banDTO);
        Task DeleteBanById(int id);
        Task DeleteBanByUserId(string id);
    }
}
