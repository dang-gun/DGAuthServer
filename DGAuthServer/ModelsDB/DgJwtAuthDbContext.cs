using System;
using Microsoft.EntityFrameworkCore;

namespace DGAuthServer
{
    /// <summary>
    /// 
    /// </summary>
    public class DgJwtAuthDbContext : DbContext
    {
#pragma warning disable CS8618 // 생성자를 종료할 때 null을 허용하지 않는 필드에 null이 아닌 값을 포함해야 합니다. null 허용으로 선언해 보세요.
        public DgJwtAuthDbContext(DbContextOptions<DgJwtAuthDbContext> options)
#pragma warning restore CS8618 // 생성자를 종료할 때 null을 허용하지 않는 필드에 null이 아닌 값을 포함해야 합니다. null 허용으로 선언해 보세요.
			: base(options)
        {
        }

#pragma warning disable CS8618 // 생성자를 종료할 때 null을 허용하지 않는 필드에 null이 아닌 값을 포함해야 합니다. null 허용으로 선언해 보세요.
        public DgJwtAuthDbContext()
#pragma warning restore CS8618 // 생성자를 종료할 때 null을 허용하지 않는 필드에 null이 아닌 값을 포함해야 합니다. null 허용으로 선언해 보세요.
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (null != DGAuthServerGlobal.ActDbContextOnConfiguring)
            {
                DGAuthServerGlobal.ActDbContextOnConfiguring(options);
            }
        }


        /// <summary>
        /// 엑세스 토큰
        /// </summary>
        public DbSet<DgJwtAuthAccessToken> DGAuthServer_AccessToken { get; set; }

        /// <summary>
        /// 리플레시 토큰
        /// </summary>
        public DbSet<DgJwtAuthRefreshToken> DGAuthServer_RefreshToken { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
