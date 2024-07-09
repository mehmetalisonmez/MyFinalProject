using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business.Abstract;
using Business.Concrete;
using Business.DependencyResolvers.Autofac;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new AutofacBusinessModule());
});   //12-15 satýrlarý arasý gözümüzü korkutmasýn hoca bile ezbere bilmiyor sadece Autofac'deki IoC ' yi kullanacaðýmýzý belirten koddur

// Add services to the container.
builder.Services.AddControllers(); // Controller'larý eklemek için bu satýrý ekleyin.
//builder.Services.AddSingleton<IProductService, ProductManager>(); //AddSingleton(IProductService, ProductManager) : Birisi senden IProductSerbice isterse arkada ona ProductManager new'i oluþtur onu ver!
//builder.Services.AddSingleton<IProductDal, EfProductDal>(); //bu .net ' in kendi IoC containerýdýr. (11.Ders ile bu þekilde yaptýðýmýz IoC'yi 12çDers ile birlikte Autofac ile yapýp AOP programlamaya uygun yapacaðýz.
//Autofac, Ninject, CastleWindsor, StructureMap, LightInject, DryInjext --> IoC container (Biz .net altyapýsý yerine bunlardan birini kullanacaðýz bunun nedeni ileride AOP programlama yapacaðýmýz.)
//AOP = Bir metodun önünde, sonunda metod hata verdiðinde çalýþan kod parçalarýný AOP mimarileriyle yazýlýr.


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAll"); // CORS ayarlarýný ekleyin.

app.UseAuthorization();

app.MapControllers(); // Controller'larý haritalamak için bu satýrý ekleyin.
app.MapRazorPages(); // Razor Pages kullanýyorsanýz bu satýrý ekleyin.

app.Run();
