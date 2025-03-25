using Microsoft.EntityFrameworkCore;
using PalletsApiCore.Models;

namespace PalletsApiCore
{
    public static class Fun
    {

        public static async Task<bool> IsAvailableAsync(int serial, ESCORIALContext context)
        {
            var exists = await context.cenker_prod_x_pallet
                .AnyAsync(c => c.serie == serial.ToString() && c.activo);
            return !exists;
        }
    }
}
