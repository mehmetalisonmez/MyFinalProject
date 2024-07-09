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
});   //12-15 sat�rlar� aras� g�z�m�z� korkutmas�n hoca bile ezbere bilmiyor sadece Autofac'deki IoC ' yi kullanaca��m�z� belirten koddur

// Add services to the container.
builder.Services.AddControllers(); // Controller'lar� eklemek i�in bu sat�r� ekleyin.
//builder.Services.AddSingleton<IProductService, ProductManager>(); //AddSingleton(IProductService, ProductManager) : Birisi senden IProductSerbice isterse arkada ona ProductManager new'i olu�tur onu ver!
//builder.Services.AddSingleton<IProductDal, EfProductDal>(); //bu .net ' in kendi IoC container�d�r. (11.Ders ile bu �ekilde yapt���m�z IoC'yi 12�Ders ile birlikte Autofac ile yap�p AOP programlamaya uygun yapaca��z.
//Autofac, Ninject, CastleWindsor, StructureMap, LightInject, DryInjext --> IoC container (Biz .net altyap�s� yerine bunlardan birini kullanaca��z bunun nedeni ileride AOP programlama yapaca��m�z.)
//AOP = Bir metodun �n�nde, sonunda metod hata verdi�inde �al��an kod par�alar�n� AOP mimarileriyle yaz�l�r.


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

app.UseCors("AllowAll"); // CORS ayarlar�n� ekleyin.

app.UseAuthorization();

app.MapControllers(); // Controller'lar� haritalamak i�in bu sat�r� ekleyin.
app.MapRazorPages(); // Razor Pages kullan�yorsan�z bu sat�r� ekleyin.

app.Run();
