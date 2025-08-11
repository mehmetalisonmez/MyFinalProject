using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Business.Abstract;
using Business.Concrete;
using Business.DependencyResolvers.Autofac;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Core.Utilities.Security.JWT;
using Core.Utilities.IoC;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Core.Utilities.Security.Encryption;
using Castle.Core.Configuration;
using Core.Extensions;
using Core.DependencyResolvers;
using Microsoft.AspNetCore.Mvc.Filters;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new AutofacBusinessModule());
});   //22-25 satýrlarý arasý gözümüzü korkutmasýn hoca bile ezbere bilmiyor sadece Autofac'deki IoC ' yi kullanacaðýmýzý belirten koddur



// Add services to the container.
builder.Services.AddControllers(); // Controller'larý eklemek için bu satýrý ekleyin.
//builder.Services.AddSingleton<IProductService, ProductManager>(); //AddSingleton(IProductService, ProductManager) : Birisi senden IProductSerbice isterse arkada ona ProductManager new'i oluþtur onu ver!
//builder.Services.AddSingleton<IProductDal, EfProductDal>(); //bu .net ' in kendi IoC containerýdýr. (11.Ders ile bu þekilde yaptýðýmýz IoC'yi 12çDers ile birlikte Autofac ile yapýp AOP programlamaya uygun yapacaðýz.
//Autofac, Ninject, CastleWindsor, StructureMap, LightInject, DryInjext --> IoC container (Biz .net altyapýsý yerine bunlardan birini kullanacaðýz bunun nedeni ileride AOP programlama yapacaðýmýz.)
//AOP = Bir metodun önünde, sonunda metod hata verdiðinde çalýþan kod parçalarýný AOP mimarileriyle yazýlýr.
// Add the custom service collection extension
builder.Services.AddDependencyResolvers(new ICoreModule[] {  //ileride CoreModule gibi farklý Module'ler oluþtutrsak onlarý kolayca injection yapabilecez
    // Buraya modüllerinizi ekleyin, örneðin:
    new CoreModule()
});



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")    //parametre olarak verilen adresten
                   .AllowAnyMethod()
                   .AllowAnyHeader();  //get , post ne istek gelirse kabul et ben bu web sayfasýna güveniyorum. (bu satýr bu iþe yarar).
        });
});

var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = tokenOptions.Issuer,
            ValidAudience = tokenOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
        };
    });

// Add Razor Pages services if you need them
builder.Services.AddRazorPages();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.ConfigureCustomExceptionMiddleware(); //Yazdýðýmýz middleware sayesinde tüm sistemimiz try catcth içinde

app.UseCors("AllowAll"); // CORS ayarlarýný ekleyin.

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//middleware : ASPNET yaþam döngüsünde hangi yapýlarýn sýrasýyla devreye gireceðini söylüyoruz
app.UseAuthentication(); // Kimlik doðrulama middleware'ini ekleyin. 
app.UseAuthorization();

app.MapControllers(); // Controller'larý haritalamak için bu satýrý ekleyin.
app.MapRazorPages(); // Razor Pages kullanýyorsanýz bu satýrý ekleyin.

app.Run();
