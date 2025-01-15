using AppData.IRepository;
using AppData.IService;
using AppData.Repository;
using AppData.Service;
using AppData;
using Microsoft.EntityFrameworkCore;

using Net.payOS;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = false,
			ValidateAudience = false,
			ValidateLifetime = true,  // Kiểm tra thời gian hết hạn của token
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SecretKey"])) // Key xác thực
		};
	});
builder.Services.AddSwaggerGen(c =>
{
	c.CustomSchemaIds(type => type.FullName); // Sử dụng FullName (bao gồm namespace)
});

// Add services to the container.
builder.Services.AddDbContext<MyDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllersWithViews(); // Thay vì AddControllers
builder.Services.AddSession();
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", builder =>
	{
		builder.AllowAnyOrigin()
			   .AllowAnyMethod()
			   .AllowAnyHeader();
	});
});
IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

PayOS payOS = new PayOS(configuration["Environment:PAYOS_CLIENT_ID"] ?? throw new Exception("Cannot find environment"),
					configuration["Environment:PAYOS_API_KEY"] ?? throw new Exception("Cannot find environment"),
					configuration["Environment:PAYOS_CHECKSUM_KEY"] ?? throw new Exception("Cannot find environment"));

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", builder =>
	{
		builder.AllowAnyOrigin()
			   .AllowAnyMethod()
			   .AllowAnyHeader();
	});
});
/*builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});*/
builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
	options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddScoped<IphuongthucthanhtoanRepos, PhuongthucthanhtoanRepos>();
