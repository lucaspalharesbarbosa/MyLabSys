using Microsoft.EntityFrameworkCore;

namespace MyLabSys.Models {
    public class MyLabSysContext : DbContext {
        public MyLabSysContext(DbContextOptions<MyLabSysContext> options) : base(options) { }
    }
}