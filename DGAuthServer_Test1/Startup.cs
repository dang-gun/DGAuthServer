using WebApi_JwtAuthTest.Global;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using DGAuthServer;

namespace WebApi_JwtAuthTest
{
	public class Startup
	{
        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            //전달받은 'appsettings.json'백업
            Configuration = configuration;

            //DB 커낵션 스트링 받아오기
            string sConnectStringSelect = "Test_sqlite";
            GlobalStatic.DBType = Configuration[sConnectStringSelect + ":DBType"].ToLower();
            GlobalStatic.DBString = Configuration[sConnectStringSelect + ":ConnectionString"];

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //API모델을 파스칼 케이스 유지하기
            services.AddControllers().AddNewtonsoftJson(options => { options.SerializerSettings.ContractResolver = new DefaultContractResolver(); });


            //Jwt Auth Setting 정보 전달
            //appsettings.json 의 내용 전달
            //services.Configure<DgJwtAuthSettingModel>(Configuration.GetSection("JwtSecretSetting"));
            services.AddDgAuthServerBuilder(
                new DgAuthSettingModel()
                {
                    Secret = Configuration["JwtSecretSetting:Secret"],
                    //개인 시크릿 허용
                    SecretAlone = true,

                    //테스트를 위해 60초로 설정
                    AccessTokenLifetime = 60,
                    AccessTokenCookie = true,
                    RefreshTokenCookie = true,
                }
                , (options => options.UseSqlite("Data Source=DGAuthServerTest.db")));
                //, null);


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //스웨거 사용
                //app.UseSwagger();
                //app.UseSwaggerUI();
            }

            //DGAuthServerService 빌더
            app.UseDgAuthServerAppBuilder();

            //스웨거 사용
            app.UseSwagger();
            app.UseSwaggerUI();

            //3.0 api 라우트
            app.UseRouting();

            //기본 페이지
            app.UseDefaultFiles();
            //wwwroot
            app.UseStaticFiles();


            //3.0 api 라우트 끝점
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
