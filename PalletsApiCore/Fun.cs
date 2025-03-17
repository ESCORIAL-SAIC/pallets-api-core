using Microsoft.EntityFrameworkCore;
using PalletsApiCore.Models;

namespace PalletsApiCore
{
    public static class Fun
    {

        public static async Task<bool> IsAvailableAsync(int serial, ESCORIALContext context)
        {
            var item = await context.cenker_prod_x_pallet
                .FirstOrDefaultAsync(c => c.serie == serial.ToString());
            var available = item is null;
            return available;
        }
    }
}
