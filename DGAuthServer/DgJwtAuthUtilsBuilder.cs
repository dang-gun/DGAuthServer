
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DGAuthServer;

/// <summary>
/// DG Auth Server Service�� ���� ���� ����
/// </summary>
public static class DgJwtAuthUtilsBuilder
{
    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="services"></param>
    /// <param name="settingData"></param>
    /// <param name="actDbContextOnConfiguring">
    /// DB ���ý�Ʈ ������ ���� 'OnConfiguring'�׼�<br />
    /// ���� DB�� 'DGAuthServer_AccessToken'�� 'DGAuthServer_RefreshToken' ���̺��� ������
    /// �ش� ���̺��� �����ȴ�.<br />
    /// �ʿ信 ���� �ڽ��� ���ý�Ʈ(��)�� �ش� ���̺��� �����Ͽ� ������ �� �ִ�.
    /// </param>
    /// <returns></returns>
    public static IServiceCollection AddDgAuthServerBuilder(
        this IServiceCollection services
        , DgAuthSettingModel settingData
        , Action<DbContextOptionsBuilder>? actDbContextOnConfiguring)
    {

        //���� ������ ����
        DGAuthServerGlobal.Setting.ToCopy(settingData);

        //builder.addcl
        //services.Configure<DgJwtAuthSettingModel>();

        //�ɼ� ����
        services.Configure<DgAuthSettingModel>(options =>
        {
            options.ToCopy(DGAuthServerGlobal.Setting);
        });

        if (null != actDbContextOnConfiguring)
        {
            DGAuthServerGlobal.ActDbContextOnConfiguring
                = actDbContextOnConfiguring;
        }
        else
        {
            //��ü������ ����� ������ ���̽�
            DGAuthServerGlobal.ActDbContextOnConfiguring
                = (options => options.UseInMemoryDatabase(databaseName: "DGAuthServer_DB"));
        }

        //���̺� ����
        using (DgAuthDbContext db1 = new DgAuthDbContext())
        {
            db1.Database.EnsureCreated();
        }


        return services;
    }

    /// <summary>
    /// ���ø����̼�(�̵����) ����
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseDgAuthServerAppBuilder(
        this IApplicationBuilder app)
    {

        //JwtAuth �̵���� ����
        app.UseMiddleware<DgJwtAuthMiddleware>();
        return app;
    }
}