builder.Services.AddScoped<IphuongthucthanhtoanServicee, PhuongthucthanhtoanService>();
builder.Services.AddScoped<IGiamgiaRepos, GiamgiaRepos>();
builder.Services.AddScoped<IGiamgiaService, GiamgiaService>();
builder.Services.AddScoped<INhanvienRepos, NhanvienRepos>();
builder.Services.AddScoped<INhanvienService, NhanvienService>();
builder.Services.AddScoped<IsaleRepos, SaleRepos>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<IsalechitietRepos, SaleechitietRepos>();
builder.Services.AddScoped<ISalechitietService, SalechitietService>();
builder.Services.AddScoped<IsanphamRepos, SanphamRepos>();
builder.Services.AddScoped<ISanPhamservice, SanphamService>();
builder.Services.AddScoped<IThuonghieuRepos, ThuonghieuRepos>();
builder.Services.AddScoped<IthuonghieuService, ThuonghieuService>();
builder.Services.AddScoped<IThuoctinhRepository, ThuoctinhRepository>();
builder.Services.AddScoped<IThuoctinhService, ThuoctinhService>();
builder.Services.AddScoped<ISanphamchitietRepository, SanphamchitietRepository>();
builder.Services.AddScoped<ISanphamchitietService, SanphamchitietService>();
builder.Services.AddScoped<IthuoctinhsanphamchitietService, ThuoctinhsanphamchitietService>();
builder.Services.AddScoped<IthuoctinhsanphamchitietRepository, ThuoctinhsanphamRepository>();
builder.Services.AddScoped<Ikhachhangrepository, KhachhangRepostory>();
builder.Services.AddScoped<IKhackhangservice, Khachhangservice>();
builder.Services.AddScoped<IRankService, RankService>();
builder.Services.AddScoped<IRankRepository, RankRepository>();
builder.Services.AddScoped<IHoadonRepository, HoadonRepository>();
builder.Services.AddScoped<IHoadonService, HoadonService>();
builder.Services.AddScoped<IThongkesanphamRepository, ThongkeRepository>();
builder.Services.AddScoped<IThongkeService, ThongkeService>();
builder.Services.AddScoped<IDanhgiaRepository, DanhgiaRepository>();
builder.Services.AddScoped<IDanhgiaService, DanhgiaService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<KhachHang_IDanhGiaRepos, KhachHang_DanhGiaRepos>();
builder.Services.AddScoped<KhachHang_IDanhGiaServices, KhachHang_DanhGiaServices>();
builder.Services.AddScoped<KhachHang_IDiaChiRepos, KhachHang_DiaChiRepos>();
builder.Services.AddScoped<KhachHang_IDiaChiService, KhachHang_DiaChiService>();
builder.Services.AddScoped<KhachHang_IGiamgiaRepos, KhachHang_GiamgiaRepos>();
builder.Services.AddScoped<KhachHang_IGiamgiaService, KhachHang_GiamgiaService>();
builder.Services.AddScoped<KhachHang_IGiohangchitietRepos, KhachHang_GiohangchitietRepos>();
builder.Services.AddScoped<KhachHang_IGiohangchitietService, KhachHang_GiohangchitietService>();
builder.Services.AddScoped<KhachHang_IGiohangRepos, KhachHang_GiohangRepos>();
builder.Services.AddScoped<KhachHang_IGiohangService, KhachHang_GiohangService>();
builder.Services.AddScoped<KhachHang_IHoaDonChiTietRepository, HoaDonChiTietRepos>();
builder.Services.AddScoped<KhachHang_IHoaDonChiTietService, KhachHang_HoaDonChiTietService>();
builder.Services.AddScoped<KhachHang_IHoadonRepository, KhachHang_HoadonRepos>();
builder.Services.AddScoped<KhachHang_IHoadonService, KhachHang_HoaDonService>();
builder.Services.AddScoped<KhachHang_IKhachhangRepos, KhachHang_KhachhangRepos>();
builder.Services.AddScoped<KhachHang_IKhachhangService, KhachHang_KhachhangService>();
builder.Services.AddScoped<KhachHang_InhacungcapRepos, KhachHang_NhacungcapRepos>();
builder.Services.AddScoped<KhachHang_InhacungcapService, KhachHang_NhacungcapService>();
builder.Services.AddScoped<KhachHang_INhanvienRepos, KhachHang_NhanvienRepos>();
builder.Services.AddScoped<KhachHang_INhanvienService, KhachHang_NhanvienService>();
builder.Services.AddScoped<KhachHang_IphuongthucthanhtoanRepos, KhachHang_PhuongthucthanhtoanRepos>();
builder.Services.AddScoped<KhachHang_IphuongthucthanhtoanServicee, KhachHang_PhuongthucthanhtoanService>();
builder.Services.AddScoped<KhachHang_IRankRepos, KhachHang_RankRepos>();
builder.Services.AddScoped<KhachHang_IRankServiece, KhachHang_RankSevi>();
builder.Services.AddScoped<KhachHang_IsalechitietRepos, KhachHang_SaleechitietRepos>();
builder.Services.AddScoped<KhachHang_ISalechitietService, KhachHang_SalechitietService>();
builder.Services.AddScoped<KhachHang_IsaleRepos, KhachHang_SaleRepos>();
builder.Services.AddScoped<KhachHang_ISaleService, KhachHang_SaleService>();
builder.Services.AddScoped<KhachHang_ISanphamchitietRepos, KhachHang_SanphamchitietRepos>();
builder.Services.AddScoped<KhachHang_ISanphamchitietService, KhachHang_SanphamchitietService>();
builder.Services.AddScoped<KhachHang_IsanphamRepos, KhachHang_SanphamRepos>();
builder.Services.AddScoped<KhachHang_ISanPhamservice, KhachHang_SanphamService>();
builder.Services.AddScoped<KhachHang_IThuocTinhRepos, KhachHang_ThuocTinhRepos>();
builder.Services.AddScoped<KhachHang_IThuoctinhService, KhachHang_ThuocTinhService>();
builder.Services.AddScoped<KhachHang_IThuongHieuRepos, KhachHang_ThuongHieuRepos>();
builder.Services.AddScoped<KhachHang_IThuongHieuService, KhachHang_ThuongHieuService>();
builder.Services.AddScoped<KhachHang_ITraHangChiTietRepos, KhachHang_TraHangChiTietRepos>();
builder.Services.AddScoped<KhachHang_ITraHangChiTietService, KhachHang_TraHangChiTietService>();
builder.Services.AddScoped<KhachHang_ITraHangRepos, KhachHang_TraHangRepos>();
builder.Services.AddScoped<KhachHang_ITraHangService, KhachHang_TraHangService>();
builder.Services.AddScoped<KhachHang_IGiamgia_RankRepos, KhachHang_Giamgia_RankRepos>();
builder.Services.AddScoped<KhachHang_IGiamgia_RankService, KhachHang_Giamgia_RankService>();
builder.Services.AddScoped<KhachHang_IHinhanhRepos, KhachHang_HinhanhRepos>();
builder.Services.AddScoped<KhachHang_IHinhanhService, KhachHang_HinhanhService>();
builder.Services.AddScoped<KhachHang_ILichsuthanhtoanRepos, KhachHang_LichsuthanhtoanRepos>();
builder.Services.AddScoped<KhachHang_ILichsuthanhtoanService, KhachHang_LichsuthanhtoanService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton(payOS);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.Use(async (context, next) =>
{
	// Thêm headers vào response
	context.Response.Headers.Add("Cross-Origin-Opener-Policy", "same-origin");
	context.Response.Headers.Add("Cross-Origin-Embedder-Policy", "require-corp");
	context.Response.Headers.Add("Content-Security-Policy",
		"default-src 'self'; " +
		"script-src 'self' https://apis.google.com https://www.gstatic.com; " +
		"style-src 'self' 'unsafe-inline'; " +
		"img-src 'self' data:; " +
		"font-src 'self'; " +
		"connect-src 'self'; " +
		"frame-src https://accounts.google.com;");

	await next();
});
app.UseRouting();
app.UseStaticFiles();
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseCors("AllowAll"); // S? d?ng chính sách CORS
app.UseSession();
app.UseAuthorization();

app.MapControllers();

app.Run();
app.UseStaticFiles(new StaticFileOptions
{
	OnPrepareResponse = ctx =>
	{
		ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=604800");
	}
});
